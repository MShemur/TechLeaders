using System;
using System.Collections.Generic;
using System.Text;

namespace MaxNumber
{
    /// <summary>
    /// Class for searching for the maximum number in array
    /// </summary>
    public class MaxNumberFinder
    {
        private readonly int[] inputNumbers;
        private readonly int maxNumberCount;

        /// <summary>
        /// Class for searching for the maximum number in array
        /// </summary>
        /// <param name="inputNumbers">input array</param>
        /// <param name="maxNumberCount">how many maximum numbers you want to get from array</param>
        public MaxNumberFinder(int[] inputNumbers, in int maxNumberCount)
        {
            this.inputNumbers = inputNumbers;
            this.maxNumberCount = maxNumberCount;
        }

        /// <summary>
        /// Returns string of maximum numbers
        /// </summary>
        /// <returns></returns>
        public string GetMaxNumbers()
        {
            string numbers = "";
            var counter = 0;

            for (int j = 0; j < maxNumberCount; j++)
            {
                var maxIndex = j;
                var max = inputNumbers[maxIndex];
                for (int i = j; i < inputNumbers.Length; i++)
                {
                    if (inputNumbers[i] > max)
                    {
                        max = inputNumbers[i];
                        maxIndex = i;
                    }
                    counter++;
                }

                var temp = inputNumbers[j];
                inputNumbers[j] = max;
                inputNumbers[maxIndex] = temp;
            }

            for (int i = 0; i < maxNumberCount; i++)
            {
                numbers += inputNumbers[i] + " ";
            }

            return numbers.Trim();
        }
    }
}
