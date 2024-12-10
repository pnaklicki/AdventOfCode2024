namespace AdventOfCode2024.Day9
{
    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public class HoofIt : PuzzleBase
    {
        private Dictionary<Direction, (int, int)> _directionMap = new()
        {
            {Direction.Up, (-1, 0) },
            {Direction.Right, (0, 1) },
            {Direction.Down, (1, 0) },
            {Direction.Left, (0, -1) },
        };

        public override int DayNumber => 10;

        public override (string, string) Solve()
        {
            var map = GetInputData();

            var firstSolution = SolveFirstPuzzle(map);
            var secondSolution = SolveSecondPuzzle(map);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(int[,] map)
        {
            return EvaluateTrailScoreOrRank(map).ToString();
        }

        private string SolveSecondPuzzle(int[,] map)
        {
            return EvaluateTrailScoreOrRank(map, ifEvaluateByScore: true).ToString();
        }

        private int EvaluateTrailScoreOrRank(int[,] map, bool ifEvaluateByScore = false)
        {
            var result = 0;

            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0)
                    {
                        var visitedPeaks = ifEvaluateByScore ? null : new List<(int, int)>();
                        var trailheadScore = 0;
                        GetTrailheadScore(map, (i, j), visitedPeaks, ref trailheadScore);
                        result += trailheadScore;
                    }
                }
            }

            return result;
        }

        private void GetTrailheadScore(int[,] map, (int, int) currentTrail, List<(int, int)>? visitedPeaks, ref int trailheadScore)
        {
            var currentTrailHeight = map[currentTrail.Item1, currentTrail.Item2];

            foreach (var direction in Enum.GetValues<Direction>())
            {
                var directionCoords = _directionMap[direction];
                var newTrailLocation = (currentTrail.Item1 + directionCoords.Item1, currentTrail.Item2 + directionCoords.Item2);
                if (!IsTrailOutsideMap((map.GetLength(0), map.GetLength(1)), newTrailLocation)
                    && map[newTrailLocation.Item1, newTrailLocation.Item2] == currentTrailHeight + 1)
                {
                    var newTrailHeight = map[newTrailLocation.Item1, newTrailLocation.Item2];

                    if (newTrailHeight == 9)
                    {
                        if (visitedPeaks != null)
                        {
                            if (visitedPeaks.Contains(newTrailLocation))
                                continue;
                            visitedPeaks.Add(newTrailLocation);
                        }
                        trailheadScore++;
                    }
                    else
                    {
                        GetTrailheadScore(map, newTrailLocation, visitedPeaks, ref trailheadScore);
                    }
                }
            }
        }

        private bool IsTrailOutsideMap((int, int) mapSize, (int, int) trail)
        {
            return trail.Item1 < 0 || trail.Item1 >= mapSize.Item1
                || trail.Item2 < 0 || trail.Item2 >= mapSize.Item2;
        }

        private int[,] GetInputData()
        {
            var input = LoadInputData();
            var inputRows = input.Split(Environment.NewLine);
            var map = new int[inputRows.Length, inputRows[0].Length];

            for (var i = 0; i < inputRows.Length; i++)
            {
                for (var j = 0; j < inputRows[i].Length; j++)
                {
                    map[i, j] = int.Parse(inputRows[i][j].ToString());
                }
            }

            return map;
        }
    }
}
