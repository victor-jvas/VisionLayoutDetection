using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using VisionLayoutDetection;

namespace CloudVision
{
    public class Vision
    {

        public static List<string> PrintStrings(string path, string[] expectedStrings)
        {
            //var path = "D:\\Dev\\VisionTest\\IKGAME-897.png";
            // Instantiates a client
            //fvar client = ImageAnnotatorClient.Create();
            // Load the image file into memory
            //var image = Image.FromFile("D:\\Dev\\VisionTest\\IKGAME-897.png");
            // Performs text detection on the image file
            //var response = client.DetectDocumentText(image);
            List<string> results = new List<string> { };
            var response = ProcessImage(path);

            List<string> paraList = new List<string> { };


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
                        paraList.Add(sentence);
                    }
                }
            }
            //var expected = "Configurações";
            //Console.WriteLine(MatchString(paraList, expected));
            //DisplayResult(MatchString(paraList, expected));
            foreach (var str in expectedStrings)
            {
                results.Add(MatchString(paraList, str));
            }
            return results;

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
    }
}
