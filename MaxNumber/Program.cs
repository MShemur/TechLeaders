using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace MaxNumber
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

                GetInputData(strings, out var maxNumberCount, out var inputNumbers);

                MaxNumberFinder maxNumberFinder = new MaxNumberFinder(inputNumbers, maxNumberCount);

                File.WriteAllText(folderName + "output.txt", maxNumberFinder.GetMaxNumbers());

                Console.WriteLine("Successfully written");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }

            Console.ReadKey();
        }

        private static void GetInputData(List<string> strings, out int maxNumberCount, out int[] inputNumbers)
        {
            bool correctNumberCount = int.TryParse(strings[0], out var numbersCount);
            bool correctMaxNumberCount = int.TryParse(strings[2], out maxNumberCount);

            var arrayN = strings[1].Split().ToList();

            if (correctNumberCount && correctMaxNumberCount)
            {
                inputNumbers = new int[numbersCount];
                for (int i = 0; i < numbersCount; i++)
                {
                    if (int.TryParse(arrayN[i], out var number))
                    {
                        inputNumbers[i] = number;
                    }
                    else
                    {
                        throw new Exception("Wrong input data");
                    }
                }
            }
            else
            {
                throw new Exception("Wrong input data");
            }
        }
    }
}
