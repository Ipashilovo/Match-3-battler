using Match3Test.Core;
using TestProject1;

namespace Match3Test.Tests
{
    public class Find3
    {
        [Test]
        public void CheckPositive()
        {
            var grids = PatternLoader.LoadPattern("3Positive");
            MapGenerator mapGenerator = new MapGenerator();
            foreach (var grid in grids)
            {
                var size = new Vector2Byte((byte)grid.Grid.Count, (byte)grid.Grid[0].Length);
                Span<Cell> cells = stackalloc Cell[size.X * size.Y];
                mapGenerator.GeneratePattern(size, 4, ref cells, grid.Grid);
            }
        }
    }
}