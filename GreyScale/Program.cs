using System;
using System.Drawing;
using System.IO;
using ImageMagick;

namespace GreyScale
{

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputAddress;

                if (args.Length > 0)
                {
                    inputAddress = args[0];
                }
                else
                {
                    Console.WriteLine("Type file address:");
                    inputAddress = Console.ReadLine()?.Replace("\"", "");
                }

                if (File.Exists(inputAddress))
                {
                    string outputAddress = inputAddress.Insert(inputAddress.LastIndexOf(".", StringComparison.Ordinal), "-result");

                    using (var image = new MagickImage(inputAddress))
                    {
                        image.ColorSpace = ColorSpace.Gray;
                        image.Write(outputAddress);
                    }
                    Console.WriteLine("Successfully saved");
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}
