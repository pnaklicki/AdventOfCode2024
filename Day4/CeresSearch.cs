namespace AdventOfCode2024.Day4
{
    public class CeresSearch : PuzzleBase
    {
        public override int DayNumber => 4;

        private const string _searchedStringFirstPuzzle = "XMAS";
        private const string _searchedStringSecondPuzzle = "MAS";

        public override (string, string) Solve()
        {
            var inputMatrix = GetInputData();
            var firstSolution = SolveFirstPuzzle(inputMatrix);
            var secondSolution = SolveSecondPuzzle(inputMatrix);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(char[,] inputMatrix)
        {
            var totalFoundStringNumber = 0;
            for (var x = 0; x < inputMatrix.GetLength(0); x++)
            {
                for (var y = 0; y < inputMatrix.GetLength(1); y++)
                {
                    if (inputMatrix[x, y] == _searchedStringFirstPuzzle[0])
                    {
                        SearchStringStartingFromElement(x, y, inputMatrix, 1, null, null, ref totalFoundStringNumber);
                    }
                }
            }

            return totalFoundStringNumber.ToString();
        }

        private void SearchStringStartingFromElement(int x, int y, char[,] inputMatrix, int currentlySearchedChar, int? directionX, int? directionY, ref int totalStringOccurrences)
        {
            if (directionX.HasValue && directionY.HasValue)
            {
                var newX = x + directionX.Value;
                var newY = y + directionY.Value;
                if (newX < 0 || newX >= inputMatrix.GetLength(0)
                    || newY < 0 || newY >= inputMatrix.GetLength(1))
                    return;

                if (inputMatrix[newX, newY] == _searchedStringFirstPuzzle[currentlySearchedChar])
                {
                    if (currentlySearchedChar == _searchedStringFirstPuzzle.Length - 1)
                        totalStringOccurrences++;
                    else
                        SearchStringStartingFromElement(newX, newY, inputMatrix, currentlySearchedChar + 1, directionX, directionY, ref totalStringOccurrences);
                }
            }
            else
            {
                for (var i = -1; i <= 1; i++)
                {
                    var newX = x + i;
                    if (newX < 0 || newX >= inputMatrix.GetLength(0))
                        continue;
                    for (var j = -1; j <= 1; j++)
                    {
                        var newY = y + j;
                        if (newY < 0 || newY >= inputMatrix.GetLength(1))
                            continue;
                        if (inputMatrix[newX, newY] == _searchedStringFirstPuzzle[currentlySearchedChar])
                        {
                            if (currentlySearchedChar == _searchedStringFirstPuzzle.Length - 1)
                                totalStringOccurrences++;
                            else
                                SearchStringStartingFromElement(newX, newY, inputMatrix, currentlySearchedChar + 1, i, j, ref totalStringOccurrences);
                        }
                    }
                }
            }
        }

        private string SolveSecondPuzzle(char[,] inputMatrix)
        {
            var totalFoundXmasNumber = 0;
            for (var x = 0; x < inputMatrix.GetLength(0); x++)
            {
                for (var y = 0; y < inputMatrix.GetLength(1); y++)
                {
                    if (inputMatrix[x, y] == 'A')
                    {
                        if (x == 0 || x == inputMatrix.GetLength(0) - 1 || y == 0 || y == inputMatrix.GetLength(1) - 1)
                            continue;
                        var firstLeterFirstDiag = inputMatrix[x - 1, y - 1];
                        var secondLetterFirstDiag = inputMatrix[x + 1, y + 1];
                        var firstLetterSecondDiag = inputMatrix[x + 1, y - 1];
                        var secondLetterSecondDiag = inputMatrix[x - 1, y + 1];

                        if (!_searchedStringSecondPuzzle.Except([firstLeterFirstDiag, 'A', secondLetterFirstDiag]).Any()
                            && !_searchedStringSecondPuzzle.Except([firstLetterSecondDiag, 'A', secondLetterSecondDiag]).Any())
                            totalFoundXmasNumber++;
                    }
                }
            }

            return totalFoundXmasNumber.ToString();
        }

        private char[,] GetInputData()
        {
            var input = LoadInputData();

            var rows = input.Split(Environment.NewLine);
            var rowsCount = rows.Length;
            var colsCount = rows[0].Length;

            var inputMatrix = new char[rowsCount, colsCount];

            for (var i = 0; i < rowsCount; i++)
            {
                for (var j = 0; j < colsCount; j++)
                {
                    inputMatrix[i, j] = rows[i][j];
                }
            }

            return inputMatrix;
        }
    }
}
