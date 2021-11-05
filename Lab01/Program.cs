using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace Lab01
{
    class Program
    {
        static string ReplaceHexSymbols(string text)
        {
            return Regex.Replace(text, "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]", "", RegexOptions.Compiled);
        }

        static void Main(string[] args)
        {
            // directory with text files
            var basePath = "C:\\reuters21578";
            var match = new Regex(@"reut.*\.sgm");

            var countryTags = new String[] { "west-germany", "usa", "france", "uk", "canada", "japan" };

            var summaryFreqs = new Dictionary<String, int>();
            foreach (var tag in countryTags)
            {
                summaryFreqs.Add(tag, 0);
            }

            try
            {
                var files = Directory.GetFiles(basePath);

                foreach (var file in Directory.EnumerateFiles(basePath))
                {
                    if (match.IsMatch(Path.GetFileName(file)))
                    {
                        // Prepare articles with specific tag frequency table
                        var freqs = new Dictionary<String, int>();
                        foreach (var tag in countryTags)
                        {
                            freqs.Add(tag, 0);
                        }

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

                        // Display articles count
                        Console.WriteLine("[" + Path.GetFileName(file) +  "]: Znaleziono " + articles.REUTERS.Length + "artykułów.");

                        // Count articles with specific place tag
                        foreach (var article in articles.REUTERS)
                        {
                            // Do not count the articles with tags other than *countryTags*
                            if (article.PLACES.Where(p => !countryTags.Contains(p)).Count() == 0)
                            {
                                foreach (var place in article.PLACES)
                                {
                                    if (countryTags.Contains(place))
                                    {
                                        freqs[place] += 1;
                                    }
                                }
                            }
                        }

                        // Display tag count for each file
                        foreach (var tagCountPair in freqs)
                        {
                            summaryFreqs[tagCountPair.Key] += tagCountPair.Value;

                            Console.WriteLine("     " + tagCountPair.Key + ": " + tagCountPair.Value);
                        }
                    }
                }

                // Display summary
                Console.WriteLine("-------------------------------");
                Console.WriteLine("\n");
                Console.WriteLine("Podsumowanie");
                foreach (var tagCountPair in summaryFreqs)
                {
                    Console.WriteLine("     " + tagCountPair.Key + ": " + tagCountPair.Value);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error reading files: " + ex.Message);
            }
        }
    }
}
