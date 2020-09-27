using System;
using System.IO;

namespace CurrencyExchange
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StringGetter stringGetter = new StringGetter();

                var strings = stringGetter.GetStringsFromFile();

                var fileAddress = stringGetter.FileAddress;
                var folderName = fileAddress.Substring(0, fileAddress.LastIndexOf(@"\", StringComparison.Ordinal) + 1);

                if (int.TryParse(strings[1], out int counter))
                {
                    var pathFinder = new ExchangePathFinder(strings[0], strings.GetRange(2, counter));
                    var exchangePath = pathFinder.GetExchangePath();

                    File.WriteAllText(folderName + "output.txt", exchangePath);

                    Console.WriteLine("Successfully written");
                }
                else
                {
                    throw new Exception("Invalid input data");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }

            Console.ReadKey();
        }
    }
}