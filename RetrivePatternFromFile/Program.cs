using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RetrivePatternFromFile
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string input = File.ReadAllText("appsettings.json");
            string pattern = @"__\w+__";
            MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.Singleline);
            List<string> wordsList = new List<string>();
            foreach (Match match in matches)
            {
                wordsList.Add(match.ToString());
            }

            foreach (var wordToReplace in wordsList)
            {
                var wordKey = wordToReplace.Replace("_", string.Empty);
                //TODO: Retrive the real value from somewhere.
                string realValue = "RealValue";
                input = input.Replace(wordToReplace, realValue);
            }


            File.WriteAllText("appsettings.json", input);
            input = File.ReadAllText("appsettings.json");


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
