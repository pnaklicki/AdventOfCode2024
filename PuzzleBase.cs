using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public abstract class PuzzleBase
    {
        public abstract (string, string) Solve();

        protected string LoadInputData(string inputFileName)
        {
            return File.ReadAllText($"./InputFiles/{inputFileName}.txt");
        }
    }
}
