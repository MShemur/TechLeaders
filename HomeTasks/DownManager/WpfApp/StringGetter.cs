using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WpfApp
{
    /// <summary>
    /// Get list of strings from specified file
    /// </summary>
    public class StringGetter
    {
        public string FileAddress { get; private set; }

        public List<string> GetStringsFromFile(string address)
        {
            FileAddress = address;

            if (!File.Exists(FileAddress)) throw new FileNotFoundException();

            var output = File.ReadAllLines(FileAddress).ToList();

            return output;
        }
    }
}