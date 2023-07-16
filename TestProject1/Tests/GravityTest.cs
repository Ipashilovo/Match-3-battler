using Match3Test.Core;
using TestProject1;

namespace Match3Test.Tests
{
    public class GravityTest
    {
        [Test]
        public void HorizontalGravityTest()
        {
            var gridPatterns = PatternLoader.LoadPattern("GravityOneLine");
            var mapGenerator = new MapGenerator();
            SpecialHandlersContainer specialHandlersContainer = new SpecialHandlersContainer();
            OneColorHandler oneColorHandler = new OneColorHandler(specialHandlersContainer);
            BombHandler bombHandler = new BombHandler(specialHandlersContainer);
            specialHandlersContainer.SetSpecial(new ISpecialCellHandler[]{bombHandler, oneColorHandler});
            MatchFinder matchFinder = new MatchFinder(specialHandlersContainer);
            var gravity = new MapGravity();
            for (int k = 0; k < gridPatterns.Length; k++)
            {
                var grid = gridPatterns[k];
                var size = new Vector2Byte((byte)grid.Grid.Count, (byte)grid.Grid[0].Length);
                Span<Cell> cells = stackalloc Cell[size.X * size.Y];
                Span<byte> matchResult = stackalloc byte[(byte)grid.Grid[0].Length];
                GridData gridData = new GridData()
                {
                    Cells = cells,
                    Grid = matchResult,
                    Size = size,
                    Ids = 4
                };
                mapGenerator.GeneratePattern(size, 4, ref cells, grid.Grid);
                if (matchFinder.TryMove(gridData, grid.MoveAction))
                {
                    gravity.FindFreeCell(gridData);
                    foreach (var positiveGravityResult in grid.PositiveGravityResults)
                    {
                        int xSize = positiveGravityResult.FallCells.Count;
                        int ySize = positiveGravityResult.FallCells[0].Length;
                        for (int i = 0; i < xSize; i++)
                        {
                            for (int j = 0; j < ySize; j++)
                            {
                                if (positiveGravityResult.FallCells[i][j] != 0)
                                {
                                    Assert.IsTrue((matchResult[j] | (1 << i)) == matchResult[j]);
                                }
                                else
                                {
                                    Assert.IsTrue((matchResult[j] & (1 << i)) == 0);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}