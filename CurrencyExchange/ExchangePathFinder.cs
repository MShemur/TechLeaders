using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurrencyExchange
{
    /// <summary>
    /// Finds the shortest way for currency exchange
    /// </summary>
    public class ExchangePathFinder
    {
        private string searchCurrencies;
        private List<string> inputStrings;

        /// <summary>
        /// Pairs of currencies form a graph
        /// </summary>
        private Dictionary<string, List<string>> graph;
        private List<string> checkedVertexes;
        private Dictionary<string, string> waysForVertexes;
        private Queue<string> searchQue;

        /// <summary>
        /// Finds the shortest way for currency exchange
        /// </summary>
        /// <param name="searchCurrencies">Pair of currencies, for which we search exchange way (format "FIRST SECOND")</param>
        /// <param name="inputStrings">Strings with pairs of possible exchange</param>
        public ExchangePathFinder(string searchCurrencies, List<string> inputStrings)
        {
            this.searchCurrencies = searchCurrencies;
            this.inputStrings = inputStrings;

            InitInputData();
        }

        private void InitInputData()
        {
            searchQue = new Queue<string>();
            checkedVertexes = new List<string>();
            waysForVertexes = new Dictionary<string, string>();

            FillGraphFromInput(ref graph);
        }

        /// <summary>
        /// Returns a path of the shortest exchanging way
        /// </summary>
        /// <returns></returns>
        public string GetExchangePath()
        {
            GetSearchCurrencies(searchCurrencies, out var changeFrom, out var changeTo);

            searchQue.Enqueue(changeFrom);
            checkedVertexes.Add(changeFrom);

            ExploreVertex(searchQue.Dequeue(), changeTo);

            var path = GetPathList(changeFrom, changeTo);
            path.Reverse();

            StringBuilder way = new StringBuilder();

            foreach (var vertex in path)
            {
                way.Append(vertex);
                way.Append(" ");
            }

            return way.ToString().Trim();
        }

        /// <summary>
        /// Returns a pair from input string of currencies from which to start and what to finish
        /// </summary>
        private void GetSearchCurrencies(string inputString, out string changeFrom, out string changeTo)
        {
            var currencies = inputString.Split(" ");

            changeFrom = currencies[0];
            changeTo = currencies[1];
        }

        /// <summary>
        /// Fill graph of currencies from input pairs
        /// </summary>
        /// <param name="graph">Graph to fill</param>
        private void FillGraphFromInput(ref Dictionary<string, List<string>> graph)
        {
            graph = new Dictionary<string, List<string>>();

            foreach (var inputString in inputStrings)
            {
                var splits = inputString.Split(" ");
                var key = splits[0];
                var value = splits[1];

                if (!graph.ContainsKey(key))
                {
                    graph.Add(key, new List<string>() { value });
                }
                else
                {
                    graph[key].Add(value);
                }
            }
        }

        /// <summary>
        /// Searching for currency we want to change to in a graph recursively.Filling dictionary of shortest way to this currency
        /// </summary>
        /// <param name="currentGraphVertex"></param>
        /// <param name="searchVertex">currency we are looking for</param>
        private void ExploreVertex(string currentGraphVertex, string searchVertex)
        {
            if (currentGraphVertex == searchVertex) return;

            if (graph.ContainsKey(currentGraphVertex))
            {
                foreach (var vertex in graph[currentGraphVertex])
                {
                    if (checkedVertexes.Contains(vertex)) continue;

                    checkedVertexes.Add(vertex);
                    searchQue.Enqueue(vertex);
                    waysForVertexes.Add(vertex, currentGraphVertex);
                }
            }

            while (searchQue.Count > 0)
            {
                ExploreVertex(searchQue.Dequeue(), searchVertex);
            }
        }

        private List<string> GetPathList(string entry, string search)
        {
            var way = new List<string>();
            if (waysForVertexes.TryGetValue(search, out string value))
            {
                if (value != entry)
                    way.AddRange(GetPathList(entry, value));
                else
                {
                    way.Add(entry);
                }
            }

            if (way.Any()) way.Insert(0, search);

            return way;
        }
    }
}
