using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day14
{
    class Robot
    {
        public (int X, int Y) InitialPosition { get; set; }
        public (int X, int Y) Velocity { get; set; }
    }

    public class RestroomRedoubt : PuzzleBase
    {
        public override int DayNumber => 14;

        public override (string, string) Solve()
        {
            var robots = GetInputData();

            var firstSolution = SolveFirstPuzzle(robots);
            var secondSolution = SolveSecondPuzzle(robots);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(List<Robot> robots)
        {
            var tileWidth = 101;
            var tileHeight = 103;
            var seconds = 100;
            var robotFinalPositions = new int[tileHeight, tileWidth];

            foreach (var robot in robots)
            {
                var totalRobotVelocity = (robot.Velocity.X * seconds, robot.Velocity.Y * seconds);
                var finalRobotAbsolutePosition = (robot.InitialPosition.X + totalRobotVelocity.Item1, robot.InitialPosition.Y + totalRobotVelocity.Item2);
                var finalRobotPosition = (finalRobotAbsolutePosition.Item1 % tileHeight, finalRobotAbsolutePosition.Item2 % tileWidth);
                finalRobotPosition = (finalRobotPosition.Item1 < 0 ? tileHeight + finalRobotPosition.Item1 : finalRobotPosition.Item1, finalRobotPosition.Item2 < 0 ? tileWidth + finalRobotPosition.Item2 : finalRobotPosition.Item2);
                robotFinalPositions[finalRobotPosition.Item1, finalRobotPosition.Item2]++;
            }
            
            return CalculateSafetyFactor(robotFinalPositions).ToString();
        }

        private long CalculateSafetyFactor(int[,] robotPositions)
        {
            int rows = robotPositions.GetLength(0);
            int cols = robotPositions.GetLength(1);
            int midRow = rows / 2;
            int midCol = cols / 2;

            var q1 = CalculateRobotsInQuadrant(robotPositions, 0, 0, midRow-1, midCol-1);
            var q2 = CalculateRobotsInQuadrant(robotPositions, 0, midCol+1, midRow-1, cols-1);
            var q3 = CalculateRobotsInQuadrant(robotPositions, midRow+1, 0, rows-1, midCol-1);
            var q4 = CalculateRobotsInQuadrant(robotPositions, midRow+1, midCol+1, rows-1, cols-1);

            return q1 * q2 * q3 * q4;
        }

        private long CalculateRobotsInQuadrant(int[,] robots, int rowStart, int colStart, int rowEnd, int colEnd)
        {
            var robotCount = 0L;


            for (var i = rowStart; i <= rowEnd; i++)
            {
                for (var j = colStart; j <= colEnd; j++)
                {
                    robotCount += robots[i, j];
                }
            }

            return robotCount;
        }

        private string SolveSecondPuzzle(List<Robot> robots)
        {
            var tileWidth = 101;
            var tileHeight = 103;
            var seconds = 10000;
            var robotFinalPositions = new int[tileHeight, tileWidth];
            var easterEggSecond = 0;

            for (var i = 0; i < seconds; i++)
            {
                foreach (var robot in robots)
                {
                    if (robotFinalPositions[robot.InitialPosition.Item1, robot.InitialPosition.Item2] > 0)
                        robotFinalPositions[robot.InitialPosition.Item1, robot.InitialPosition.Item2]--;
                    (int X, int Y) newPosition = (robot.InitialPosition.X + robot.Velocity.X, robot.InitialPosition.Y + robot.Velocity.Y);
                    while (newPosition.X < 0 || newPosition.X >= tileHeight)
                    {
                        if (newPosition.X < 0)
                        {
                            newPosition.X = tileHeight + newPosition.X;
                        }
                        else if (newPosition.X >= tileHeight)
                        {
                            newPosition.X = newPosition.X - tileHeight;
                        }
                    }

                    while (newPosition.Y < 0 || newPosition.Y >= tileWidth)
                    {
                        if (newPosition.Y < 0)
                        {
                            newPosition.Y = tileWidth + newPosition.Y;
                        }
                        else if (newPosition.Y >= tileWidth)
                        {
                            newPosition.Y = newPosition.Y - tileWidth;
                        }
                    }

                    robot.InitialPosition = newPosition;

                    robotFinalPositions[robot.InitialPosition.Item1, robot.InitialPosition.Item2]++;
                }

                if(AnalyzeRobotsPositionAndPrintProbableEasterEgg(robotFinalPositions, i + 1))
                {
                    easterEggSecond = i + 1;
                }
            }

            return easterEggSecond.ToString();
        }

        private bool AnalyzeRobotsPositionAndPrintProbableEasterEgg(int[,] robotsPositions, int second)
        {
            var tileHeight = robotsPositions.GetLength(0);
            var tileWidth = robotsPositions.GetLength(1);
            var foundEasterEgg = false;

            for (var x = 0; x < tileHeight; x++)
            {
                var chainLength = 0;
                var longestChainLength = 0;
                for (var y = 0; y < tileWidth; y++)
                {
                    if (robotsPositions[x, y] > 0)
                    {
                        chainLength++;
                    }
                    else
                    {
                        if (chainLength > longestChainLength)
                            longestChainLength = chainLength;
                        chainLength = 0;
                    }

                }
                if (longestChainLength >= 20)
                {
                    PrintRobots(robotsPositions);
                    foundEasterEgg = true;
                    break;
                }
            }

            return foundEasterEgg;
        }

        private void PrintRobots(int[,] robots)
        {
            for(var i = 0;i < robots.GetLength(0);i++)
            {
                for(var j = 0;j < robots.GetLength(1);j++)
                {
                    var robotsCount = robots[i, j];
                    var valueToPrint = robotsCount == 0 ? "." : robotsCount.ToString();
                    Console.Write(valueToPrint);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private List<Robot> GetInputData()
        {
            var input = LoadInputData();
            var inputRows = input.Split(Environment.NewLine);
            var robots = new List<Robot>();
            var numberRegex = new Regex("-?\\d+");

            foreach (var row in inputRows)
            {
                var numbers = numberRegex.Matches(row);
                var robot = new Robot
                {
                    InitialPosition = (int.Parse(numbers[1].Value), int.Parse(numbers[0].Value)),
                    Velocity = (int.Parse(numbers[3].Value), int.Parse(numbers[2].Value))
                };
                robots.Add(robot);
            }

            return robots;
        }
    }
}
