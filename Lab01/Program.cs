using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml.Serialization;

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
            var basePath = "C:\\Users\\ausflR\\source\\repos\\InteligentnaAnaliza\\reuters21578";
            var match = new Regex(@"reut.*\.sgm");

            try
            {
                var files = Directory.GetFiles(basePath);

                foreach (var file in Directory.EnumerateFiles(basePath))
                {
                    if (match.IsMatch(Path.GetFileName(file)))
                    {
                        string xmlString = File.ReadAllText(file);
                        xmlString = ReplaceHexSymbols(
                                "<Articles>" +
                                xmlString.Replace("<!DOCTYPE lewis SYSTEM \"lewis.dtd\">", string.Empty) +
                                "</Articles>"
                            );
                        XmlSerializer serializer = new XmlSerializer(typeof(Articles), new XmlRootAttribute("Articles"));

                        StringReader sRead = new StringReader(xmlString);

                        Articles articles = (Articles)serializer.Deserialize(sRead);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error reading files: " + ex.Message);
            }
        }
    }
}
