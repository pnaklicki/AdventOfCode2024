using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
