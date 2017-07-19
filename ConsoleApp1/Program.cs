using System;
using System.Text.RegularExpressions;
using FLib;
using Microsoft.Extensions.Configuration;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var evn = AppContext.BaseDirectory;
            var builder = new ConfigurationBuilder()
                 .SetBasePath(evn)
                .AddJsonFile("123.json", optional: true, reloadOnChange: true);
               // .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)//增加环境配置文件，新建项目默认有
           var Configuration = builder.Build();
            
           // FLib.TwoWordLineInfo info=new TwoWordLineInfo("s和a",Configuration.GetSection("TwoWord"));
           string regex = "(?<name1>.*?)(和|与)(?<name2>.*?)";
           var  items = Regex.Matches("s和a", regex, RegexOptions.IgnoreCase);
        }
    }
}