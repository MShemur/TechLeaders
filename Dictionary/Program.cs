using System;
using System.Collections.Generic;

namespace DictionarySpace
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var dict = new Dictionary<string, string>();

            for (int i = 1; i < 1000; i++)
            {
                dict.Add(i.ToString(), (i + i).ToString());
            }

            dict.TryGetValue("78", out var val4uee);
            for (int i = 1; i < 1000; i++)
            {
                dict.TryGetValue(i.ToString(), out var value);
            }

            dict.Clear();

            for (int i = 1; i < 1000; i = i + 1)
            {
                dict.Add(i.ToString(), (i + i).ToString());
            }

            for (int i = 1; i < 1000; i++)
            {
                dict.TryGetValue(i.ToString(), out var value);
                Console.WriteLine(value);
            }

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}