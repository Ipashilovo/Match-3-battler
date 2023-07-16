using System.Runtime.CompilerServices;

namespace Match3Test.Core
{
    public class MapGravity
    {
        public void FindFreeCell(GridData gridData)
        {
            Span<int> freeCells = stackalloc int[gridData.Size.Y];
            int minY = gridData.Grid.GetMin();
            int maxY = gridData.Grid.GetMax();
            for (int i = minY; i <= maxY; i++)
            {
                for (int j = gridData.Size.X - 1; j >= 0; j--)
                {
                    if ((gridData.Grid[i] & (1 << j)) != 0)
                    {
                        freeCells[i]++;
                        var cell = gridData.Cells[PositionUtils.GetCurrent(j, i, gridData.Size)];
                        for (int k = j; k < (gridData.Size.X - 1); k++)
                        {
                            var nextCell = gridData.Cells[PositionUtils.GetDown(cell.Position, gridData.Size)];
                            var nextCellDirection = nextCell.Direction;
                            nextCell.Direction = cell.Direction;
                            
                            gridData.Cells[PositionUtils.GetCurrent(cell.Position, gridData.Size)] = new Cell(cell.Position, nextCell.Id, cell.Direction, nextCell.SpecialType);
                            cell = nextCell;
                            cell.Direction = nextCellDirection;
                        }
                    }
                }
            }

            for (int i = minY; i <= maxY; i++)
            {
                int result = 0;
                for (byte j = 0; j < freeCells[i]; j++)
                {
                    result |= (1 << ((gridData.Size.X - 1) - j));
                }

                gridData.Grid[i] = (byte)result;
            }
        }
    }
}