using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace FLib
{
    public class TwoWordLineInfo
    {
        public IConfiguration Config { get; set; }

        public bool IsSuccess { get; set; }

        public WordInfo LeftWord { get; set; }
        public WordInfo RightWord { get; set; }

        public bool IsHaveYes { get; set; }

        public bool IsHaveNo { get; set; }

        public bool IsDis => !IsHaveYes && IsHaveNo;

        public TwoWordLineInfo() { }

        public TwoWordLineInfo(string line, IConfiguration config = null)
        {
            Config = config;

            string regex = GetParrern();
            var items = Regex.Matches(line, regex, RegexOptions.IgnoreCase);

            if (items.Count > 0)
            {
                LeftWord = new WordInfo()
                {
                    Content = items[0].Groups["name1"].Value,
                };
                TypeRegnize(LeftWord);
                RightWord = new WordInfo()
                {
                    Content = items[0].Groups["name2"].Value,
                };
                TypeRegnize(RightWord);
                IsHaveYes = !string.IsNullOrWhiteSpace(items[0].Groups["yes"].Value);

                IsHaveNo = !string.IsNullOrWhiteSpace(items[0].Groups["not"].Value);

                IsSuccess = true;
            }
        }

        private void TypeRegnize(WordInfo word)
        {
            string[] keys= Config["TwoWordLine_NullWord"].Trim(',').Split(',');
            word.WType = keys.ToList().Contains(word.Content) ? WordType.空值 : WordType.普通;
        }

        private string GetParrern() => $"({GetHeaders()})(?<name1>.*?)和(?<name2>.*?)(?<yes>能{{0,1}})(?<not>不{{0,1}})能({GetEnds()})";
        
        private string GetEnds()
        {
            if (Config == null)
            {
                return "\\s";
            }
            return Config["TwoWordLine_End"].Trim(',').Replace(",", "|");
        }

        private string GetHeaders()
        {
            if (Config == null)
            {
                return "\\s";
            }
            return Config["TwoWordLine_Header"].Trim(',').Replace(",", "|");
        }
    }
}
