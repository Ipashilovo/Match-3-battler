using System.Runtime.CompilerServices;

namespace Match3Test.Core
{
    public class PossibleCellsChecker
    { 
        public unsafe bool TryFindActionCellPosition(Span<Cell> cells, Vector2Byte size)
        {
            for (int x = 0; x < cells.Length; x++)
            {
                    var cell = cells[x];
                    if (cell.SpecialType != SpecialType.None)
                    {
                        return true;
                    }
                    #region CreateSpan1

                    var down = cell.Direction.HasFlag(Direction.Down);
                    var up = cell.Direction.HasFlag(Direction.Up);
                    var right = cell.Direction.HasFlag(Direction.Right);
                    var left = cell.Direction.HasFlag(Direction.Left);
                    Span<int> neighborsId = stackalloc int[5];
                    byte i = 0;
                    if (right)
                    {
                        var cell1 = cells[PositionUtils.GetRight(cell.Position.X, cell.Position.Y, size)];
                        neighborsId[cell1.Id]++;
                        if (cell1.Direction.HasFlag(Direction.Right))
                        {
                            if (cells[PositionUtils.GetRight(cell1.Position.X, cell1.Position.Y, size)].Id == cell1.Id)
                            {
                                neighborsId[cell1.Id]++;
                            }
                        }
                    }

                    if (left)
                    {
                        var cell1 = cells[PositionUtils.GetLeft(cell.Position, size)];
                        neighborsId[cell1.Id]++;
                        if (cell1.Direction.HasFlag(Direction.Left))
                        {
                            if (cells[PositionUtils.GetLeft(cell1.Position, size)].Id == cell1.Id)
                            {
                                neighborsId[cell1.Id]++;
                            }
                        }
                    }
                    if (up)
                    {
                        var cell1 = cells[PositionUtils.GetUp(cell.Position, size)];
                        neighborsId[cell1.Id]++;
                        if (cell1.Direction.HasFlag(Direction.Up))
                        {
                            if (cells[PositionUtils.GetUp(cell1.Position, size)].Id == cell1.Id)
                            {
                                neighborsId[cell1.Id]++;
                            }
                        }
                    }
                    if (down)
                    {
                        var cell1 = cells[PositionUtils.GetDown(cell.Position, size)];
                        neighborsId[cell1.Id]++;
                        if (cell1.Direction.HasFlag(Direction.Down))
                        {
                            if (cells[PositionUtils.GetDown(cell1.Position, size)].Id == cell1.Id)
                            {
                                neighborsId[cell1.Id]++;
                            }
                        }
                    }
                    #endregion

                    for (byte k = 0; k < 5; k++)
                    {
                        if (neighborsId[k] > 2)
                        { 
                            return true;
                        }
                    }
            }
            return false;
        }
        public unsafe int GetActionCellsCount(Span<Cell> cells, Vector2Byte size)
        {
            int result = 0;

            for (int x = 0; x < cells.Length; x++)
            {
                    var cell = cells[x];
                    if (cell.SpecialType != SpecialType.None)
                    {
                        result++;
                    }
                    #region CreateSpan1

                    var down = cell.Direction.HasFlag(Direction.Down);
                    var up = cell.Direction.HasFlag(Direction.Up);
                    var right = cell.Direction.HasFlag(Direction.Right);
                    var left = cell.Direction.HasFlag(Direction.Left);
                    Span<int> neighborsId = stackalloc int[5];
                    byte i = 0;
                    if (right)
                    {
                        CheckNeighbor(cells, ref neighborsId, cell, size, PositionUtils.GetRight);
                    }

                    if (left)
                    {
                        CheckNeighbor(cells, ref neighborsId, cell, size, PositionUtils.GetLeft);
                    }
                    if (up)
                    {
                        CheckNeighbor(cells, ref neighborsId, cell, size, PositionUtils.GetUp);
                    }
                    if (down)
                    {
                        CheckNeighbor(cells, ref neighborsId, cell, size, PositionUtils.GetDown);
                    }
                    #endregion

                    for (byte k = 0; k < 5; k++)
                    {
                        if (neighborsId[k] > 2)
                        {
                            result++;
                        }
                    }
            }

            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckNeighbor(Span<Cell> cells, ref Span<int> neighborsId, Cell cell, Vector2Byte size, Func<Vector2Byte, Vector2Byte, int> numberFunc)
        {
            var cell1 = cells[numberFunc.Invoke(cell.Position, size)];
            neighborsId[cell1.Id]++;
            if (cell1.Direction.HasFlag(Direction.Down))
            {
                if (cells[numberFunc.Invoke(cell1.Position, size)].Id == cell1.Id)
                {
                    neighborsId[cell1.Id]++;
                }
            }
        }

        public List<int> GetActionCellsPositions(Span<Cell> cells, Vector2Byte size)
        {
            List<int> result = new List<int>();
            for (int x = 0; x < cells.Length; x++)
            {
                    var cell = cells[x];
                    if (cell.SpecialType != SpecialType.None)
                    {
                        result.Add(x);
                    }
                    #region CreateSpan1

                    var down = cell.Direction.HasFlag(Direction.Down);
                    var up = cell.Direction.HasFlag(Direction.Up);
                    var right = cell.Direction.HasFlag(Direction.Right);
                    var left = cell.Direction.HasFlag(Direction.Left);
                    Span<int> neighborsId = stackalloc int[5];
                    byte i = 0;
                    if (right)
                    {
                        var cell1 = cells[PositionUtils.GetRight(cell.Position.X, cell.Position.Y, size)];
                        neighborsId[cell1.Id]++;
                        if (cell1.Direction.HasFlag(Direction.Right))
                        {
                            if (cells[PositionUtils.GetRight(cell1.Position.X, cell1.Position.Y, size)].Id == cell1.Id)
                            {
                                neighborsId[cell1.Id]++;
                            }
                        }
                    }

                    if (left)
                    {
                        var cell1 = cells[PositionUtils.GetLeft(cell.Position, size)];
                        neighborsId[cell1.Id]++;
                        if (cell1.Direction.HasFlag(Direction.Left))
                        {
                            if (cells[PositionUtils.GetLeft(cell1.Position, size)].Id == cell1.Id)
                            {
                                neighborsId[cell1.Id]++;
                            }
                        }
                    }
                    if (up)
                    {
                        var cell1 = cells[PositionUtils.GetUp(cell.Position, size)];
                        neighborsId[cell1.Id]++;
                        if (cell1.Direction.HasFlag(Direction.Up))
                        {
                            if (cells[PositionUtils.GetUp(cell1.Position, size)].Id == cell1.Id)
                            {
                                neighborsId[cell1.Id]++;
                            }
                        }
                    }
                    if (down)
                    {
                        var cell1 = cells[PositionUtils.GetDown(cell.Position, size)];
                        neighborsId[cell1.Id]++;
                        if (cell1.Direction.HasFlag(Direction.Down))
                        {
                            if (cells[PositionUtils.GetDown(cell1.Position, size)].Id == cell1.Id)
                            {
                                neighborsId[cell1.Id]++;
                            }
                        }
                    }
                    #endregion

                    for (byte k = 0; k < 5; k++)
                    {
                        if (neighborsId[k] > 2)
                        {
                            result.Add(x);
                            break;
                        }
                    }
            }
            return result;
        }

    }
}