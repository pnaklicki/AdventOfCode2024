using AdventOfCode2024;
using System.Diagnostics;
using System.Reflection;

var puzzleList = new List<PuzzleBase>();
var puzzleTypes = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => !t.IsAbstract && typeof(PuzzleBase).IsAssignableFrom(t)).ToList();
foreach (var puzzleType in puzzleTypes)
{
    var puzzle = Activator.CreateInstance(puzzleType) as PuzzleBase;
    if (puzzle != null)
        puzzleList.Add(puzzle);
}
puzzleList = puzzleList.OrderBy(p => p.DayNumber).ToList();

do
{

    Console.WriteLine("Welcome to Advent of Code 2024 solution hub by Patrycjusz Naklicki!");
    Console.WriteLine();
    Console.WriteLine("Available solutions are:");
    foreach (var puzzle in puzzleList)
    {
        Console.WriteLine($"Day {puzzle.DayNumber}: {puzzle.GetType().Name}");
    }
    Console.WriteLine();

    Console.WriteLine("Please input day number to see solution for this days puzzle in AOC 2024");

    int result;
    bool isCorrectInput;
    do
    {
        isCorrectInput = true;
        Console.Write("Day: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out result) || result < 1 || result > 31 || puzzleList.Count < result)
        {
            Console.WriteLine("Invalid input, please try again");
            Console.WriteLine();
            isCorrectInput = false;
        }
    } while (!isCorrectInput);

    Console.WriteLine();
    Console.WriteLine("Computing solutions, please wait...");
    Console.WriteLine();
    Console.WriteLine();

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var selectedPuzzle = puzzleList[result - 1];
    var solution = selectedPuzzle.Solve();

    stopwatch.Stop();

    Console.WriteLine($"Solution for AOC 2024 Day{result} puzzles are:");
    Console.WriteLine($"Puzzle 1: {solution.Item1}");
    Console.WriteLine($"Puzzle 2: {solution.Item2}");
    Console.WriteLine();
    Console.WriteLine($"Solutions were computed in {stopwatch.Elapsed}");
    Console.ReadLine();
    Console.Clear();
} while (true);