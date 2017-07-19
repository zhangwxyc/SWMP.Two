using System;
using System.Text.RegularExpressions;

namespace FLib
{
    public class Class1
    {
       public string aa = "b馒头和米饭不能c吗？";

        public WordInfo[] Test(string line)
        {
            string regex = "(s|d)(?<name1>.*?)和(?<name2>.*?)(?<yes>能{0,1})(?<not>不{0,1})能(a|b)";
            var items = Regex.Matches(line, regex, RegexOptions.IgnoreCase);
            if (items.Count>0)
            {
                var leftWord = new WordInfo()
                {
                    Content = items[0].Groups["name1"].Value,
                };
                var rightWord = new WordInfo()
                {
                    Content = items[0].Groups["name2"].Value,
                };

            }
            return null;
        }

    }
}
