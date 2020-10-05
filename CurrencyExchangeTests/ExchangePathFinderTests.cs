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
                "SEK USD",
                "USD RUB",
                "RUB EUR",
                "RUB POL",
                "SEK UAH",
                "POL UAH",
                "EUR ARG",
                "ARG EUR",
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
            Assert.AreEqual("USD SEK UAH", pathFinder.GetExchangePath());

            searchCurrency = "EUR ANA";
            pathFinder = new ExchangePathFinder(searchCurrency, inputs);
            Assert.AreEqual("EUR GBD CAD CED SEK ANA", pathFinder.GetExchangePath());

            searchCurrency = "EUR UAH";
            pathFinder = new ExchangePathFinder(searchCurrency, inputs);
            Assert.AreEqual("EUR GBD CAD CED UAH", pathFinder.GetExchangePath());

            searchCurrency = "CAD EUR";
            pathFinder = new ExchangePathFinder(searchCurrency, inputs);
            Assert.AreEqual("CAD CED SEK USD RUB EUR", pathFinder.GetExchangePath());

            searchCurrency = "UAH GAR";
            pathFinder = new ExchangePathFinder(searchCurrency, inputs);
            Assert.AreEqual("", pathFinder.GetExchangePath());
        }
    }
}