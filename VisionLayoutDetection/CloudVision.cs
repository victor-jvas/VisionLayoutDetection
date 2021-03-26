using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using VisionLayoutDetection;

namespace CloudVision
{
    public class Vision
    {

        public static List<string> PrintStrings(string imgPath, string strPath)
        {

            List<string> results = new List<string> { };

            List<string> expectedStrings = ReadStringsFile(strPath);

            var response = ProcessImage(imgPath);

            List<string> strList = DetectLines(response);
            

            foreach (var str in expectedStrings)
            {
                results.Add(MatchString(strList, str));
            }
            return results;

        }

        private static List<string> DetectLines(TextAnnotation response)
        {

            List<string> lines = new List<string>();

            foreach (Page pages in response.Pages)
            {
                foreach (Block blocks in pages.Blocks)
                {
                    foreach (Paragraph para in blocks.Paragraphs)
                    {
                        string sentence = "";
                        foreach (Word words in para.Words)
                        {
                            foreach (Symbol sym in words.Symbols)
                            {
                                sentence += sym.Text;
                            }
                            sentence += " ";
                        }
                        lines.Add(sentence);
                    }
                }
            }

            return lines;
        }

        public static string MatchString(List<string> text, string expected)
        {
            var match = text.Any(stringToCheck => string.Equals(stringToCheck.Trim(), expected));

            if (match) return null;

            var aux = "Erro na String: \"" + expected + "\"";
            return aux;

        }

        public static TextAnnotation ProcessImage(string path)
        {
            var client = ImageAnnotatorClient.Create();

            var image = Image.FromFile(path);

            return client.DetectDocumentText(image);

        }

        public static List<string> ReadStringsFile(string path)
        {
            var counter = 0;
            string line;
            List<string> results = new List<string> { };

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                results.Add(line);
                counter++;
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            //System.Console.ReadLine();
            return results;
        }

        public static List<string> ShowFoundStrings(string imgPath)
        {
            var response = ProcessImage(imgPath);

            return  DetectLines(response);
        }
    }
}
