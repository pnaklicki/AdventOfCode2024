using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day12
{
    public class ClawContraption : PuzzleBase
    {
        public override int DayNumber => 13;

        public override (string, string) Solve()
        {
            var equations = GetInputData();

            var firstSolution = SolveFirstPuzzle(equations);
            var secondSolution = SolveSecondPuzzle(equations);

            return (firstSolution, secondSolution);
        }

        private (decimal A, decimal B) GaussElimination(decimal[,] equation)
        {
            var firstRowDivider = equation[0, 0];
            equation[0, 0] /= firstRowDivider;
            equation[0, 1] /= firstRowDivider;
            equation[0, 2] /= firstRowDivider;
            var secondRowMultiplier = equation[1, 0];
            equation[1, 0] = equation[1, 0] - (equation[0, 0] * secondRowMultiplier);
            equation[1, 1] = equation[1, 1] - (equation[0, 1] * secondRowMultiplier);
            equation[1, 2] = equation[1, 2] - (equation[0, 2] * secondRowMultiplier);
            var secondRowDivider = equation[1, 1];
            equation[1, 0] /= secondRowDivider;
            equation[1, 1] /= secondRowDivider;
            equation[1, 2] /= secondRowDivider;
            var firstRowMultiplier = equation[0, 1];
            equation[0, 0] = equation[0, 0] - (equation[1, 0] * firstRowMultiplier);
            equation[0, 1] = equation[0, 1] - (equation[1, 1] * firstRowMultiplier);
            equation[0, 2] = equation[0, 2] - (equation[1, 2] * firstRowMultiplier);

            return ((decimal)(double)equation[0, 2], (decimal)(double)equation[1, 2]);
        }

        private string SolveFirstPuzzle(List<decimal[,]> equations)
        {
            var firstPuzzleSolution = 0L;

            foreach (var equation in equations)
            {
                var equationCopy = new decimal[equation.GetLength(0), equation.GetLength(1)];
                Array.Copy(equation, equationCopy, equation.Length);

                var presses = GaussElimination(equationCopy);
                if (decimal.IsInteger(presses.A) && decimal.IsInteger(presses.B))
                {
                    firstPuzzleSolution += decimal.ToInt64(presses.A) * 3 + decimal.ToInt64(presses.B);
                }
            }

            return firstPuzzleSolution.ToString();
        }

        private string SolveSecondPuzzle(List<decimal[,]> equations)
        {
            var secondPuzzleSolution = 0L;

            foreach (var equation in equations)
            {
                equation[0, 2] += 10_000_000_000_000;
                equation[1, 2] += 10_000_000_000_000;
                var presses = GaussElimination(equation);
                if (decimal.IsInteger(presses.A) && decimal.IsInteger(presses.B))
                {
                    secondPuzzleSolution += decimal.ToInt64(presses.A) * 3 + decimal.ToInt64(presses.B);
                }
            }

            return secondPuzzleSolution.ToString();
        }

        private List<decimal[,]> GetInputData()
        {
            var input = LoadInputData();
            var inputRows = input.Split(Environment.NewLine);
            var equations = new List<decimal[,]>();
            var currentRow = 0;
            while (currentRow < inputRows.Length)
            {
                var equation = new decimal[2, 3];
                var buttonRegex = new Regex(@"\d+");
                var buttonARow = inputRows[currentRow];
                var buttonBRow = inputRows[++currentRow];
                var prizeRow = inputRows[++currentRow];
                var buttonAValues = buttonRegex.Matches(buttonARow);
                var buttonBValues = buttonRegex.Matches(buttonBRow);
                var prizeValues = buttonRegex.Matches(prizeRow);

                equation[0, 0] = decimal.Parse(buttonAValues[0].Value);
                equation[0, 1] = decimal.Parse(buttonBValues[0].Value);
                equation[1, 0] = decimal.Parse(buttonAValues[1].Value);
                equation[1, 1] = decimal.Parse(buttonBValues[1].Value);
                equation[0, 2] = decimal.Parse(prizeValues[0].Value);
                equation[1, 2] = decimal.Parse(prizeValues[1].Value);

                equations.Add(equation);

                currentRow += 2;
            }

            return equations;
        }
    }
}
