using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SWMP.LikeMatch
{
    public class TwoWordLineInfo
    {
        public NameValueCollection Config { get; set; }

        public bool IsSuccess { get; set; }

        public WordInfo LeftWord { get; set; }
        public WordInfo RightWord { get; set; }

        public bool IsHaveYes { get; set; }

        public bool IsHaveNo { get; set; }

        public bool IsDis => !IsHaveYes && IsHaveNo;

        public TwoWordLineInfo() { }

        public TwoWordLineInfo(string line, NameValueCollection config = null)
        {
            Config = config ?? System.Configuration.ConfigurationManager.AppSettings;

            //模式1：
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

            if (!IsSuccess)
            {
                //模式4：不能和xx,能和xx,和XX
                regex = $"(?<not>不{{0,1}})(可以|能|^)(和|与)(?<name>.*?)({GetEnds()})";
                items = Regex.Matches(line, regex, RegexOptions.IgnoreCase);
                if (items.Count > 0)
                {
                    LeftWord = new WordInfo()
                    {
                        Content = items[0].Groups["name"].Value,
                    };
                    TypeRegnize(LeftWord);
                    RightWord = new WordInfo()
                    {
                        WType = WordType.空值
                    };
                    // IsHaveYes = !string.IsNullOrWhiteSpace(items[0].Groups["yes"].Value);

                    IsHaveNo = !string.IsNullOrWhiteSpace(items[0].Groups["not"].Value);
                    IsSuccess = true;
                }
            }

            if (!IsSuccess)
            {
                //模式2：
                regex = $"^(?<name1>.*?)({GetAnd()})(?<name2>.*?)$";
                items = Regex.Matches(line, regex, RegexOptions.IgnoreCase);
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
                    IsSuccess = true;
                }
            }


            if (!IsSuccess)
            {
                //模式3：
                regex = $"(?<name>.*?)(?<yes>(能|可以){{0,1}})(?<not>不{{0,1}})(能|可以)({GetEnds()})";
                items = Regex.Matches(line, regex, RegexOptions.IgnoreCase);
                if (items.Count > 0)
                {
                    LeftWord = new WordInfo()
                    {
                        Content = items[0].Groups["name"].Value,
                    };
                    TypeRegnize(LeftWord);
                    RightWord = new WordInfo()
                    {
                        WType = WordType.空值
                    };
                    IsHaveYes = !string.IsNullOrWhiteSpace(items[0].Groups["yes"].Value);

                    IsHaveNo = !string.IsNullOrWhiteSpace(items[0].Groups["not"].Value);
                    IsSuccess = true;
                }
            }

        }

        private void TypeRegnize(WordInfo word)
        {
            string[] keys = Config["TwoWordLine_NullWord"].Trim(',').Split(',');
            word.WType = keys.ToList().Contains(word.Content) ? WordType.空值 : WordType.普通;
        }

        private string GetParrern() => $"({GetHeaders()})(?<name1>.*?)(和|与)(?<name2>.*?)(?<yes>(能|可以){{0,1}})(?<not>不{{0,1}})(能|可以)({GetEnds()})";

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

        private string GetAnd()
        {
            if (Config == null || Config["TwoWordLine_And"] == null)
            {
                return "和|与";
            }
            return Config["TwoWordLine_And"].Trim(',').Replace(",", "|");
        }
    }
}
