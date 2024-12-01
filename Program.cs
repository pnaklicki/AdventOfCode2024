using AdventOfCode2024;
using AdventOfCode2024.Day1;
using System.Reflection;

var puzzleList = new List<PuzzleBase>
{
    new HistorianHysteria()
};

do
{

    Console.WriteLine("Welcome to Advent of Code 2024 solution hub by Patrycjusz Naklicki!");
    Console.WriteLine();
    Console.WriteLine("Available solutions are:");
    foreach (var puzzle in puzzleList)
    {
        Console.WriteLine($"{puzzleList.IndexOf(puzzle) + 1}: {puzzle.GetType().Name}");
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

    var selectedPuzzle = puzzleList[result - 1];
    var solution = selectedPuzzle.Solve();

    Console.WriteLine($"Solution for AOC 2024 Day{result} puzzles are:");
    Console.WriteLine($"Puzzle 1: {solution.Item1}");
    Console.WriteLine($"Puzzle 2: {solution.Item2}");
    Console.ReadLine();
    Console.Clear();
} while (true);