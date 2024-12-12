using System.Linq;

namespace AdventOfCode2024.Day12
{
    class CropRegion
    {
        public char CropType { get; set; }
        public long Area { get; set; }
        public long Perimeter { get; set; }
        public HashSet<(int, int)> CropLocations { get; set; } = new HashSet<(int, int)>();
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public class GardenGroups : PuzzleBase
    {
        private Dictionary<Direction, (int, int)> _directionMap = new()
        {
            {Direction.Up, (-1, 0) },
            {Direction.Right, (0, 1) },
            {Direction.Down, (1, 0) },
            {Direction.Left, (0, -1) },
        };

        public override int DayNumber => 12;

        public override (string, string) Solve()
        {
            var regions = GetInputData();

            var firstSolution = SolveFirstPuzzle(regions);
            var secondSolution = SolveSecondPuzzle(regions);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(List<CropRegion> regions)
        {
            for (var i = 0; i < regions.Count; i++)
            {
                var currentRegion = regions[i];
                foreach (var cropLocation in currentRegion.CropLocations)
                {
                    foreach (var direction in _directionMap)
                    {
                        var neighbourCropLocation = (cropLocation.Item1 + direction.Value.Item1, cropLocation.Item2 + direction.Value.Item2);
                        if (!currentRegion.CropLocations.Contains(neighbourCropLocation))
                        {
                            currentRegion.Perimeter++;
                        }
                    }
                }
            }

            var firstPuzzleSolution = 0L;
            foreach (var region in regions)
            {
                firstPuzzleSolution += region.Area * region.Perimeter;
            }

            return firstPuzzleSolution.ToString();
        }

        private string SolveSecondPuzzle(List<CropRegion> regions)
        {
            var secondPuzzleSolution = 0L;
            var directions = new (int x, int y)[]
            {
                (0, -1),  // Left
                (-1, 0),  // Up
                (-1, -1), // LeftUp
                (-1, 1),  // RightUp
                (0, 1),   // Right
                (1, 1),   // RightDown
                (1, 0),   // Down
                (1, -1)   // LeftDown
            };


            for (var i = 0; i < regions.Count; i++)
            {
                var currentRegion = regions[i];
                currentRegion.Perimeter = 0;
                bool Contains(int x, int y) => currentRegion.CropLocations.Contains((x, y));
                void IncrementPerimeterIf(bool condition)
                {
                    if (condition)
                        currentRegion.Perimeter++;
                }

                foreach (var crop in currentRegion.CropLocations)
                {
                    var containsNeighbour = new bool[8];
                    for (int j = 0; j < directions.Length; j++)
                    {
                        var (x, y) = directions[j];
                        containsNeighbour[j] = Contains(crop.Item1 + x, crop.Item2 + y);
                    }

                    // Adjacent conditions
                    IncrementPerimeterIf(containsNeighbour[0] && containsNeighbour[1] && !containsNeighbour[2]); // Left & Up & not LeftUp
                    IncrementPerimeterIf(containsNeighbour[1] && containsNeighbour[4] && !containsNeighbour[3]); // Up & Right & not RightUp
                    IncrementPerimeterIf(containsNeighbour[4] && containsNeighbour[6] && !containsNeighbour[5]); // Right & Down & not RightDown
                    IncrementPerimeterIf(containsNeighbour[0] && containsNeighbour[6] && !containsNeighbour[7]); // Left & Down & not LeftDown

                    // Gap conditions
                    IncrementPerimeterIf(!containsNeighbour[0] && !containsNeighbour[1] && !containsNeighbour[2]); // Left, Up, LeftUp all absent
                    IncrementPerimeterIf(!containsNeighbour[1] && !containsNeighbour[4] && !containsNeighbour[3]); // Up, Right, RightUp all absent
                    IncrementPerimeterIf(!containsNeighbour[4] && !containsNeighbour[6] && !containsNeighbour[5]); // Right, Down, RightDown all absent
                    IncrementPerimeterIf(!containsNeighbour[0] && !containsNeighbour[6] && !containsNeighbour[7]); // Left, Down, LeftDown all absent

                    // Diagonal conditions
                    IncrementPerimeterIf(!containsNeighbour[0] && !containsNeighbour[1] && containsNeighbour[2]); // Left, Up absent but LeftUp present
                    IncrementPerimeterIf(!containsNeighbour[4] && !containsNeighbour[6] && containsNeighbour[5]); // Right, Down absent but RightDown present
                    IncrementPerimeterIf(!containsNeighbour[0] && !containsNeighbour[6] && containsNeighbour[7]); // Left, Down absent but LeftDown present
                    IncrementPerimeterIf(!containsNeighbour[4] && !containsNeighbour[1] && containsNeighbour[3]); // Left, Down, LeftDown all absent
                }
            }

            foreach (var region in regions)
            {
                secondPuzzleSolution += region.Area * region.Perimeter;
            }

            return secondPuzzleSolution.ToString();
        }

        private bool IsCropOutsideGarden((int, int) gardenSize, (int, int) crop)
        {
            return crop.Item1 < 0 || crop.Item1 >= gardenSize.Item1
                || crop.Item2 < 0 || crop.Item2 >= gardenSize.Item2;
        }

        private List<CropRegion> GetInputData()
        {
            var input = LoadInputData();
            var gardenRows = input.Split(Environment.NewLine);
            var gardenSize = (gardenRows.Length, gardenRows[0].Length);
            var regions = new List<CropRegion>();

            for (var i = 0; i < gardenSize.Item1; i++)
            {
                for (var j = 0; j < gardenSize.Item2; j++)
                {
                    CropRegion? currentRegion = null;
                    foreach (var direction in _directionMap)
                    {
                        var neighbourCropLocation = (i + direction.Value.Item1, j + direction.Value.Item2);
                        if (!IsCropOutsideGarden(gardenSize, neighbourCropLocation)
                            && gardenRows[neighbourCropLocation.Item1][neighbourCropLocation.Item2] == gardenRows[i][j])
                        {
                            var foundRegion = regions.FirstOrDefault(r => r.CropLocations.Contains((neighbourCropLocation.Item1, neighbourCropLocation.Item2)));
                            if (foundRegion != null && currentRegion != null && foundRegion != currentRegion)
                            {
                                currentRegion.Area += foundRegion.Area;
                                foreach (var otherCrop in foundRegion.CropLocations)
                                {
                                    currentRegion.CropLocations.Add(otherCrop);
                                }
                                regions.Remove(foundRegion);
                            }
                            else if (foundRegion != null)
                            {
                                currentRegion = foundRegion;
                            }
                        }
                    }

                    if (currentRegion == null)
                    {
                        currentRegion = new CropRegion();
                        currentRegion.CropType = gardenRows[i][j];
                        regions.Add(currentRegion);
                    }

                    currentRegion.Area++;
                    currentRegion.CropLocations.Add((i, j));
                }
            }

            return regions;
        }
    }
}
