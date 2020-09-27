using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CurrencyExchange
{
    /// <summary>
    /// Get list of strings from specified file
    /// </summary>
    public class StringGetter
    {
        public string FileAddress { get; private set; }

        public List<string> GetStringsFromFile()
        {
            Console.WriteLine("Type file address:");

            FileAddress = Console.ReadLine().Replace("\"", "");
            if (!File.Exists(FileAddress)) throw new FileNotFoundException();

            var output = File.ReadAllLines(FileAddress).ToList();

            return output;
        }
    }
}
