using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaxNumber;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaxNumber.Tests
{
    [TestClass()]
    public class MaxNumberFinderTests
    {
        [TestMethod()]
        public void GetMaxNumbersTest()
        {
            var testArray = new int[] { 1, 2, 3, 4, 5, 6 };
            var maxCount = 2;
            MaxNumberFinder maxNumberFinder = new MaxNumberFinder(testArray, maxCount);
            Assert.AreEqual(maxNumberFinder.GetMaxNumbers(), "6 5");

            testArray = new int[] { 51, 1, 65, 12, 34, 75, 98, 45, 86, 47, 56, 87, 41, 23, 54, 78 };
            maxCount = 4;
            maxNumberFinder = new MaxNumberFinder(testArray, maxCount);
            Assert.AreEqual(maxNumberFinder.GetMaxNumbers(), "98 87 86 78");
        }
    }
}