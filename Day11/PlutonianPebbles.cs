namespace AdventOfCode2024.Day11
{
    public class PlutonianPebbles : PuzzleBase
    {
        private const int _blinkingCountFirstPuzzle = 25;
        private const int _blinkingCountSecondPuzzle = 75;

        public override int DayNumber => 11;

        public override (string, string) Solve()
        {
            var stones = GetInputData();
            var stonesCopy = new List<long>(stones);

            var firstSolution = SolveFirstPuzzle(stones);
            var secondSolution = SolveSecondPuzzle(stonesCopy);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(List<long> stones)
        {
            return GetNumberOfStonesAfterBlinkingCount(stones, _blinkingCountFirstPuzzle).ToString();
        }

        private string SolveSecondPuzzle(List<long> stones)
        {
            return GetNumberOfStonesAfterBlinkingCount(stones, _blinkingCountSecondPuzzle).ToString();
        }

        private long GetNumberOfStonesAfterBlinkingCount(List<long> initialStones, int blinkingCount)
        {
            var stoneCounts = new Dictionary<long, long>();

            foreach (var stone in initialStones)
            {
                if (stoneCounts.ContainsKey(stone))
                    stoneCounts[stone]++;
                else
                    stoneCounts[stone] = 1;
            }

            for (var blink = 0; blink < blinkingCount; blink++)
            {
                var newStoneCounts = new Dictionary<long, long>();

                foreach (var entry in stoneCounts)
                {
                    var stone = entry.Key;
                    var count = entry.Value;

                    if (stone == 0)
                    {
                        if (newStoneCounts.ContainsKey(1))
                            newStoneCounts[1] += count;
                        else
                            newStoneCounts[1] = count;
                    }
                    else
                    {
                        var length = (int)Math.Log10(stone) + 1;
                        if (length % 2 == 0)
                        {
                            var divisor = (long)Math.Pow(10, length / 2);
                            var firstPart = stone / divisor;
                            var secondPart = stone % divisor;

                            if (newStoneCounts.ContainsKey(firstPart))
                                newStoneCounts[firstPart] += count;
                            else
                                newStoneCounts[firstPart] = count;

                            if (newStoneCounts.ContainsKey(secondPart))
                                newStoneCounts[secondPart] += count;
                            else
                                newStoneCounts[secondPart] = count;
                        }
                        else
                        {
                            var newStone = stone * 2024;
                            if (newStoneCounts.ContainsKey(newStone))
                                newStoneCounts[newStone] += count;
                            else
                                newStoneCounts[newStone] = count;
                        }
                    }
                }

                stoneCounts = newStoneCounts;
            }

            long totalStones = 0;
            foreach (var count in stoneCounts.Values)
            {
                totalStones += count;
            }

            return totalStones;
        }

        private List<long> GetInputData()
        {
            var input = LoadInputData();
            var stonesInput = input.Split(" ");
            var stones = new List<long>();

            for (var i = 0; i < stonesInput.Length; i++)
            {
                var stone = long.Parse(stonesInput[i].ToString());
                stones.Add(stone);
            }

            return stones;
        }
    }
}
