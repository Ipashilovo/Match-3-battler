namespace Match3Test.Core
{
    public class MapGenerator
    {
        public void Generate(GridData gridData, Random random)
        {
            Span<int> idsCount = stackalloc int[gridData.Ids + 1];

            for (byte i = 0; i < gridData.Size.X; i++)
            {
                for (byte j = 0; j < gridData.Size.Y; j++)
                {
                    int horizonyalBlock = -1;
                    int verticalBlock = -1;
                    if (i > 1 && gridData.Cells[PositionUtils.GetUp(i, j, gridData.Size)].Id == gridData.Cells[PositionUtils.GetUp((byte)(i - 1), j, gridData.Size)].Id)
                    {
                        horizonyalBlock = gridData.Cells[PositionUtils.GetUp(i, j, gridData.Size)].Id;
                    }

                    if (j > 1 && gridData.Cells[PositionUtils.GetLeft(i, j, gridData.Size)].Id == gridData.Cells[PositionUtils.GetLeft(i, (byte)(j-1), gridData.Size)].Id)
                    {
                        verticalBlock = gridData.Cells[PositionUtils.GetLeft(i, j, gridData.Size)].Id;
                    }

                    Cell cell;
                    if (horizonyalBlock != -1 || verticalBlock != -1)
                    {
                        int newId = 0;
                        int summ = Int32.MaxValue;
                        for (int k = 0; k < gridData.Ids + 1; k++)
                        {
                            if (k == horizonyalBlock || k == verticalBlock)
                            {
                                continue;
                            }
                            if (idsCount[k] < summ)
                            {
                                summ = idsCount[k];
                                newId = k;
                            }
                        }
                        cell = new Cell(new Vector2Byte(i, j), newId);
                        idsCount[newId]++;
                    }
                    else
                    {
                        var newId = random.Next(gridData.Ids+1);
                        idsCount[newId]++;
                        cell = new Cell(new Vector2Byte(i, j), newId);
                    }

                    if (i > 0)
                    {
                        ref var upCell = ref gridData.Cells[PositionUtils.GetUp(i, j, gridData.Size)];
                        upCell.Direction |= Direction.Down;
                        cell.Direction |= Direction.Up;
                    }

                    if (j > 0)
                    {
                        ref var leftCell = ref gridData.Cells[PositionUtils.GetLeft(i, j, gridData.Size)];
                        leftCell.Direction |= Direction.Right;
                        cell.Direction |= Direction.Left;
                    }

                    gridData.Cells[PositionUtils.GetCurrent(i, j, gridData.Size)] = cell;
                }
            }
        }
        
        public void GenerateWithNoCheck(Vector2Byte size, int ids, Random random, ref Span<Cell> cells)
        {
            for (byte i = 0; i < size.X; i++)
            {
                for (byte j = 0; j < size.Y; j++)
                {
                    Cell cell;
                    cell = new Cell(new Vector2Byte(i, j), random.Next(ids));
                    
                    if (i > 0)
                    {
                        ref var upCell = ref cells[PositionUtils.GetUp(i, j, size)];
                        upCell.Direction |= Direction.Down;
                        cell.Direction |= Direction.Up;
                    }

                    if (j > 0)
                    {
                        ref var leftCell = ref cells[PositionUtils.GetLeft(i, j, size)];
                        leftCell.Direction |= Direction.Right;
                        cell.Direction |= Direction.Left;
                    }

                    cells[PositionUtils.GetCurrent(i, j, size)] = cell;
                }
            }
        }

        public void GeneratePattern(Vector2Byte size, int ids, ref Span<Cell> cells, List<int[]> pattern)
        {
            for (byte i = 0; i < size.X; i++)
            {
                for (byte j = 0; j < size.Y; j++)
                {
                    Cell cell;
                    cell = new Cell(new Vector2Byte(i, j), pattern[i][j]);
                    
                    if (i > 0)
                    {
                        ref var upCell = ref cells[PositionUtils.GetUp(i, j, size)];
                        upCell.Direction |= Direction.Down;
                        cell.Direction |= Direction.Up;
                    }

                    if (j > 0)
                    {
                        ref var leftCell = ref cells[PositionUtils.GetLeft(i, j, size)];
                        leftCell.Direction |= Direction.Right;
                        cell.Direction |= Direction.Left;
                    }

                    cells[PositionUtils.GetCurrent(i, j, size)] = cell;
                }
            }
        }
    }
}