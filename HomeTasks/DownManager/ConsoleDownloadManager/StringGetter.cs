using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleDownloadManager
{
    /// <summary>
    /// Get list of strings from specified file
    /// </summary>
    public class StringGetter
    {
        public string FileAddress { get; private set; }

        public List<string> GetStringsFromFile(params string[] address)
        {
            Console.WriteLine("Type file address:");
            if (address.Length > 0)
                FileAddress = address[0];
            else
                FileAddress = Console.ReadLine().Replace("\"", "");

            if (!File.Exists(FileAddress)) throw new FileNotFoundException();

            var output = File.ReadAllLines(FileAddress).ToList();

            return output;
        }
    }
}