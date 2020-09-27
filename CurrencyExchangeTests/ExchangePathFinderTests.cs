using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CurrencyExchange.Tests
{
    [TestClass()]
    public class ExchangePathFinderTests
    {
        [TestMethod()]
        public void GetExchangePathTest()
        {
            string searchCurrency = "USD UAH";
            List<string> inputs = new List<string>() {
                "USD SEK",
                "USD RUB",
                "RUB EUR",
                "RUB POL",
                "SEK UAH",
                "POL UAH",
                "EUR ARG",
                "POL GBD",
                "GBD EUR",
                "EUR GBD",
                "GBD CAD",
                "CAD CED",
                "CED UAH",
                "CAD GAR",
                "SEK ANA",
                "ANA GAR",
                "CED SEK",
            };
            ExchangePathFinder pathFinder = new ExchangePathFinder(searchCurrency, inputs);
            Assert.AreEqual(pathFinder.GetExchangePath(), "USD SEK UAH");

            searchCurrency = "EUR ANA";
            pathFinder = new ExchangePathFinder(searchCurrency, inputs);
            Assert.AreEqual(pathFinder.GetExchangePath(), "EUR GBD CAD CED SEK ANA");

            searchCurrency = "EUR UAH";
            pathFinder = new ExchangePathFinder(searchCurrency, inputs);
            Assert.AreEqual(pathFinder.GetExchangePath(), "EUR GBD CAD CED UAH");

            searchCurrency = "CAD EUR";
            pathFinder = new ExchangePathFinder(searchCurrency, inputs);
            Assert.AreEqual(pathFinder.GetExchangePath(), "");
        }
    }
}