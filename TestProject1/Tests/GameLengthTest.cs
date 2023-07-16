using Match3Test.Core;
using Newtonsoft.Json;
using TestProject1;

namespace Match3Test.Tests
{
    public class GameLengthTest
    {
        [Test]
        public void GetGameLength()
        {
            MapGenerator mapGenerator = new MapGenerator();
            int allCount = 0;
            MoveFinder moveFinder = new MoveFinder();
            MapGravity mapGravity = new MapGravity();
            PossibleCellsChecker possibleCellsChecker = new PossibleCellsChecker();
            FreeCellFiller filler = new FreeCellFiller();
            SpecialHandlersContainer specialHandlersContainer = new SpecialHandlersContainer();
            OneColorHandler oneColorHandler = new OneColorHandler(specialHandlersContainer);
            BombHandler bombHandler = new BombHandler(specialHandlersContainer);
            specialHandlersContainer.SetSpecial(new ISpecialCellHandler[]{bombHandler, oneColorHandler});
            MatchFinder matchFinder = new MatchFinder(specialHandlersContainer);
            Dictionary<int, float> statistic = new Dictionary<int, float>();
            Vector2Byte size = new Vector2Byte(5, 8);
            Span<byte> grid = stackalloc byte[size.Y];
            Span<Cell> cells = stackalloc Cell[size.X * size.Y];

            GridData gridData = new GridData()
            {
                Cells = cells,
                Grid = grid,
                Ids = 4,
                Size = size
            };
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < gridData.Grid.Length; j++)
                {
                    gridData.Grid[j] = 0;
                }
                Random random = new Random(i);
                int count = 0;
                bool canMove = false;
                mapGenerator.Generate(gridData, random);
                MapGeneratorTest.CheckMapNonError(size, cells);
                var possibleCells = possibleCellsChecker.GetActionCellsPositions(cells, size);
                MapGeneratorTest.CheckMapNonError(size, cells);
                canMove = possibleCells.Count > 0;
                allCount++;
                if (canMove == false)
                {
                    if (statistic.ContainsKey(count))
                    {
                        statistic[count]++;
                    }
                    else
                    {
                        statistic.Add(count, 1);
                    }

                    continue;
                }

                while (count <= 1000 && canMove)
                {
                    var moveAction = moveFinder.Find(gridData, possibleCells[random.Next(possibleCells.Count)], matchFinder);
                    MapGeneratorTest.CheckMapNonError(size, cells);
                    for (int j = 0; j < grid.Length; j++)
                    {
                        grid[j] = 0;
                    }

                    if (matchFinder.TryMoveNonSpecial(gridData, moveAction))
                    {
                        GridUtils.ChangePosition(cells, size, moveAction);
                        mapGravity.FindFreeCell(gridData);
                        filler.SimpleFill(gridData, random);
                        
                        while (matchFinder.FindMatchInUpdatable(gridData))
                        {
                            mapGravity.FindFreeCell(gridData);
                            filler.SimpleFill(gridData, random);
                        }

                        MapGeneratorTest.CheckMapNonError(size, cells);
                        for (int j = 0; j < grid.Length; j++)
                        {
                            grid[j] = 0;
                        }

                        count++;
                        possibleCells = possibleCellsChecker.GetActionCellsPositions(cells, size);
                        canMove = possibleCells.Count > 0;
                    }
                }

                if (statistic.ContainsKey(count))
                {
                    statistic[count]++;
                }
                else
                {
                    statistic.Add(count, 1);
                }
            }


            PatternLoader.Save("ActionCount.txt",
                string.Join('\n', statistic.OrderBy(v => v.Key).Select(v => $"{v.Key.ToString()} - count : {v.Value} percent : {(v.Value / allCount) * 100}%")));
        }

        [Test]
        public void SaveBenchmarkMoves()
        {
            MapGenerator mapGenerator = new MapGenerator();
            int allCount = 0;
            MoveFinder moveFinder = new MoveFinder();
            MapGravity mapGravity = new MapGravity();
            PossibleCellsChecker possibleCellsChecker = new PossibleCellsChecker();
            FreeCellFiller filler = new FreeCellFiller();
            SpecialHandlersContainer specialHandlersContainer = new SpecialHandlersContainer();
            OneColorHandler oneColorHandler = new OneColorHandler(specialHandlersContainer);
            BombHandler bombHandler = new BombHandler(specialHandlersContainer);
            specialHandlersContainer.SetSpecial(new ISpecialCellHandler[]{bombHandler, oneColorHandler});
            MatchFinder matchFinder = new MatchFinder(specialHandlersContainer);
            Dictionary<int, float> statistic = new Dictionary<int, float>();
            SpecialCellFinder specialCellFinder = new SpecialCellFinder();
            Vector2Byte size = new Vector2Byte(5, 7);
            MoveData moveData = new MoveData();
            List<MoveAction> moveActions;
            Span<byte> grid = stackalloc byte[size.Y];
            Span<Cell> cells = stackalloc Cell[size.X * size.Y];
            GridData gridData = new GridData()
            {
                Cells = cells,
                Grid = grid,
                Ids = 4,
                Size = size
            };
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < gridData.Grid.Length; j++)
                {
                    gridData.Grid[j] = 0;
                }
                moveActions = new List<MoveAction>();
                moveData.Seed = i;
                Random random = new Random(i);
                int count = 0;
                bool canMove = false;
                mapGenerator.Generate(gridData, random);
                MapGeneratorTest.CheckMapNonError(size, cells);
                var possibleCells = possibleCellsChecker.GetActionCellsPositions(cells, size);
                MapGeneratorTest.CheckMapNonError(size, cells);
                canMove = possibleCells.Count > 0;
                allCount++;
                if (canMove == false)
                {
                    if (statistic.ContainsKey(count))
                    {
                        statistic[count]++;
                    }
                    else
                    {
                        statistic.Add(count, 1);
                    }

                    continue;
                }
                
                while (count <= 1000 && canMove)
                {
                    var moveAction = moveFinder.Find(gridData, possibleCells[random.Next(possibleCells.Count)], matchFinder);
                    MapGeneratorTest.CheckMapNonError(size, cells);
                    for (int j = 0; j < grid.Length; j++)
                    {
                        grid[j] = 0;
                    }
                    moveActions.Add(moveAction);

                    if (matchFinder.TryMoveNonSpecial(gridData, moveAction))
                    {
                        GridUtils.ChangePosition(cells, size, moveAction);
                        specialCellFinder.FindSpecialOnMove(gridData, moveAction);
                        mapGravity.FindFreeCell(gridData);
                        filler.SimpleFill(gridData, random);
                        
                        while (matchFinder.FindMatchInUpdatable(gridData))
                        {
                            specialCellFinder.FindSpecialOnChain(gridData, random);
                            mapGravity.FindFreeCell(gridData);
                            filler.SimpleFill(gridData, random);
                        }

                        MapGeneratorTest.CheckMapNonError(size, cells);
                        for (int j = 0; j < grid.Length; j++)
                        {
                            grid[j] = 0;
                        }

                        count++;
                        possibleCells = possibleCellsChecker.GetActionCellsPositions(cells, size);
                        canMove = possibleCells.Count > 0;
                    }
                }

                if (count >= 1000)
                {
                    moveData.MoveActions = moveActions.ToArray();
                    PatternLoader.Save("Benchmark.json", JsonConvert.SerializeObject(moveData, Formatting.Indented));
                    return;
                }
            }
        }
    }
}