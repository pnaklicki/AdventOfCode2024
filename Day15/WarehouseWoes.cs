namespace AdventOfCode2024.Day15
{
    class WarehouseData
    {
        public Queue<(int X, int Y)> Moves { get; set; }
        public char[,] Map { get; set; }
        public (int X, int Y) RobotPosition { get; set; }

        public WarehouseData(Queue<(int X, int Y)> moves, char[,] map, (int X, int Y) robotPosition)
        {
            Moves = moves;
            Map = map;
            RobotPosition = robotPosition;
        }

        public WarehouseData(WarehouseData other)
        {
            Map = other.Map;
            Moves = other.Moves;
            RobotPosition = other.RobotPosition;
        }
    }

    public class WarehouseWoes : PuzzleBase
    {
        private Dictionary<char, (int, int)> _directionMap = new()
        {
            {'^', (-1, 0) },
            {'>', (0, 1) },
            {'v', (1, 0) },
            {'<', (0, -1) },
        };

        public override int DayNumber => 15;

        public override (string, string) Solve()
        {
            var warehouseData = GetInputData();

            var firstSolution = SolveFirstPuzzle(warehouseData.Normal);
            var secondSolution = SolveSecondPuzzle(warehouseData.Wide);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(WarehouseData warehouseData)
        {
            while (warehouseData.Moves.Count > 0)
            {
                var nextMove = warehouseData.Moves.Dequeue();
                var newRobotPosition = (warehouseData.RobotPosition.X + nextMove.X, warehouseData.RobotPosition.Y + nextMove.Y);
                var elementAtNewPosition = warehouseData.Map[newRobotPosition.Item1, newRobotPosition.Item2];

                if (elementAtNewPosition == '.')
                {
                    warehouseData.RobotPosition = newRobotPosition;
                }
                else if (elementAtNewPosition == '#')
                {
                    continue;
                }
                else
                {
                    var freeSpaceForBox = FindFreeSpaceToMoveBox(warehouseData.Map, newRobotPosition, nextMove);
                    if (freeSpaceForBox.HasValue)
                    {
                        warehouseData.Map[newRobotPosition.Item1, newRobotPosition.Item2] = '.';
                        warehouseData.Map[freeSpaceForBox.Value.X, freeSpaceForBox.Value.Y] = 'O';
                        warehouseData.RobotPosition = newRobotPosition;
                    }
                }
            }

            return CalculateSumOfBoxesCoords(warehouseData.Map).ToString();
        }

        private string SolveSecondPuzzle(WarehouseData warehouseData)
        {
            while (warehouseData.Moves.Count > 0)
            {
                var nextMove = warehouseData.Moves.Dequeue();
                var newRobotPosition = (warehouseData.RobotPosition.X + nextMove.X, warehouseData.RobotPosition.Y + nextMove.Y);
                var elementAtNewPosition = warehouseData.Map[newRobotPosition.Item1, newRobotPosition.Item2];

                if (elementAtNewPosition == '.')
                {
                    warehouseData.RobotPosition = newRobotPosition;
                }
                else if (elementAtNewPosition == '#')
                {
                    continue;
                }
                else
                {
                    var potentialMovedMap = MoveWiderBox(warehouseData.Map, newRobotPosition, nextMove);
                    if (potentialMovedMap != null)
                    {
                        warehouseData.Map = potentialMovedMap;
                        warehouseData.RobotPosition = newRobotPosition;
                    }
                }
            }

            return CalculateSumOfWideBoxesCoords(warehouseData.Map).ToString();
        }

        private long CalculateSumOfWideBoxesCoords(char[,] map)
        {
            var sumOfCoords = 0L;

            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '[')
                        sumOfCoords += 100 * i + j;
                }
            }

            return sumOfCoords;
        }

        private long CalculateSumOfBoxesCoords(char[,] map)
        {
            var sumOfCoords = 0L;

            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 'O')
                        sumOfCoords += 100 * i + j;
                }
            }

            return sumOfCoords;
        }

        private char[,]? MoveWiderBox(char[,] map, (int X, int Y) robotPosition, (int X, int Y) move)
        {
            (int X, int Y) firstBoxPartPosition;
            (int X, int Y) secondBoxPartPosition;


            if (map[robotPosition.X, robotPosition.Y] == '[')
            {
                firstBoxPartPosition = robotPosition;
                secondBoxPartPosition = (firstBoxPartPosition.X, firstBoxPartPosition.Y + 1);
            }
            else
            {
                secondBoxPartPosition = robotPosition;
                firstBoxPartPosition = (secondBoxPartPosition.X, secondBoxPartPosition.Y - 1);
            }

            return MoveWiderBox(ref map, (firstBoxPartPosition, secondBoxPartPosition), move) ? map : null;
        }

        private bool MoveWiderBox(ref char[,] map, ((int X, int Y) FirstPart, (int X, int Y) SecondPart) boxPosition, (int X, int Y) move)
        {
            if (move.Y == 0)
            {
                if (map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] == '.' && map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] == '.')
                {
                    map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] = '[';
                    map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] = ']';
                    map[boxPosition.FirstPart.X, boxPosition.FirstPart.Y] = '.';
                    map[boxPosition.SecondPart.X, boxPosition.SecondPart.Y] = '.';
                    return true;
                }
                else if (map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] == '#' || map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] == '#')
                {
                    return false;
                }
                else
                {
                    var firstBoxPartTouchingPart = map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y];
                    var secondBoxPartTouchingPart = map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y];
                    ((int X, int Y) FirstPart, (int X, int Y) SecondPart)? firstPotentialBox = null;
                    ((int X, int Y) FirstPart, (int X, int Y) SecondPart)? secondPotentialBox = null;

                    if (firstBoxPartTouchingPart == '[')
                    {
                        firstPotentialBox = ((boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y), (boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y + 1));
                    }
                    else if (firstBoxPartTouchingPart == ']')
                    {
                        firstPotentialBox = ((boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y - 1), (boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y));
                    }

                    if (secondBoxPartTouchingPart == '[')
                    {
                        secondPotentialBox = ((boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y), (boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y + 1));
                    }
                    else if (secondBoxPartTouchingPart == ']')
                    {
                        secondPotentialBox = ((boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y - 1), (boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y));
                    }

                    if (firstPotentialBox.HasValue && secondPotentialBox.HasValue && firstPotentialBox.Value == secondPotentialBox.Value)
                    {
                        if (MoveWiderBox(ref map, firstPotentialBox.Value, move))
                        {
                            map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] = '[';
                            map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] = ']';
                            map[boxPosition.FirstPart.X, boxPosition.FirstPart.Y] = '.';
                            map[boxPosition.SecondPart.X, boxPosition.SecondPart.Y] = '.';

                            return true;
                        }
                    }
                    else if (firstPotentialBox.HasValue && secondPotentialBox.HasValue)
                    {
                        var firstMapCopy = (char[,])map.Clone();
                        var secondMapCopy = (char[,])map.Clone();
                        if (MoveWiderBox(ref firstMapCopy, firstPotentialBox.Value, move) && MoveWiderBox(ref secondMapCopy, secondPotentialBox.Value, move))
                        {
                            MoveWiderBox(ref map, firstPotentialBox.Value, move);
                            MoveWiderBox(ref map, secondPotentialBox.Value, move);

                            map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] = '[';
                            map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] = ']';
                            map[boxPosition.FirstPart.X, boxPosition.FirstPart.Y] = '.';
                            map[boxPosition.SecondPart.X, boxPosition.SecondPart.Y] = '.';

                            return true;

                        }
                    }
                    else if (firstPotentialBox.HasValue || secondPotentialBox.HasValue)
                    {
                        var singleBox = firstPotentialBox ?? secondPotentialBox.Value;
                        if (MoveWiderBox(ref map, singleBox, move))
                        {
                            map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] = '[';
                            map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] = ']';
                            map[boxPosition.FirstPart.X, boxPosition.FirstPart.Y] = '.';
                            map[boxPosition.SecondPart.X, boxPosition.SecondPart.Y] = '.';

                            return true;
                        }
                    }
                }
            }
            else
            {
                if (move.Y == 1 && map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] == '.')
                {
                    map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] = '[';
                    map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] = ']';
                    map[boxPosition.FirstPart.X, boxPosition.FirstPart.Y] = '.';
                    return true;
                }
                else if (move.Y == -1 && map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] == '.')
                {
                    map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] = '[';
                    map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] = ']';
                    map[boxPosition.SecondPart.X, boxPosition.SecondPart.Y] = '.';
                    return true;
                }
                else if (move.Y == 1 && map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] == '#')
                {
                    return false;
                }
                else if (move.Y == -1 && map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] == '#')
                {
                    return false;
                }
                else if (move.Y == 1 && map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] == '[')
                {
                    if (MoveWiderBox(ref map, ((boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y + 1), (boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y + 1)), move))
                    {
                        map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] = '[';
                        map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] = ']';
                        map[boxPosition.FirstPart.X, boxPosition.FirstPart.Y] = '.';
                        return true;
                    }
                }
                else if (move.Y == -1 && map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] == ']')
                {
                    if (MoveWiderBox(ref map, ((boxPosition.FirstPart.X, boxPosition.FirstPart.Y + move.Y - 1), (boxPosition.SecondPart.X, boxPosition.SecondPart.Y + move.Y - 1)), move))
                    {
                        map[boxPosition.FirstPart.X + move.X, boxPosition.FirstPart.Y + move.Y] = '[';
                        map[boxPosition.SecondPart.X + move.X, boxPosition.SecondPart.Y + move.Y] = ']';
                        map[boxPosition.SecondPart.X, boxPosition.SecondPart.Y] = '.';
                        return true;
                    }
                }
            }
            return false;
        }


        private (int X, int Y)? FindFreeSpaceToMoveBox(char[,] map, (int X, int Y) robotPosition, (int X, int Y) move)
        {
            while (map[robotPosition.X, robotPosition.Y] != '#')
            {
                if (map[robotPosition.X, robotPosition.Y] == '.')
                {
                    return (robotPosition.X, robotPosition.Y);
                }
                robotPosition = (robotPosition.X + move.X, robotPosition.Y + move.Y);
            }

            return null;
        }

        private (WarehouseData Normal, WarehouseData Wide) GetInputData()
        {
            var input = LoadInputData();
            var inputRows = input.Split(Environment.NewLine);
            var mapRows = inputRows.TakeWhile(r => r != string.Empty).ToList();
            var movesRows = inputRows.Except(mapRows).ToList();
            movesRows.RemoveAt(0);
            var movesQueue = new Queue<(int X, int Y)>();
            var map = new char[mapRows.Count, mapRows[0].Length];
            var widerMap = new char[mapRows.Count, mapRows[0].Length * 2];
            (int, int) robotPosition = (-1, -1);

            for (var i = 0; i < mapRows.Count; i++)
            {
                for (var j = 0; j < mapRows[i].Length; j++)
                {
                    var currentMapElement = mapRows[i][j];
                    if (currentMapElement == '@')
                    {
                        robotPosition = (i, j);
                        widerMap[i, j * 2] = '.';
                        widerMap[i, j * 2 + 1] = '.';
                        map[i, j] = '.';
                    }
                    else
                    {
                        map[i, j] = currentMapElement;
                        widerMap[i, j * 2] = currentMapElement == 'O' ? '[' : currentMapElement;
                        widerMap[i, j * 2 + 1] = currentMapElement == 'O' ? ']' : currentMapElement;
                    }
                }
            }

            foreach (var moveRow in movesRows)
            {
                foreach (var move in moveRow)
                {
                    movesQueue.Enqueue(_directionMap[move]);
                }
            }

            var normalWarehouseData = new WarehouseData(movesQueue, map, robotPosition);
            var wideWarehouseData = new WarehouseData(new(movesQueue), widerMap, (robotPosition.Item1, robotPosition.Item2 * 2));

            return (normalWarehouseData, wideWarehouseData);
        }
    }
}
