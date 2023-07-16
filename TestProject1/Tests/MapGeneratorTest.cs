using System.Text;
using Match3Test.Core;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using TestProject1;

namespace Match3Test.Tests
{
    public class MapGeneratorTest
    {
        [Test]
        public void TestGeneratorNeighbors()
        {
            Random random = new Random(100);
            var size = new Vector2Byte(5,8);
            Span<Cell> cells = stackalloc Cell[size.X * size.Y];
            GridData gridData = new GridData()
            {
                Cells = cells,
                Ids = 4,
                Size = size
            };
            new MapGenerator().Generate(gridData, random);
            Cell[,] Cells = new Cell[5, 8];
            CheckMapNonError(size,  cells);
        }

        private static void CheckMapNonError(Vector2Byte size, Cell[,] Cells, Span<Cell> cells)
        {
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    Cells[i, j] = cells[i * size.Y + j];
                }
            }

            for (int j = 1; j < size.Y - 1; j++)
            {
                Assert.IsTrue((Cells[0, j].Direction | Direction.Down | Direction.Right | Direction.Left) ==
                              Cells[0, j].Direction);
            }

            Assert.IsTrue((Cells[0, size.Y - 1].Direction | Direction.Down | Direction.Left) == Cells[0, size.Y - 1].Direction);
            for (int i = 1; i < size.X - 1; i++)
            {
                for (int j = 1; j < size.Y - 1; j++)
                {
                    Assert.IsTrue((Cells[i, j].Direction | Direction.Up | Direction.Down | Direction.Right | Direction.Left) ==
                                  Cells[i, j].Direction);
                }
            }

            for (int i = 1; i < size.X - 1; i++)
            {
                Assert.IsTrue(
                    (Cells[i, 0].Direction | Direction.Up | Direction.Down | Direction.Right) == Cells[i, 0].Direction);
            }

            for (int i = 1; i < size.X - 1; i++)
            {
                Assert.IsTrue((Cells[i, size.Y - 1].Direction | Direction.Up | Direction.Down | Direction.Left) ==
                              Cells[i, size.Y - 1].Direction);
            }

            Assert.IsTrue((Cells[size.Y - 1, 0].Direction | Direction.Up | Direction.Right) == Cells[size.Y - 1, 0].Direction);
            for (int j = 1; j < size.Y - 1; j++)
            {
                Assert.IsTrue((Cells[size.X - 1, j].Direction | Direction.Up | Direction.Right | Direction.Left) ==
                              Cells[size.X - 1, j].Direction);
            }

            Assert.IsTrue((Cells[size.X - 1, size.Y - 1].Direction | Direction.Up | Direction.Left) ==
                          Cells[size.X - 1, size.Y - 1].Direction);
        }

        public static void CheckMapNonError(Vector2Byte size, Span<Cell> cells)
        {
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    cells[PositionUtils.GetCurrent(i, j, size)] = cells[i * size.Y + j];
                }
            }

            for (int j = 1; j < size.Y - 1; j++)
            {
                Assert.IsTrue((cells[PositionUtils.GetCurrent(0, j, size)].Direction | Direction.Down | Direction.Right | Direction.Left) ==
                              cells[PositionUtils.GetCurrent(0, j, size)].Direction);
            }

            Assert.IsTrue((cells[PositionUtils.GetCurrent(0, size.Y - 1, size)].Direction | Direction.Down | Direction.Left) == cells[PositionUtils.GetCurrent(0, size.Y - 1, size)].Direction);
            for (int i = 1; i < size.X - 1; i++)
            {
                for (int j = 1; j < size.Y - 1; j++)
                {
                    Assert.IsTrue((cells[PositionUtils.GetCurrent(i, j, size)].Direction | Direction.Up | Direction.Down | Direction.Right | Direction.Left) ==
                                  cells[PositionUtils.GetCurrent(i, j, size)].Direction);
                }
            }

            for (int i = 1; i < size.X - 1; i++)
            {
                Assert.IsTrue(
                    (cells[PositionUtils.GetCurrent(i, 0, size)].Direction | Direction.Up | Direction.Down | Direction.Right) == cells[PositionUtils.GetCurrent(i, 0, size)].Direction);
            }

            for (int i = 1; i < size.X - 1; i++)
            {
                Assert.IsTrue((cells[PositionUtils.GetCurrent(i, size.Y - 1, size)].Direction | Direction.Up | Direction.Down | Direction.Left) ==
                              cells[PositionUtils.GetCurrent(i, size.Y - 1, size)].Direction);
            }

            Assert.IsTrue((cells[PositionUtils.GetCurrent(size.X - 1, 0, size)].Direction | Direction.Up | Direction.Right) == cells[PositionUtils.GetCurrent(size.X - 1, 0, size)].Direction);
            for (int j = 1; j < size.Y - 1; j++)
            {
                Assert.IsTrue((cells[PositionUtils.GetCurrent(size.X - 1, j, size)].Direction | Direction.Up | Direction.Right | Direction.Left) ==
                              cells[PositionUtils.GetCurrent(size.X - 1, j, size)].Direction);
            }

            Assert.IsTrue((cells[PositionUtils.GetCurrent(size.X - 1, size.Y - 1, size)].Direction | Direction.Up | Direction.Left) ==
                          cells[PositionUtils.GetCurrent(size.X - 1, size.Y - 1, size)].Direction);

        }

        [Test]
        public void CountCheck()
        {
            Random random = new Random(100);
            var size = new Vector2Byte(5,8);
            Span<Cell> cells = stackalloc Cell[size.X * size.Y];
            GridData gridData = new GridData()
            {
                Cells = cells,
                Ids = 4,
                Size = size
            };
            new MapGenerator().Generate(gridData, random);
            Cell[,] Cells = new Cell[5, 8];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cells[i, j] = cells[i * 5 + j];
                }
            }
            Assert.IsTrue(Cells.GetLength(0) == 5);
            Assert.IsTrue(Cells.GetLength(1) == 8);
        }
        
        [Test]
        public void GenerationNonLockTest()
        {
            List<int> indexes = new List<int>();
            bool result = true;
            PossibleCellsChecker possibleCellsChecker = new PossibleCellsChecker();
            var mapGenerator = new MapGenerator();
            for (int i = 0; i < 2000; i++)
            {
                var size = new Vector2Byte(5,8);
                Span<Cell> cells = stackalloc Cell[size.X * size.Y];
                GridData gridData = new GridData()
                {
                    Cells = cells,
                    Ids = 4,
                    Size = size
                };
                Random random = new Random(i);
                for (int j = 0; j < 10; j++)
                {
                    mapGenerator.Generate(gridData, random);
                    if (!possibleCellsChecker.TryFindActionCellPosition(cells, size))
                    {
                        result = false;
                        indexes.Add(i);
                    }
                }
            }
            Assert.IsTrue(result, string.Join(",", indexes.Select(v => v.ToString())));
        }

        [Test]
        public void GeneratorStatisticTest()
        {
            Dictionary<int, float> results = new Dictionary<int, float>();
            Dictionary<int, float> ids = new Dictionary<int, float>()
            {
                { 0, 0 },
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
                { 4, 0 }
            };
            StringBuilder specialGrid = new StringBuilder();
            PossibleCellsChecker possibleCellsChecker = new PossibleCellsChecker();
            var mapGenerator = new MapGenerator();
            var checkCount = 2000;
            int count = 0;
            for (int i = 0; i < checkCount; i++)
            {
                Random random = new Random(i);
                var size = new Vector2Byte(5,8);
                Span<Cell> cells = stackalloc Cell[size.X * size.Y];
                GridData gridData = new GridData()
                {
                    Cells = cells,
                    Ids = 4,
                    Size = size
                };
                for (int j = 0; j < 10; j++)
                {
                    mapGenerator.Generate(gridData, random);
                    foreach (var cell in cells)
                    {
                        ids[cell.Id]++;
                    }
                    count = possibleCellsChecker.GetActionCellsCount(cells, size);
                    if (results.ContainsKey(count) == false)
                    {
                        results.Add(count, 0);
                    }
                    else
                    {
                        results[count]++;
                    }

                    if (count > 10)
                    {
                        StringBuilder special = new StringBuilder();
                        int number = 0;
                        foreach (var cell in cells)
                        {
                            number++;
                            special.Append(cell.Id);
                            if (number % size.Y == 0)
                            {
                                special.Append('\n');
                            }
                        }
                        specialGrid.Append('\n');
                        specialGrid.Append('\n');
                        specialGrid.Append('\n');
                        specialGrid.Append(special);
                    }
                }

                cells.Clear();
            }

            foreach (var result in results)
            {
                results[result.Key] /= (checkCount * 10);
            }

            foreach (var id in ids)
            {
                ids[id.Key] /= (checkCount * 10);
            }
            PatternLoader.Save("MapGeneratorStatistic.txt", 
                string.Join('\n', results.OrderBy(v => v.Key).Select(v => $"{v.Key.ToString()} - {v.Value}")) 
                + "\n\n\n" +
                string.Join('\n', ids.Select(v => $"{v.Key.ToString()} - {v.Value}"))
                + specialGrid);
        }

        private class StatisticTestResult
        {
            public List<float> CountBySeeds;

            public StatisticTestResult(List<float> countBySeeds)
            {
                CountBySeeds = countBySeeds;
            }
        }
    }
}