using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MaxNumber
{
    public class StringGetter
    {
        public string FileAddress { get; private set; }

        public List<string> GetStringsFromFile()
        {
            List<string> output;
            Console.WriteLine("Type file address:");
            FileAddress = Console.ReadLine().Replace("\"", "");
            if (File.Exists(FileAddress))
            {
                output = File.ReadAllLines(FileAddress).ToList();
                return output;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
