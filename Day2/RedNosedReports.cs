namespace AdventOfCode2024.Day1
{
    public class RedNosedReports : PuzzleBase
    {
        public override int DayNumber => 2;

        public override (string, string) Solve()
        {
            var reportList = GetInputData();
            var firstSolution = SolveFirstPuzzle(reportList);
            var secondSolution = SolveSecondPuzzle(reportList);

            return (firstSolution, secondSolution);
        }

        private string SolveFirstPuzzle(List<List<int>> reportList)
        {
            var validReportsCount = 0;
            foreach (var report in reportList)
            {
                if (IsReportValid(report))
                {
                    validReportsCount++;
                }
            }
            return validReportsCount.ToString();
        }

        private string SolveSecondPuzzle(List<List<int>> reportList)
        {
            var validReportsCount = 0;

            foreach (var report in reportList)
            {
                for (var i = 0; i < report.Count; i++) 
                {
                    var reportVariant = new List<int>(report);
                    reportVariant.RemoveAt(i);
                    if(IsReportValid(reportVariant))
                    {
                        validReportsCount++;
                        break;
                    }
                }
            }

            return validReportsCount.ToString();
        }

        private bool IsReportValid(List<int> report)
        {
            var isReportValid = true;
            var isDecreasing = true;
            var isIncreasing = true;

            for (var i = 1; i < report.Count; i++)
            {
                var levelDifference = Math.Abs(report[i] - report[i - 1]);
                if (levelDifference < 1 || levelDifference > 3)
                {
                    isReportValid = false;
                }

                if (report[i] > report[i - 1])
                {
                    isDecreasing = false;
                }
                else if (report[i] < report[i - 1])
                {
                    isIncreasing = false;
                }
                else
                {
                    isReportValid = false;
                }

                if (!isDecreasing && !isIncreasing)
                {
                    isReportValid = false;
                }

                if (!isReportValid)
                {
                    break;
                }
            }

            return isReportValid;
        }

        private List<List<int>> GetInputData()
        {
            var inputString = LoadInputData();

            var reportList = new List<List<int>>();

            foreach (var inputLine in inputString.Split(Environment.NewLine))
            {
                var report = new List<int>();
                var reportContent = inputLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (var reportLevel in reportContent)
                {
                    var level = int.Parse(reportLevel);
                    report.Add(level);
                }
                reportList.Add(report);
            }

            return reportList;
        }
    }
}
