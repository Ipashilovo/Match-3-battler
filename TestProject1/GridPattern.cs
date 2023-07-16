using Match3Test.Core;

namespace TestProject1.GridPatterns
{
    public class GridPattern
    {
        public MoveAction MoveAction;
        public PositiveMatchResult[] PositiveMatchResults;
        public List<int[]> Grid;
        public PositiveGravityResult[] PositiveGravityResults;
        public PositiveMatchFindResult PositiveMatchFindResult;
        public bool HaveAction;
        public int ActionCount;
    }

    public class PositiveMatchFindResult
    {
        public List<int[]> Grid;
        public List<int[]> Result;
    }

    public class PositiveGravityResult
    {
        public List<int[]> FallCells;
        public List<int[]> UpdateGrid;
    }

    public class PositiveMatchResult
    {
        public bool IsVertical;
        public MoveAction MoveAction;
        public byte Result;
    }
}