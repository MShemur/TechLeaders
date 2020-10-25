using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NUnit.Framework;

namespace TestProject1
{
    public class Tests
    {
        [Test]
        public void TryGetIntInt()
        {
            DictionarySpace.Dictionary<int, int> dict = new DictionarySpace.Dictionary<int, int>
            {
                {1, 11},
                {2, 22},
                {3, 33},
                {4, 44},
                {5, 55},
                {6, 66},
                {7, 77},
                {8, 88},
                {9, 99}
            };

            int[] array = new int[9];
            for (int i = 1; i < 10; i++)
            {
                dict.TryGetValue(i, out var value);
                array[i - 1] = value;
            }

            Assert.That(new[] { 11, 22, 33, 44, 55, 66, 77, 88, 99 }, Is.EquivalentTo(array));
        }

        [Test]
        public void TryGetIntString()
        {
            var dict = new DictionarySpace.Dictionary<int, string>
            {
                {1, "11"},
                {2, "22"},
                {3, "33"},
                {4, "44"},
                {5, "55"},
                {6, "66"},
                {7, "77"},
                {8, "88"},
                {9, "99"}
            };


            string[] array = new string[9];
            for (int i = 1; i < 10; i++)
            {
                dict.TryGetValue(i, out var value);
                array[i - 1] = value;
            }

            Assert.That(new string[] { "11", "22", "33", "44", "55", "66", "77", "88", "99" }, Is.EquivalentTo(array));
        }

        [Test]
        public void TryGetStringString()
        {
            for (int j = 0; j < 500; j++)
            {
                var dict = new DictionarySpace.Dictionary<string, string>
                {
                    {"1", "11"},
                    {"2", "22"},
                    {"3", "33"},
                    {"4", "44"},
                    {"5", "55"},
                    {"6", "66"},
                    {"7", "77"},
                    {"8", "88"},
                    {"9", "99"},
                    {"10", "100"},
                    {"11", "110"},
                    {"12", "120"},
                    {"13", "130"},
                    {"14", "140"},
                    {"15", "150"},
                    {"16", "160"},
                    {"17", "170"},
                    {"18", "180"}
                };
                string[] sarray = new string[18];
                for (int i = 1; i < 19; i++)
                {
                    dict.TryGetValue(i.ToString(), out var value);
                    sarray[i - 1] = value;
                }

                Assert.That(sarray
                    , Is.EquivalentTo(new string[]
                    {
                        "11", "22", "33", "44", "55", "66", "77", "88", "99", "100", "110", "120", "130", "140", "150",
                        "160", "170", "180"
                    }));


                dict.Remove("6");
                dict.Remove("7");
                dict.Remove("13");
                dict.Remove("18");

                sarray = new string[18];
                for (int i = 1; i < 19; i++)
                {
                    dict.TryGetValue(i.ToString(), out var value);
                    sarray[i - 1] = value;
                }

                Assert.That(sarray
                    , Is.EquivalentTo(new string[]
                    {
                        "11", "22", "33", "44", "55", null, null, "88", "99", "100", "110", "120", null, "140", "150",
                        "160", "170", null
                    }));

                dict.Add("6", "66");
                dict.Add("7", "77");
                dict.Add("18", "180");
                dict.Add("13", "130");

                sarray = new string[18];
                for (int i = 1; i < 19; i++)
                {
                    dict.TryGetValue(i.ToString(), out var value);
                    sarray[i - 1] = value;
                }

                Assert.That(sarray
                    , Is.EquivalentTo(new string[]
                    {
                        "11", "22", "33", "44", "55", "66", "77", "88", "99", "100", "110", "120", "130", "140", "150",
                        "160", "170", "180"
                    }));
            }
        }

        [Test]
        public void TestStringWithRemoval()
        {
            var dict = new DictionarySpace.Dictionary<string, string>();
            List<string> inputListAll = new List<string>(1000);
            for (int i = 1; i < 1001; i++)
            {
                dict.Add(i.ToString(), (i + i).ToString());
                inputListAll.Add((i + i).ToString());
            }

            var outArrayAll = new string[1000];

            for (int i = 1; i < 1001; i++)
            {
                dict.TryGetValue(i.ToString(), out var value);
                outArrayAll[i - 1] = value;
            }

            Assert.That(outArrayAll
                , Is.EquivalentTo(inputListAll));

            for (int i = 1; i < 1000; i = i + 2)
            {
                dict.Remove(i.ToString());
                inputListAll.Remove((i + i).ToString());
            }

            var outArrayAllL = new List<string>(1000);
            for (int i = 1; i < 1001; i++)
            {
                dict.TryGetValue(i.ToString(), out var value);
                outArrayAllL.Add(value);
            }

            inputListAll = inputListAll.Where(x => x != null).ToList();
            outArrayAllL = outArrayAllL.Where(x => x != null).ToList();
            Assert.That(inputListAll
                , Is.EquivalentTo(outArrayAllL));

            inputListAll = new List<string>(1000);
            for (int i = 1; i < 1001; i++)
            {
                inputListAll.Add((i + i).ToString());

            }

            for (int i = 1; i < 1000; i = i + 2)
            {
                dict.Add(i.ToString(), (i + i).ToString());
            }

            outArrayAll = new string[1000];

            for (int i = 1; i < 1001; i++)
            {
                dict.TryGetValue(i.ToString(), out var value);
                outArrayAll[i - 1] = value;
            }

            Assert.That(outArrayAll
                , Is.EquivalentTo(inputListAll));

            // for (int i = 1; i < 1000; i++)
            // {
            //     dict.TryGetValue(i.ToString(), out var value);
            // }
        }

        [Test]
        public void TryGetStringStringSmall()
        {
            var dict = new DictionarySpace.Dictionary<string, string>
            {
                {"1", "11"},
                {"2", "22"},
                {"3", "33"},
                {"4", "44"},
            };


            var sarray = new string[4];
            for (int i = 1; i < 5; i++)
            {
                dict.TryGetValue(i.ToString(), out var value);
                sarray[i - 1] = value;
            }

            Assert.That(sarray
                , Is.EquivalentTo(new string[]
                {
                    "11", "22", "33", "44"
                }));

            dict.Remove("1");
            dict.Remove("2");
            dict.Remove("3");

            dict.Add("1", "11");
            dict.Add("2", "22");
            dict.Add("3", "33");

            sarray = new string[4];
            for (int i = 1; i < 5; i++)
            {
                dict.TryGetValue(i.ToString(), out var value);
                sarray[i - 1] = value;
            }

            Assert.That(sarray
                , Is.EquivalentTo(new string[]
                {
                    "11", "22", "33", "44"
                }));
        }
    }
}