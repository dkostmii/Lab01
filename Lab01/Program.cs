using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

using static Lab01.Util;

namespace Lab01
{
    class Program
    {
        static Dictionary<string, Articles> ReadArticleFiles(string basePath)
        {
            var match = new Regex(@"reut.*\.sgm");

            var result = new Dictionary<string, Articles>();

            var filesCount = Directory.GetFiles(basePath).Length;
            var counter = 0;

            Console.WriteLine("Reading files...");

            foreach (var file in Directory.EnumerateFiles(basePath))
            {
                counter++;
                Console.WriteLine(String.Format("{0:0.0}", (double) counter / filesCount * 100) + "%");

                if (match.IsMatch(Path.GetFileName(file)))
                {
                    // Read XML
                    string xmlString = File.ReadAllText(file);
                    xmlString = ReplaceHexSymbols(
                            "<Articles>" +
                            xmlString.Replace("<!DOCTYPE lewis SYSTEM \"lewis.dtd\">", string.Empty) +
                            "</Articles>"
                        );
                    XmlSerializer serializer = new XmlSerializer(typeof(Articles), new XmlRootAttribute("Articles"));

                    StringReader sRead = new StringReader(xmlString);

                    Articles articles = (Articles)serializer.Deserialize(sRead);

                    result.Add(Path.GetFileName(file), articles);
                }
            }

            return result;
        }

        static void Main(string[] args)
        {
            // directory with text files
            var basePath = "C:\\Users\\ausflR\\source\\repos\\InteligentnaAnaliza\\reuters21578";

            var countryTags = new string[] { "west-germany", "usa", "france", "uk", "canada", "japan" };
            var checkAnalyzedCountry = new Func<string, bool>(country => CheckContains(country, countryTags));

            var summaryFreqs = new Frequencies(countryTags);

            try
            {
                var data = ReadArticleFiles(basePath);

                // Prepare articles with specific tag frequency table

                foreach (var keyValuePair in data)
                {
                    var file = keyValuePair.Key;
                    var articles = keyValuePair.Value;

                    var freqs = new Frequencies(countryTags);

                    // Display articles count
                    Console.WriteLine("[" + file + "]: Znaleziono " + articles.REUTERS.Length + "artykułów.");

                    foreach (var article in articles.REUTERS)
                    {
                        // find the articles with exactly 1 tag in countryTags
                        // with non-empty body
                        if (article.PLACES.Length == 1 &&
                            checkAnalyzedCountry(article.PLACES[0]) &&
                            article.TEXT.BODY != null)
                        {
                            foreach (var place in article.PLACES)
                            {
                                freqs.Increment(place);
                            }
                        }
                    }

                    // Display tag count for each file
                    foreach (var tagCountPair in freqs.Data)
                    {
                        Console.WriteLine("     " + tagCountPair.Key + ": " + tagCountPair.Value);
                    }

                    summaryFreqs.ReduceWith(freqs);
                }

                // Display summary
                Console.WriteLine("-------------------------------");
                Console.WriteLine("\n");
                Console.WriteLine("Podsumowanie");
                foreach (var tagCountPair in summaryFreqs.Data)
                {
                    Console.WriteLine("     " + tagCountPair.Key + ": " + tagCountPair.Value);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error reading files: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown error occurred: " + ex.Message);
            }
        }
    }
}
