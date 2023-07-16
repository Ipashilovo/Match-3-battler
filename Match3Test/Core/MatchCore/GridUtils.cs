using System.Drawing;
using System.Runtime.CompilerServices;

namespace Match3Test.Core
{
    public static class GridUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMin(this Span<byte> grid)
        {
            byte min = 0;
            for (byte i = 0; i < grid.Length; i++)
            {
                if (grid[i] != 0)
                {
                    min = i;
                    break;
                }
            }

            return min;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMax(this Span<byte> grid)
        {
            int max = 0;
            for (int i = grid.Length - 1; i >= 0; i--)
            {
                if (grid[i] != 0)
                {
                    max = i; 
                    break;
                }
            }

            return max;
        }
        
        
        public static void ChangePosition(Span<Cell> cells, Vector2Byte size, MoveAction moveAction)
        {
            var startCell = cells[PositionUtils.GetCurrent(moveAction.StartPosition, size)];
            var endCell = cells[PositionUtils.GetCurrent(moveAction.EndPosition, size)];
            cells[PositionUtils.GetCurrent(moveAction.StartPosition, size)] =
                new Cell(startCell.Position, endCell.Id, startCell.Direction, endCell.SpecialType);
            cells[PositionUtils.GetCurrent(moveAction.EndPosition, size)] =
                new Cell(endCell.Position, startCell.Id, endCell.Direction, startCell.SpecialType);
        }
    }
}