using Match3Test.Core;
using TestProject1;

namespace Match3Test.Tests
{
    public class MoveTest
    {
        [Test]
        public void MoveTestOneLine()
        {
            var gridPatterns = PatternLoader.LoadPattern("MoveMatchOneLine");
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
                Assert.IsTrue(matchFinder.TryMove(gridData, grid.MoveAction));
                foreach (var positiveMatchResult in grid.PositiveMatchResults)
                {
                    var byteResult = positiveMatchResult.Result;

                    if (positiveMatchResult.IsVertical)
                    {
                        for (int i = positiveMatchResult.MoveAction.StartPosition.X; i <= positiveMatchResult.MoveAction.EndPosition.X; i++)
                        {
                            Assert.IsTrue((matchResult[byteResult] | (1 << i)) == matchResult[byteResult]);
                        }
                    }
                    else
                    {
                        for (int i = positiveMatchResult.MoveAction.StartPosition.Y; i <= positiveMatchResult.MoveAction.EndPosition.Y; i++)
                        {
                            Assert.IsTrue(matchResult[i] == byteResult);
                        }
                    }
                }
            }
        }
        [Test]
        public void MoveTestNegative()
        {
            var gridPatterns = PatternLoader.LoadPattern("MoveMatchNegative");
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
                Assert.IsFalse(matchFinder.TryMove(gridData, grid.MoveAction));
                foreach (var result in matchResult)
                {
                    Assert.IsTrue(result == 0);
                }
            }
        }
        [Test]
        public void MoveTestTwoLine()
        {
            var gridPatterns = PatternLoader.LoadPattern("MoveMatchTwoLine");
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

                Assert.IsTrue(matchFinder.TryMove(gridData, grid.MoveAction));
                foreach (var positiveMatchResult in grid.PositiveMatchResults)
                {
                    var byteResult = positiveMatchResult.Result;

                    if (positiveMatchResult.IsVertical)
                    {
                        for (int i = positiveMatchResult.MoveAction.StartPosition.X; i <= positiveMatchResult.MoveAction.EndPosition.X; i++)
                        {
                            Assert.IsTrue((matchResult[byteResult] | (1 << i)) == matchResult[byteResult]);
                        }
                    }
                    else
                    {
                        for (int i = positiveMatchResult.MoveAction.StartPosition.Y; i <= positiveMatchResult.MoveAction.EndPosition.Y; i++)
                        {
                            Assert.IsTrue((matchResult[i] | byteResult) == matchResult[i]);
                        }
                    }
                }
            }
        }
    }
}