using System.Collections.Generic;

namespace AdventOfCode2024.Day6
{
    struct MapInput
    {
        public char[,] map;
        public (int, int) guardPosition;
        public Direction initialDirection;
    }

    enum Direction
    {
        Unknown,
        Up,
        Right,
        Down,
        Left
    }

    public class GuardGallivant : PuzzleBase
    {
        private const char _visitedDistrictMark = 'X';
        private const char _obstacleMark = '#';

        private Dictionary<Direction, (int, int)> _directionMap = new()
        {
            {Direction.Up, (-1, 0) },
            {Direction.Right, (0, 1) },
            {Direction.Down, (1, 0) },
            {Direction.Left, (0, -1) },
        };

        public override int DayNumber => 6;

        public override (string, string) Solve()
        {
            var map = GetInputData();
            var firstSolution = SolveFirstPuzzle(map);
            var secondSolution = SolveSecondPuzzle(map);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(MapInput inputMap)
        {
            var mapCopy = new char[inputMap.map.GetLength(0), inputMap.map.GetLength(1)];
            Buffer.BlockCopy(inputMap.map, 0, mapCopy, 0, inputMap.map.Length * sizeof(char));

            EvaluateGuardMoves(mapCopy, inputMap.initialDirection, inputMap.guardPosition);

            return mapCopy.Cast<char>().Count(c => c == _visitedDistrictMark).ToString();
        }

        private bool EvaluateGuardMoves(char[,] map, Direction initialDirection, (int, int) guardPosition)
        {
            var currentDirection = initialDirection;
            var currentGuardPosition = guardPosition;
            var movesWithoutNewDistrict = 0;
            bool guardLeftArea = false;

            while (true)
            {
                var nextGuardPosition = EvaluateNewGuardPosition(currentGuardPosition, currentDirection);
                if (IsPositionOutsideMap(map, nextGuardPosition))
                {
                    guardLeftArea = true;
                    break;
                }
                while (IsObstacleAtPosition(map, nextGuardPosition))
                {
                    currentDirection = currentDirection == Direction.Left ? Direction.Up : currentDirection + 1;
                    nextGuardPosition = EvaluateNewGuardPosition(currentGuardPosition, currentDirection);
                }
                currentGuardPosition = nextGuardPosition;
                if (map[currentGuardPosition.Item1, currentGuardPosition.Item2] == _visitedDistrictMark)
                    movesWithoutNewDistrict++;
                else
                {
                    movesWithoutNewDistrict = 0;
                    map[currentGuardPosition.Item1, currentGuardPosition.Item2] = _visitedDistrictMark;
                }

                if (movesWithoutNewDistrict >= map.GetLength(0) || movesWithoutNewDistrict >= map.GetLength(1))
                    break;
            }

            return guardLeftArea;
        }

        private (int, int) EvaluateNewGuardPosition((int, int) currentGuardPosition, Direction direction)
        {
            var directionCoordinates = _directionMap[direction];

            return (currentGuardPosition.Item1 + directionCoordinates.Item1,
                currentGuardPosition.Item2 + directionCoordinates.Item2);
        }

        private bool IsObstacleAtPosition(char[,] map, (int, int) position)
        {
            return map[position.Item1, position.Item2] == _obstacleMark;
        }

        private bool IsPositionOutsideMap(char[,] map, (int, int) position)
        {
            var mapLength = map.GetLength(0);
            var mapWidth = map.GetLength(1);

            return 0 > position.Item1 || position.Item1 >= mapLength || 0 > position.Item2 || position.Item2 >= mapWidth;
        }

        private string SolveSecondPuzzle(MapInput inputMap)
        {
            var secondPuzzleSolution = 0;
            var map = inputMap.map;
            var mapCopy = new char[map.GetLength(0), map.GetLength(1)];

            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (inputMap.guardPosition == (i, j) || map[i,j] == _obstacleMark)
                        continue;
                    Buffer.BlockCopy(map, 0, mapCopy, 0, map.Length * sizeof(char));
                    mapCopy[i, j] = _obstacleMark;
                    if(!EvaluateGuardMoves(mapCopy, inputMap.initialDirection, inputMap.guardPosition))
                    {
                        secondPuzzleSolution++;
                    }
                }
            }

            return secondPuzzleSolution.ToString();
        }

        private MapInput GetInputData()
        {
            var input = LoadInputData();
            var mapInput = new MapInput();
            var mapRows = input.Split(Environment.NewLine);
            var map = new char[mapRows.Length, mapRows[0].Length];

            for (var i = 0; i < mapRows.Length; i++)
            {
                for (var j = 0; j < mapRows[i].Length; j++)
                {
                    map[i, j] = mapRows[i][j];
                    switch (map[i, j])
                    {
                        case '^':
                            mapInput.initialDirection = Direction.Up;
                            break;
                        case '>':
                            mapInput.initialDirection = Direction.Right;
                            break;
                        case 'v':
                            mapInput.initialDirection = Direction.Down;
                            break;
                        case '<':
                            mapInput.initialDirection = Direction.Left;
                            break;
                        default:
                            continue;
                    }
                    mapInput.guardPosition = (i, j);
                    map[i, j] = 'X';
                }
            }
            mapInput.map = map;

            return mapInput;
        }
    }
}
