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
            //var path = "D:\\Dev\\VisionTest\\IKGAME-897.png";
            // Instantiates a client
            //fvar client = ImageAnnotatorClient.Create();
            // Load the image file into memory
            //var image = Image.FromFile("D:\\Dev\\VisionTest\\IKGAME-897.png");
            // Performs text detection on the image file
            //var response = client.DetectDocumentText(image);
            
            List<string> results = new List<string> { };

            List<string> expectedStrings = ReadStringsFile(strPath);

            var response = ProcessImage(imgPath);

            List<string> strList = DetectLines(response);
           
            
            //var expected = "Configurações";
            //Console.WriteLine(MatchString(paraList, expected));
            //DisplayResult(MatchString(paraList, expected));

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

            if (!match)
            {
                var aux = "Erro na String: \"" + expected + "\"";
                //DisplayResult(aux);
                //Console.WriteLine("Erro na String: \"" + expected + "\"");
                return aux;
            }
                
            return "No Error";
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
