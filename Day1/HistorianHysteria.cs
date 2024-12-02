namespace AdventOfCode2024.Day1
{
    public class HistorianHysteria : PuzzleBase
    {
        public override int DayNumber => 1;

        public override (string, string) Solve()
        {
            var inputData = GetInputData();
            var leftList = inputData.Item1;
            var rightList = inputData.Item2;
            var firstSolution = SolveFirstPuzzle(leftList, rightList);
            var secondSolution = SolveSecondPuzzle(leftList, rightList);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(List<int> leftList, List<int> rightList)
        {
            leftList.Sort();
            rightList.Sort();

            var firstPuzzleSolution = 0;

            for (var i = 0; i < leftList.Count; i++)
            {
                firstPuzzleSolution += Math.Abs(leftList[i] - rightList[i]);
            }

            return firstPuzzleSolution.ToString();
        }
        private string SolveSecondPuzzle(List<int> leftList, List<int> rightList)
        {
            var secondPuzzleSolution = 0;

            foreach (var leftListItem in leftList)
            {
                var rightListOccurrence = rightList.Count(i => i == leftListItem);
                secondPuzzleSolution += leftListItem * rightListOccurrence;
            }
            return secondPuzzleSolution.ToString();
        }

        private (List<int>, List<int>) GetInputData()
        {
            var inputString = LoadInputData();

            var firstList = new List<int>();
            var secondList = new List<int>();

            foreach (var inputLine in inputString.Split(Environment.NewLine))
            {
                var inputNumbers = inputLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                firstList.Add(int.Parse(inputNumbers[0]));
                secondList.Add(int.Parse(inputNumbers[1]));
            }

            return (firstList, secondList);
        }
    }
}
