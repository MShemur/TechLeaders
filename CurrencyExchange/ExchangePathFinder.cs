using System.Collections.Generic;
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

        private HashSet<string> checkedNodes;
        
        private Dictionary<string, string> waysForNodes;
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
            checkedNodes = new HashSet<string>();
            waysForNodes = new Dictionary<string, string>();

            FillGraphFromInput(ref graph);
        }

        /// <summary>
        /// Returns a path of the shortest exchanging way
        /// </summary>
        /// <returns></returns>
        public string GetExchangePath()
        {
            GetSearchCurrencies(searchCurrencies, out var changeFrom, out var changeTo);
            FindWaysForNodes(changeFrom, changeTo);
            var path = GetPathList(changeFrom, changeTo);

            var way = new StringBuilder();

            foreach (var node in path)
            {
                way.Append(node);
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
        /// <param name="currentNode"></param>
        /// <param name="searchNode">currency we are looking for</param>
        private void FindWaysForNodes(string currentNode, string searchNode)
        {
            searchQue.Enqueue(currentNode);
            checkedNodes.Add(currentNode);

            while (searchQue.Count > 0)
            {
                if (checkedNodes.Contains(searchNode)) break;
                var parent = searchQue.Dequeue();
                if (!graph.ContainsKey(parent)) continue;
                
                foreach (var child in graph[parent])
                {
                    if (checkedNodes.Contains(child)) continue;

                    checkedNodes.Add(child);
                    searchQue.Enqueue(child);
                    waysForNodes.Add(child, parent);
                }
            }
        }

        private List<string> GetPathList(string entry, string search)
        {
            var way = new List<string>();
            if (waysForNodes.Count == 0) return way;
            way.Add(search);
            var key = search;
            while (waysForNodes.ContainsKey(key))
            {
                way.Insert(0, waysForNodes[key]);
                key = waysForNodes[key];
            }

            return way;
        }
    }
}