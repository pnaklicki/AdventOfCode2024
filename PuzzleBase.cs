namespace AdventOfCode2024
{
    public abstract class PuzzleBase
    {
        public abstract int DayNumber { get; }

        public abstract (string, string) Solve();

        protected string LoadInputData()
        {
            return File.ReadAllText($"./InputFiles/Day{DayNumber}.txt");
        }
    }
}
