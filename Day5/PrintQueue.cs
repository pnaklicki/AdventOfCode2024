using System.Collections.Generic;

namespace AdventOfCode2024.Day5
{
    public class PrintQueue : PuzzleBase
    {
        public override int DayNumber => 5;

        public override (string, string) Solve()
        {
            var inputData = GetInputData();
            var orderingRules = inputData.Item1;
            var updates = inputData.Item2;
            var firstSolution = SolveFirstPuzzle(orderingRules, updates);
            var secondSolution = SolveSecondPuzzle(orderingRules, updates);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(Dictionary<int, List<int>> orderingRules, List<List<int>> updates)
        {
            var firstPuzzleSolution = 0;
            foreach (var update in updates)
            {
                if (IsUpdateValid(update, orderingRules))
                {
                    firstPuzzleSolution += update.ElementAt((update.Count - 1) / 2);
                }
            }

            return firstPuzzleSolution.ToString();
        }

        private bool IsUpdateValid(List<int> update, Dictionary<int, List<int>> orderingRules)
        {
            foreach (var rule in orderingRules)
            {
                if (update.Contains(rule.Key))
                {
                    foreach (var secondRulePage in rule.Value)
                    {
                        if (update.Contains(secondRulePage))
                        {
                            if (update.IndexOf(rule.Key) > update.IndexOf(secondRulePage))
                            {
                                return false;
                            }
                        }
                    }

                }
            }

            return true;
        }

        private string SolveSecondPuzzle(Dictionary<int, List<int>> orderingRules, List<List<int>> updates)
        {
            var secondPuzzleSolution = 0;
            foreach (var update in updates)
            {
                if (!IsUpdateValid(update, orderingRules))
                {
                    var rank = new Dictionary<int, int>();
                    var currentOrderingRules = orderingRules.Where(o => update.Contains(o.Key)).ToDictionary();
                    foreach (var updatePage in update)
                    {
                        AssignRank(updatePage, currentOrderingRules, rank);
                    }

                    var validUpdate = update.OrderBy(item => rank[item]).ToList();
                    secondPuzzleSolution += validUpdate.ElementAt((validUpdate.Count - 1) / 2);
                }
            }

            return secondPuzzleSolution.ToString();
        }

        private int AssignRank(int page, Dictionary<int, List<int>> orderingRules, Dictionary<int, int> rank)
        {
            if (rank.ContainsKey(page))
                return rank[page];

            if (!orderingRules.ContainsKey(page))
            {
                rank[page] = 0;
                return rank[page];
            }

            rank[page] = orderingRules[page].Select(secondPageRule => AssignRank(secondPageRule, orderingRules, rank)).DefaultIfEmpty(0).Max() + 1;
            return rank[page];
        }

        private (Dictionary<int, List<int>>, List<List<int>>) GetInputData()
        {
            var input = LoadInputData();

            var orderingRules = new Dictionary<int, List<int>>();
            var updates = new List<List<int>>();

            foreach (var inputRow in input.Split(Environment.NewLine))
            {
                if (inputRow.Contains("|"))
                {
                    var rulePages = inputRow.Split("|");
                    var firstRulePage = int.Parse(rulePages[0]);
                    var secondRulePage = int.Parse(rulePages[1]);

                    if (orderingRules.ContainsKey(firstRulePage))
                    {
                        orderingRules[firstRulePage].Add(secondRulePage);
                    }
                    else
                    {
                        orderingRules.Add(firstRulePage, [secondRulePage]);
                    }
                }
                else if (inputRow.Contains(","))
                {
                    var updatePages = inputRow.Split(",");
                    var update = new List<int>();
                    foreach (var updatePage in updatePages)
                    {
                        update.Add(int.Parse(updatePage));
                    }
                    updates.Add(update);
                }
            }

            return (orderingRules, updates);
        }
    }
}
