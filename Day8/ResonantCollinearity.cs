namespace AdventOfCode2024.Day8
{
    public class ResonantCollinearity : PuzzleBase
    {
        private const char _antinodeMark = '#';

        public override int DayNumber => 8;

        public override (string, string) Solve()
        {
            var input = GetInputData();
            var firstSolution = SolveFirstPuzzle(input);
            var secondSolution = SolveSecondPuzzle(input);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(((int, int), Dictionary<char, List<(int, int)>>) input)
        {
            var antinodesMap = GetAntinodesMap(input, ifResonantHarmonics: false);

            return antinodesMap.Cast<char>().Count(c => c == _antinodeMark).ToString();
        }

        private string SolveSecondPuzzle(((int, int), Dictionary<char, List<(int, int)>>) input)
        {
            var antinodesMap = GetAntinodesMap(input, ifResonantHarmonics: true);

            return antinodesMap.Cast<char>().Count(c => c == _antinodeMark).ToString();
        }

        private char[,] GetAntinodesMap(((int, int), Dictionary<char, List<(int, int)>>) input, bool ifResonantHarmonics)
        {
            var mapSize = input.Item1;
            var antennasLocations = input.Item2;
            var antinodesMap = new char[mapSize.Item1, mapSize.Item2];

            foreach (var antenna in antennasLocations)
            {
                var uniqueAntennaCombinations = GetUniqueCombinations(antenna.Value);

                foreach (var combination in uniqueAntennaCombinations)
                {
                    var firstAntennaLocation = combination.Item1;
                    var secondAntennaLocation = combination.Item2;
                    var distance = (firstAntennaLocation.Item1 - secondAntennaLocation.Item1, firstAntennaLocation.Item2 - secondAntennaLocation.Item2);
                    var firstAntinodeCoords = (firstAntennaLocation.Item1 + distance.Item1, firstAntennaLocation.Item2 + distance.Item2);
                    var secondAntinodeCoords = (secondAntennaLocation.Item1 - distance.Item1, secondAntennaLocation.Item2 - distance.Item2);

                    if (ifResonantHarmonics)
                    {
                        antinodesMap[firstAntennaLocation.Item1, firstAntennaLocation.Item2] = _antinodeMark;
                        antinodesMap[secondAntennaLocation.Item1, secondAntennaLocation.Item2] = _antinodeMark;

                        while (!IsAntennaOutsideMap(mapSize, firstAntinodeCoords))
                        {
                            antinodesMap[firstAntinodeCoords.Item1, firstAntinodeCoords.Item2] = _antinodeMark;
                            firstAntinodeCoords = (firstAntinodeCoords.Item1 + distance.Item1, firstAntinodeCoords.Item2 + distance.Item2);
                        }

                        while (!IsAntennaOutsideMap(mapSize, secondAntinodeCoords))
                        {
                            antinodesMap[secondAntinodeCoords.Item1, secondAntinodeCoords.Item2] = _antinodeMark;
                            secondAntinodeCoords = (secondAntinodeCoords.Item1 - distance.Item1, secondAntinodeCoords.Item2 - distance.Item2);
                        }
                    }
                    else
                    {
                        if (!IsAntennaOutsideMap(mapSize, firstAntinodeCoords))
                            antinodesMap[firstAntinodeCoords.Item1, firstAntinodeCoords.Item2] = _antinodeMark;

                        if (!IsAntennaOutsideMap(mapSize, secondAntinodeCoords))
                            antinodesMap[secondAntinodeCoords.Item1, secondAntinodeCoords.Item2] = _antinodeMark;
                    }
                }
            }

            return antinodesMap;
        }

        private bool IsAntennaOutsideMap((int, int) mapSize, (int, int) antenna)
        {
            return antenna.Item1 < 0 || antenna.Item1 >= mapSize.Item1
                || antenna.Item2 < 0 || antenna.Item2 >= mapSize.Item2;
        }

        private IEnumerable<((int, int), (int, int))> GetUniqueCombinations(List<(int, int)> antennas)
        {
            for (int i = 0; i < antennas.Count; i++)
            {
                for (int j = i + 1; j < antennas.Count; j++)
                {
                    yield return (antennas[i], antennas[j]);
                }
            }
        }

        private ((int, int), Dictionary<char, List<(int, int)>>) GetInputData()
        {
            var input = LoadInputData();
            var mapRows = input.Split(Environment.NewLine);
            var mapSize = (mapRows.Length, mapRows[0].Length);
            var antennasLocations = new Dictionary<char, List<(int, int)>>();

            for (var i = 0; i < mapRows.Length; i++)
            {
                for (var j = 0; j < mapRows[i].Length; j++)
                {
                    var currentNode = mapRows[i][j];
                    if (currentNode != '.')
                    {
                        if (antennasLocations.ContainsKey(currentNode))
                        {
                            antennasLocations[currentNode].Add((i, j));
                        }
                        else
                        {
                            antennasLocations[currentNode] = new() { (i, j) };
                        }
                    }
                }
            }

            antennasLocations.Values.ToList().RemoveAll(a => a.Count <= 1);

            return (mapSize, antennasLocations);
        }
    }
}
