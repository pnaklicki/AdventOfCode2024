namespace AdventOfCode2024.Day9
{
    public class DiskFragmenter : PuzzleBase
    {
        public override int DayNumber => 9;

        public override (string, string) Solve()
        {
            var memory = GetInputData();
            var memoryCopy = new List<int?>(memory);

            var firstSolution = SolveFirstPuzzle(memory);
            var secondSolution = SolveSecondPuzzle(memoryCopy);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(List<int?> memory)
        {
            while (!memory.SkipWhile(d => d != null).All(d => d == null))
            {
                MoveLastElementToFirstEmptyPosition(ref memory);
            }

            return CalculateMemoryChecksum(memory).ToString();
        }

        private string SolveSecondPuzzle(List<int?> memory)
        {
            var highestFileId = memory.Max();
            var currentHighestFileId = highestFileId;

            for (var i = 0; i < highestFileId; i++)
            {
                MoveLastFileToFirstEmptyPosition(ref memory, currentHighestFileId.GetValueOrDefault(-1));
                currentHighestFileId--;
            }

            return CalculateMemoryChecksum(memory).ToString();
        }

        private long CalculateMemoryChecksum(List<int?> memory)
        {
            var checksum = 0L;

            for (var i = 0; i < memory.Count; i++)
            {
                var digit = memory[i];
                if (digit == null)
                    continue;
                checksum += i * digit.Value;
            }

            return checksum;
        }

        private void MoveLastFileToFirstEmptyPosition(ref List<int?> memory, int fileIdToMove)
        {
            var fileLength = memory.Count(d => d == fileIdToMove);
            var fileStartingIndex = memory.IndexOf(fileIdToMove);
            var fileData = memory.GetRange(fileStartingIndex, fileLength);
            var emptyMemory = new List<int?>();

            for (var i = 0; i < fileLength; i++)
            {
                emptyMemory.Add(null);
            }

            var firstAvailableIndex = GetIndexOfFirstFreeMemory(memory, emptyMemory);
            if (firstAvailableIndex != -1 && firstAvailableIndex + fileLength - 1 < fileStartingIndex)
            {
                memory.RemoveRange(firstAvailableIndex, fileLength);
                memory.InsertRange(firstAvailableIndex, fileData);
                memory.RemoveRange(fileStartingIndex, fileLength);
                memory.InsertRange(fileStartingIndex, emptyMemory);
            }
        }

        private int GetIndexOfFirstFreeMemory(List<int?> memory, List<int?> emptyMemory)
        {
            if (emptyMemory.Count == 0 || emptyMemory.Count > memory.Count)
                return -1;

            return Enumerable.Range(0, memory.Count - emptyMemory.Count + 1)
                             .FirstOrDefault(i => memory.Skip(i).Take(emptyMemory.Count).SequenceEqual(emptyMemory), -1);
        }

        private void MoveLastElementToFirstEmptyPosition(ref List<int?> memory)
        {
            var firstEmptyIndex = memory.IndexOf(null);
            if (firstEmptyIndex != -1)
            {
                var lastElementIndex = memory.Count - 1;
                while (memory[lastElementIndex] == null)
                {
                    lastElementIndex--;
                }

                var lastElement = memory[lastElementIndex];
                memory.RemoveAt(lastElementIndex);
                memory.RemoveAt(firstEmptyIndex);
                memory.Insert(firstEmptyIndex, lastElement);
                memory.Add(null);
            }
        }

        private List<int?> GetInputData()
        {
            var input = LoadInputData();
            int? currentFileId = 0;
            var memory = new List<int?>();

            for (var i = 0; i < input.Length; i++)
            {
                var digit = int.Parse(input[i].ToString());
                var blockRepresentation = (i + 1) % 2 == 0 ? null : currentFileId;

                for (var j = 1; j <= digit; j++)
                {
                    memory.Add(blockRepresentation);
                }

                if (blockRepresentation != null)
                    currentFileId++;
            }

            return memory;
        }
    }
}
