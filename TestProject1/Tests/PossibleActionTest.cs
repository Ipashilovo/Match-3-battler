using Match3Test.Core;
using TestProject1;

namespace Match3Test.Tests
{
    public class PossibleActionTest
    {
        [Test]
        public void CheckPositive()
        {
            Random random = new Random(100);
            var size = new Vector2Byte(5,8);
            Span<Cell> cells = stackalloc Cell[size.X * size.Y];
            new MapGenerator().GenerateWithNoCheck(size, 4, random, ref cells); 

            Assert.IsTrue(new PossibleCellsChecker().TryFindActionCellPosition(cells, size));
        }

        [Test]
        public void CheckActions()
        {
            var gridPatterns = PatternLoader.LoadPattern("FindAction");
            var mapGenerator = new MapGenerator();
            PossibleCellsChecker possibleCellsChecker = new PossibleCellsChecker();
            for (int k = 0; k < gridPatterns.Length; k++)
            {
                var grid = gridPatterns[k];
                var size = new Vector2Byte((byte)grid.Grid.Count, (byte)grid.Grid[0].Length);
                Span<Cell> cells = stackalloc Cell[size.X * size.Y];
                mapGenerator.GeneratePattern(size, 4, ref cells, grid.Grid);
                Assert.IsTrue(possibleCellsChecker.TryFindActionCellPosition(cells, size) == grid.HaveAction);
                Assert.IsTrue(possibleCellsChecker.GetActionCellsCount(cells, size) == grid.ActionCount);
            }            
        }
    }
}