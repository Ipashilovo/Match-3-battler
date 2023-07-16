using Match3Test.Core;
using TestProject1;

namespace Match3Test.Tests
{
    public class MatchTest
    {
        [Test]
        public void PositiveFindMatchTest()
        {
            var gridPatterns = PatternLoader.LoadPattern("MatchFinderPositive");
            var mapGenerator = new MapGenerator();
            SpecialHandlersContainer specialHandlersContainer = new SpecialHandlersContainer();
            OneColorHandler oneColorHandler = new OneColorHandler(specialHandlersContainer);
            BombHandler bombHandler = new BombHandler(specialHandlersContainer);
            specialHandlersContainer.SetSpecial(new ISpecialCellHandler[]{bombHandler, oneColorHandler});
            MatchFinder matchFinder = new MatchFinder(specialHandlersContainer);
            for (int k = 0; k < gridPatterns.Length; k++)
            {
                var grid = gridPatterns[k];
                var size = new Vector2Byte((byte)grid.Grid.Count, (byte)grid.Grid[0].Length);
                Span<Cell> cells = stackalloc Cell[size.X * size.Y];
                mapGenerator.GeneratePattern(size, 4, ref cells, grid.Grid);
                Span<byte> matchResult = stackalloc byte[(byte)grid.Grid[0].Length];
                GridData gridData = new GridData()
                {
                    Cells = cells,
                    Ids = 4,
                    Size = size,
                    Grid = matchResult
                };
                for (int i = 0; i < grid.PositiveMatchFindResult.Grid.Count; i++)
                {
                    for (int j = 0; j < grid.PositiveMatchFindResult.Grid[0].Length; j++)
                    {
                        if (grid.PositiveMatchFindResult.Grid[i][j] != 0)
                        {
                            matchResult[j] |= (byte)(1 << i);
                        }

                    }
                }
                Assert.IsTrue(matchFinder.FindMatchInUpdatable(gridData));
                int xSize = grid.PositiveMatchFindResult.Result.Count;
                int ySize = grid.PositiveMatchFindResult.Result[0].Length;
                for (int i = 0; i < xSize; i++)
                {
                    for (int j = 0; j < ySize; j++)
                    {
                        if (grid.PositiveMatchFindResult.Result[i][j] != 0)
                        {
                            Assert.IsTrue((matchResult[j] | (1 << i)) == matchResult[j]);
                        }
                    }
                }
            }
        }
    }
}