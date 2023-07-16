using System.Runtime.CompilerServices;

namespace Match3Test.Core
{
    public class MatchFinder
    {
        private ISpecialHandlerContainer _specialHandlersContainer;

        public MatchFinder(ISpecialHandlerContainer specialHandlersContainer)
        {
            _specialHandlersContainer = specialHandlersContainer;
        }

        public bool FindMatchInUpdatable(GridData gridData)
        {
            bool result = false;
            int minY = Math.Clamp(gridData.Grid.GetMin() - 2, 0, gridData.Size.Y - 1);
            int maxY = Math.Clamp(gridData.Grid.GetMax() + 2, 0, gridData.Size.Y - 1);
            for (int i = 0; i < gridData.Grid.Length; i++)
            {
                gridData.Grid[i] = 0;
            }

            for (int i = minY; i <= maxY; i++)
            {
                for (byte j = 0; j < gridData.Size.X; j++)
                {
                    if ((gridData.Grid[i] & (1 << j)) != 0)
                    {
                        continue;
                    }

                    var cell = gridData.Cells[PositionUtils.GetCurrent(j, i, gridData.Size)];

                    result |= TrySetHorizontalMatch(gridData, cell, cell.Direction, cell.Id);
                    result |= TrySetVerticalMatch(gridData, cell, cell.Direction, cell.Id);
                }
            }

            return result;
        }

        public bool TryMove(GridData gridData, MoveAction moveAction)
        {
            var startCell = gridData.Cells[PositionUtils.GetCurrent(moveAction.StartPosition, gridData.Size)];
            var endCell = gridData.Cells[PositionUtils.GetCurrent(moveAction.EndPosition, gridData.Size)];
            if (startCell.Id == endCell.Id && startCell.SpecialType == SpecialType.None && endCell.SpecialType == SpecialType.None)
            {
                return false;
            }
            
            if (startCell.Position == endCell.Position && startCell.SpecialType != SpecialType.None)
            {
                FindSpecial(gridData, startCell);
                return true;
            }
            if (startCell.SpecialType != SpecialType.None)
            {
                FindSpecial(gridData, startCell);
            }
            if (endCell.SpecialType != SpecialType.None)
            {
                FindSpecial(gridData, endCell);
            }


            Direction direction;
            if (moveAction.EndPosition.X > moveAction.StartPosition.X)
            {
                direction = Direction.Up;
            }
            else if (moveAction.EndPosition.X < moveAction.StartPosition.X)
            {
                direction = Direction.Down;
            }
            else if (moveAction.EndPosition.Y > moveAction.StartPosition.Y)
            {
                direction = Direction.Left;
            }
            else
            {
                direction = Direction.Right;
            }

            bool result = false;
            var cellDirection = endCell.Direction;
            cellDirection ^= direction;
            result = TrySetHorizontalMatch(gridData, endCell, cellDirection, startCell.Id);
            result |= TrySetVerticalMatch(gridData, endCell, cellDirection, startCell.Id);

            direction = direction.Revert();
            cellDirection = startCell.Direction;
            cellDirection ^= direction;
            result |= TrySetHorizontalMatch(gridData, startCell, cellDirection, endCell.Id);
            result |= TrySetVerticalMatch(gridData, startCell, cellDirection, endCell.Id);

            return result;
        }
        public bool TryMoveNonSpecial(GridData gridData, MoveAction moveAction)
        {
            var startCell = gridData.Cells[PositionUtils.GetCurrent(moveAction.StartPosition, gridData.Size)];
            var endCell = gridData.Cells[PositionUtils.GetCurrent(moveAction.EndPosition, gridData.Size)];
            Direction direction;
            if (moveAction.EndPosition.X > moveAction.StartPosition.X)
            {
                direction = Direction.Up;
            }
            else if (moveAction.EndPosition.X < moveAction.StartPosition.X)
            {
                direction = Direction.Down;
            }
            else if (moveAction.EndPosition.Y > moveAction.StartPosition.Y)
            {
                direction = Direction.Left;
            }
            else
            {
                direction = Direction.Right;
            }

            bool result = false;
            var cellDirection = endCell.Direction;
            cellDirection ^= direction;
            result = TrySetHorizontalMatch( gridData, endCell, cellDirection, startCell.Id);
            result |= TrySetVerticalMatch(gridData, endCell, cellDirection, startCell.Id);

            direction = direction.Revert();
            cellDirection = startCell.Direction;
            cellDirection ^= direction;
            result |= TrySetHorizontalMatch( gridData, startCell, cellDirection, endCell.Id);
            result |= TrySetVerticalMatch( gridData, startCell, cellDirection, endCell.Id);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TrySetVerticalMatch(GridData gridData, Cell cell, Direction direction, int id)
        {
            var down = direction.HasFlag(Direction.Down);
            var up = direction.HasFlag(Direction.Up);
            byte startNumber = cell.Position.X, endNumber = cell.Position.X;
            int count = 1;
            Span<byte> specialCellPosition = stackalloc byte[gridData.Grid.Length];
            gridData.Grid.CopyTo(specialCellPosition);
            if (down)
            {
                var cell1 = cell;
                for (int i = cell.Position.X; i < gridData.Size.X - 1; i++)
                {
                    SetSpecialCell(cell1, ref specialCellPosition);
                    cell1 = gridData.Cells[PositionUtils.GetDown(cell1.Position, gridData.Size)];
                    if (cell1.Id != id)
                    {
                        break;
                    }

                    count++;
                    endNumber = cell1.Position.X;
                }
            }
            if (up)
            {
                var cell1 = cell;
                for (int i = 0; i < cell.Position.X; i++)
                {
                    SetSpecialCell(cell1, ref specialCellPosition);
                    cell1 = gridData.Cells[PositionUtils.GetUp(cell1.Position, gridData.Size)];
                    if (cell1.Id != id)
                    {
                        break;
                    }

                    count++;
                    startNumber = cell1.Position.X;
                }
            }

            if (count >= CellsConst.MinCountToMatch)
            {
                for (byte i = startNumber; i <= endNumber; i++)
                {
                    gridData.Grid[cell.Position.Y] |= (byte)(1 << i);
                }

                ActiveSpecialInLine(gridData, specialCellPosition);

                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TrySetHorizontalMatch(GridData gridData, Cell cell, Direction direction, int id)
        {
            var left = direction.HasFlag(Direction.Left);
            var right = direction.HasFlag(Direction.Right);
            byte startNumber = cell.Position.Y, endNumber = cell.Position.Y;
            Span<byte> specialCellPosition = stackalloc byte[gridData.Grid.Length];
            gridData.Grid.CopyTo(specialCellPosition);
            int count = 1;
            if (left)
            {
                var cell1 = cell;
                for (int i = 0; i < cell.Position.Y; i++)
                {
                    SetSpecialCell(cell1, ref specialCellPosition);
                    cell1 = gridData.Cells[PositionUtils.GetLeft(cell1.Position, gridData.Size)];
                    if (cell1.Id != id)
                    {
                        break;
                    }

                    count++;
                    startNumber = cell1.Position.Y;
                }
            }
            if (right)
            {
                
                var cell1 = cell;
                for (int i = cell.Position.Y; i < gridData.Size.Y - 1; i++)
                {
                    SetSpecialCell(cell1, ref specialCellPosition);
                    cell1 = gridData.Cells[PositionUtils.GetRight(cell1.Position, gridData.Size)];
                    if (cell1.Id != id)
                    {
                        break;
                    }

                    count++;
                    endNumber = cell1.Position.Y;
                }
            }

            if (count >= CellsConst.MinCountToMatch)
            {
                for (byte i = startNumber; i <= endNumber; i++)
                {
                    gridData.Grid[i] |= (byte)(1 << cell.Position.X);
                }

                ActiveSpecialInLine(gridData, specialCellPosition);

                return true;
            }

            return false;
        }

        private void FindSpecial(GridData gridData, Cell startCell)
        {
            if (startCell.SpecialType == SpecialType.None)
            {
                return;
            }
            _specialHandlersContainer.DoSpecial(startCell, gridData);
        }

        private void ActiveSpecialInLine(GridData gridData, Span<byte> specialCellPosition)
        {
            for (int i = 0; i < specialCellPosition.Length; i++)
            {
                if (specialCellPosition[i] != 0)
                {
                    for (int j = 0; j < gridData.Size.X; j++)
                    {
                        if ((specialCellPosition[i] & (1 << j)) != 0)
                        {
                            FindSpecial(gridData, gridData.Cells[PositionUtils.GetCurrent(j, i, gridData.Size)]);
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetSpecialCell(Cell cell, ref Span<byte> specialCellPosition)
        {
            if (cell.SpecialType != SpecialType.None)
            {
                specialCellPosition[cell.Position.Y] =
                    (byte)(specialCellPosition[cell.Position.Y] | (1 << cell.Position.X));
            }
        }
    }
}