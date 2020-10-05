using System.Collections.Generic;

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
            var numbers = new SortedSet<ValueIndex>(new ValueIndexComparer());
            string numbersOutput = "";

            //также бинарная (двоичная) куча будет хорошо

            for (int i = 0; i < inputNumbers.Length; i++)
            {
                var number = new ValueIndex(inputNumbers[i], i);

                if (numbers.Count < maxNumberCount)
                {
                    numbers.Add(number);
                }
                else
                {
                    if (numbers.Max.Value >= inputNumbers[i]) continue;

                    numbers.Remove(numbers.Max);
                    numbers.Add(number);
                }
            }

            foreach (var value in numbers)
            {
                numbersOutput += value.Value + " ";
            }

            return numbersOutput.Trim();
        }
    }

    public class ValueIndexComparer : IComparer<ValueIndex>
    {
        public int Compare(ValueIndex x, ValueIndex y)
        {
            if (x.Value != y.Value) return -x.Value.CompareTo(y.Value);
            return x.Index.CompareTo(y.Index);
        }
    }

    public struct ValueIndex
    {
        public int Value;
        public int Index;

        public ValueIndex(int value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}