using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RetrivePatternFromFile.Utils;

namespace RetrivePatternFromFile
{
    public class Program
    {
        public static void Main(string[] args)
        {


           var jsonString =  LoadAppsettingValuesFromSomewhere();
           var memoryFileProvider = new InMemoryFileProvider(jsonString);
           var configuration = new ConfigurationBuilder()
               .AddJsonFile(memoryFileProvider, "appsettingsss.json", false, false).Build();

            string keyFromappsettings = configuration["MyKey"];

            CreateHostBuilder(args).Build().Run();
        }


        private static string LoadAppsettingValuesFromSomewhere()
        {
            //The pattern that the values to be replaced are going to follow on the appsettings file
            string pattern = @"__\w+__";

            //Read appsetings file
            string settingFile = File.ReadAllText("appsettings.json");
           
            //Get all the words matching the pattern and replace
            MatchCollection matches = Regex.Matches(settingFile, pattern, RegexOptions.Singleline);
            foreach (Match match in matches)
            {
                var wordToReplace = (match.ToString());

                //Replace all the matches with the real values
                var wordKey = wordToReplace.Replace("_", string.Empty);
                var value = GetValueFromSomewhere(wordKey);
                settingFile = settingFile.Replace(wordToReplace, value);
            }

            return settingFile;
        }

        private static string GetValueFromSomewhere(string key)
        {
            //TODO: Retrive the real value from db, secrets, file, etc.
            return "RealValue";

        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

}
