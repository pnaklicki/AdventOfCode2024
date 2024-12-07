namespace AdventOfCode2024.Day7
{
    struct Equation
    {
        public long result;
        public List<long> numbers;
    }

    public class BridgeRepair : PuzzleBase
    {
        private List<Func<long, long, long>> _possibleOperationsFirstPuzzle = new()
        {
            (a, b) => { return a + b; } ,
            (a, b) => { return a * b; }
        };

        private List<Func<long, long, long>> _possibleOperationsSecondPuzzle = new()
        {
            (a, b) => { return a + b; },
            (a, b) => { return a * b; },
            (a, b) => { return long.Parse($"{a}{b}"); }
        };

        public override int DayNumber => 7;

        public override (string, string) Solve()
        {
            var equations = GetInputData();
            var firstSolution = SolveFirstPuzzle(equations);
            var secondSolution = SolveSecondPuzzle(equations);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(List<Equation> equations)
        {
            return EvaluateCalibrationResultOfEquations(equations, _possibleOperationsFirstPuzzle).ToString();
        }

        private string SolveSecondPuzzle(List<Equation> equations)
        {
            return EvaluateCalibrationResultOfEquations(equations, _possibleOperationsSecondPuzzle).ToString();
        }

        private long EvaluateCalibrationResultOfEquations(List<Equation> equations, List<Func<long, long, long>> possibleOperations)
        {
            var calibrationResult = 0L;
            foreach (var equation in equations)
            {
                if (equation.numbers.Count == 1)
                {
                    if (equation.numbers.First() == equation.result)
                    {
                        calibrationResult += equation.numbers.First();
                        continue;
                    }
                }
                else
                {
                    var possibleCombinations = GetPossibleOperationsCombinations(possibleOperations, equation.numbers.Count);
                    foreach (var possibleCombination in possibleCombinations)
                    {
                        var result = equation.numbers.First();
                        for (var i = 0; i < possibleCombination.Count; i++)
                        {
                            result = possibleCombination.ElementAt(i).Invoke(result, equation.numbers.ElementAt(i + 1));
                        }
                        if (result == equation.result)
                        {
                            calibrationResult += result;
                            break;
                        }
                    }

                }
            }

            return calibrationResult;
        }

        private List<List<Func<long, long, long>>> GetPossibleOperationsCombinations(List<Func<long, long, long>> possibleOperations, int numbersCount)
        {
            var possibleCombinations = new List<List<Func<long, long, long>>>();
            GenerateCombinationsRecursive(possibleOperations, new(), numbersCount - 1, possibleCombinations);
            return possibleCombinations;
        }

        private void GenerateCombinationsRecursive(List<Func<long, long, long>> possibleOperations, List<Func<long, long, long>> currentCombination,
            int operationsCount, List<List<Func<long, long, long>>> possibleCombinations)
        {
            if (currentCombination.Count == operationsCount)
            {
                possibleCombinations.Add(new(currentCombination));
                return;
            }

            foreach (var operation in possibleOperations)
            {
                currentCombination.Add(operation);
                GenerateCombinationsRecursive(possibleOperations, currentCombination, operationsCount, possibleCombinations);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        private List<Equation> GetInputData()
        {
            var input = LoadInputData();
            var equations = new List<Equation>();

            foreach (var equationString in input.Split(Environment.NewLine))
            {
                var equation = new Equation();
                var equationParts = equationString.Split(":", StringSplitOptions.RemoveEmptyEntries);
                equation.result = long.Parse(equationParts[0]);
                equation.numbers = new List<long>();
                foreach (var number in equationParts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries))
                {
                    equation.numbers.Add(int.Parse(number));
                }
                equations.Add(equation);
            }

            return equations;
        }
    }
}
