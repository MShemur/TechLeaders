using System;
using System.Drawing;
using System.IO;

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

                    Bitmap inputImage = new Bitmap(inputAddress);

                    SaveToGrayScale(inputImage, outputAddress);

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

        /// <summary>
        /// Saves input image to the specified address
        /// </summary>
        /// <param name="Bmp">input image</param>
        /// <param name="outputAddress">seve to this address</param>
        public static void SaveToGrayScale(Bitmap Bmp, string outputAddress)
        {
            var input = new Bitmap(Bmp);

            for (int y = 0; y < input.Height; y++)
                for (int x = 0; x < input.Width; x++)
                {
                    var color = input.GetPixel(x, y);
                    var rgb = (int)Math.Round(.33 * color.R + .33 * color.G + .33 * color.B);
                    input.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }

            input.Save(outputAddress);
        }
    }

}
