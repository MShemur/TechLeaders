using System;

namespace MaxNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = new[] { 1, 5, 65, 7, 5, 9, 12, 18, 84, 11, 55, 685, 17, 25, 93, 412, 618, 84 };
            var count = 4;
            var counter = 0;

            for (int j = 0; j < count; j++)
            {
                var maxIndex = j;
                var max = array[maxIndex];
                for (int i = j; i < array.Length; i++)
                {
                    if (array[i] > max)
                    {
                        max = array[i];
                        maxIndex = i;
                    }
                    counter++;
                }

                var temp = array[j];
                array[j] = max;
                array[maxIndex] = temp;
            }

            for (int i = 0; i < count; i++)
            {
                Console.Write(array[i] + " ");
            }

            Console.WriteLine();
            Console.WriteLine(counter);
            Console.ReadKey();
        }
    }
}
