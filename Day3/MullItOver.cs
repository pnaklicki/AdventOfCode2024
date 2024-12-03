using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day3
{
    public class MullItOver : PuzzleBase
    {
        public override int DayNumber => 3;

        private const string _instructionsRegex = "do\\(\\)|don\\'t\\(\\)|(?<=mul\\()\\d{1,3},\\d{1,3}(?=\\))";

        public override (string, string) Solve()
        {
            var instructionsList = GetInputData();
            var firstSolution = SolveFirstPuzzle(instructionsList);
            var secondSolution = SolveSecondPuzzle(instructionsList);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(IEnumerable<string> instructionsList)
        {
            var firstSolution = 0;
            foreach (var mulPair in instructionsList.Where(i => !i.Contains("do") && !i.Contains("don't"))) 
            {
                var mulNumbers = mulPair.Split(",");
                firstSolution += int.Parse(mulNumbers[0]) * int.Parse(mulNumbers[1]);
            }

            return firstSolution.ToString();
        }

        private string SolveSecondPuzzle(IEnumerable<string> instructionsList)
        {
            var secondSolution = 0;
            var isDoFlagActive = true;
            foreach (var instructions in instructionsList)
            {
                if (instructions.Contains("don't"))
                {
                    isDoFlagActive = false;
                }
                else if (instructions.Contains("do"))
                {
                    isDoFlagActive = true;
                } 
                else if(isDoFlagActive)
                {
                    var mulNumbers = instructions.Split(",");
                    secondSolution += int.Parse(mulNumbers[0]) * int.Parse(mulNumbers[1]);
                }
            }

            return secondSolution.ToString();
        }

        private IEnumerable<string> GetInputData()
        {
            var input = LoadInputData();
            var mulRegex = new Regex(_instructionsRegex);

            return mulRegex.Matches(input).Select(m => m.Value);
        }
    }
}
