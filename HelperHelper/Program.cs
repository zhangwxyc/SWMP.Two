using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelperHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            string txt = File.ReadAllText("D:\\a.js");
            var lines = txt.Split('\n');
            string par= "keys: \\[(?<valueNames>.*?)\\].*initTableData = \\{ title: \\[(?<tab>.*?)].*reportTable = \\{ data: \\{ thData: \\[\\[(?<colNames>.*?)\\]";

            StringBuilder content=new StringBuilder();
            foreach (var item in lines)
            {
                var r= Regex.Match(item, par,RegexOptions.IgnoreCase);
                if (!r.Success)
                {
                    continue;
                }
                var cc = r.Groups["valueNames"].Value;
                var cc1 = r.Groups["tab"].Value;
                var cc2 = r.Groups["colNames"].Value;

                content.AppendFormat("{0}\r\n", cc1.Replace("\"", "").Split(',')[0]);

                var ccs = cc.Replace("\"", "").Split(',');
                var cc2s = cc2.Replace("\"", "").Split(',');
                for (int index = 0; index < ccs.Length; index++)
                {
                    content.AppendFormat("{1}\t{0}\r\n", ccs[index],cc2s[index].Replace("{ value: ", "").Replace("}",""));
                }
                content.Append("\r\n\r\n");
            }

            File.WriteAllText("D:\\b.txt",content.ToString());
        }
    }
}
