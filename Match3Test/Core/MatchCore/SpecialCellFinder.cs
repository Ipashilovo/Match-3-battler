namespace Match3Test.Core
{
    public class SpecialCellFinder
    {
        public void FindSpecialOnMove(GridData gridData, MoveAction moveAction)
        {
            CheckLine(gridData, gridData.Cells[PositionUtils.GetCurrent(moveAction.EndPosition, gridData.Size)]);
            CheckLine(gridData, gridData.Cells[PositionUtils.GetCurrent(moveAction.StartPosition, gridData.Size)]);
        }

        public void FindSpecialOnChain(GridData gridData, Random random)
        {
            Span<byte> checkedGrid = stackalloc byte[gridData.Size.Y];
            int minValue = gridData.Grid.GetMin();
            int maxValue = gridData.Grid.GetMin();
            for (int i = minValue; i <= maxValue; i++)
            {
                for (int j = 0; j < gridData.Size.X; j++)
                {
                    if ((gridData.Grid[i] & (1 << j)) == 0)
                        continue;
                    if ((checkedGrid[i] & (1 << j)) != 0)
                        continue;
                    CheckCell(gridData, gridData.Cells[PositionUtils.GetCurrent(j, i, gridData.Size)], checkedGrid, random);
                }
            }
        }

        private void CheckCell(GridData gridData, Cell first, Span<byte> checkedGrid, Random random)
        {
            Span<byte> lineToCheck = stackalloc byte[gridData.Size.Y];
            lineToCheck[first.Position.Y] = (byte)(lineToCheck[first.Position.Y] & (1 << first.Position.X));
            FindHorizontal(gridData, first, checkedGrid, lineToCheck);
            FindVertical(gridData, first, checkedGrid, lineToCheck);
            int minValue = lineToCheck.GetMin();
            int maxValue = lineToCheck.GetMax();
            int count = 0;
            for (int i = minValue; i <= maxValue; i++)
            {
                for (int j = 0; j < gridData.Size.X; j++)
                {
                    if ((lineToCheck[i] & (1 << j)) != 0)
                    {
                        count++;
                    }
                }
            }

            int number = 0;
            if (count >= CellsConst.MinNumberToGenerateSpecialCell)
            {
                number = random.Next(count);
            }
            else
            {
                return;
            }

            int currentCounter = 0;
            for (int i = minValue; i <= maxValue; i++)
            {
                for (int j = 0; j < gridData.Size.X; j++)
                {
                    if ((lineToCheck[i] & (1 << j)) != 0)
                    {
                        currentCounter++;
                        if (currentCounter == number)
                        {
                            SetSpecial(gridData, gridData.Cells[PositionUtils.GetCurrent(j,i, gridData.Size)], count);
                            return;
                        }
                    }
                }
            }

        }


        private void FindVertical(GridData gridData, Cell first, Span<byte> checkedGrid, Span<byte> gridToCheck)
        {
            first.Direction.DirectionToBoolVertical(out bool up, out bool down);
            int count = 1;
            int startNumber = first.Position.X;
            int endNumber = first.Position.X;
            if (down)
            {
                var cell1 = gridData.Cells[PositionUtils.GetDown(first.Position, gridData.Size)];
                for (int i = cell1.Position.X; i < gridData.Size.X - 1; i++)
                {
                    if ((gridData.Grid[first.Position.Y] & (1 << i)) == 0)
                    {
                        break;
                    }

                    if ((checkedGrid[first.Position.Y] & (1 << i)) != 0)
                    {
                        break;
                    }

                    endNumber = i;
                    count++;
                    cell1 = gridData.Cells[PositionUtils.GetDown(cell1.Position, gridData.Size)];
                    if (cell1.Id != first.Id)
                    {
                        break;
                    }
                }
            }

            if (up)
            {
                var cell1 = gridData.Cells[PositionUtils.GetUp(first.Position, gridData.Size)];
                for (int i = cell1.Position.X; i > 0; i--)
                {
                    if ((gridData.Grid[first.Position.Y] & (1 << i)) == 0)
                    {
                        break;
                    }

                    if ((checkedGrid[first.Position.Y] & (1 << i)) != 0)
                    {
                        break;
                    }

                    startNumber = i;
                    count++;
                    cell1 = gridData.Cells[PositionUtils.GetUp(cell1.Position, gridData.Size)];
                    if (cell1.Id != first.Id)
                    {
                        break;
                    }
                }
            }

            if (count >= CellsConst.MinCountToMatch)
            {
                for (int i = startNumber; i <= endNumber; i++)
                {
                    gridToCheck[first.Position.Y] = (byte)(gridToCheck[first.Position.Y] | (1 << i));
                    checkedGrid[first.Position.Y] = (byte)(checkedGrid[first.Position.Y] | (1 << i));
                    if (i != first.Position.X)
                    {
                        FindHorizontal(gridData, gridData.Cells[PositionUtils.GetCurrent(i, first.Position.Y, gridData.Size)], checkedGrid, gridToCheck);
                    }
                }
            }
        }

        private void FindHorizontal(GridData gridData, Cell first, Span<byte> checkedGrid, Span<byte> gridToCheck)
        {
            first.Direction.DirectionToBoolHorizontal(out bool right, out bool left);
            int count = 1;
            int startNumber = first.Position.Y;
            int endNumber = first.Position.Y;
            if (right)
            {
                var cell1 = gridData.Cells[PositionUtils.GetRight(first.Position, gridData.Size)];
                for (int i = cell1.Position.Y; i < gridData.Size.Y - 1; i++)
                {
                    if ((gridData.Grid[i] & (1 << first.Position.X)) == 0)
                    {
                        break;
                    }

                    if ((checkedGrid[i] & (1 << first.Position.X)) != 0)
                    {
                        break;
                    }
                    
                    count++;
                    endNumber = i;
                    cell1 = gridData.Cells[PositionUtils.GetRight(cell1.Position, gridData.Size)];
                    if (cell1.Id != first.Id)
                    {
                        break;
                    }
                }
            }

            if (left)
            {
                var cell1 = gridData.Cells[PositionUtils.GetLeft(first.Position, gridData.Size)];
                for (int i = cell1.Position.Y; i > 0; i--)
                {
                    if ((gridData.Grid[i] & (1 << first.Position.X)) == 0)
                    {
                        break;
                    }

                    if ((checkedGrid[i] & (1 << first.Position.X)) != 0)
                    {
                        break;
                    }
                    
                    startNumber = i;
                    count++;
                    cell1 = gridData.Cells[PositionUtils.GetLeft(cell1.Position, gridData.Size)];
                    if (cell1.Id != first.Id)
                    {
                        break;
                    }
                }
            }

            if (count >= CellsConst.MinCountToMatch)
            {
                for (int i = startNumber; i <= endNumber; i++)
                {
                    gridToCheck[i] = (byte)(gridToCheck[i] | (1 << first.Position.X));
                    checkedGrid[i] = (byte)(checkedGrid[i] | (1 << first.Position.X));
                    if (i != first.Position.Y)
                    {
                        FindVertical(gridData, gridData.Cells[PositionUtils.GetCurrent(first.Position.X, i, gridData.Size)], checkedGrid, gridToCheck);
                    }
                }
            }
        }

        private void CheckLine(GridData gridData, Cell cell)
        {
            if ((gridData.Grid[cell.Position.Y] & (1 << cell.Position.X)) == 0)
            {
                return;
            }
            cell.Direction.DirectionToBool(out bool up, out bool down, out bool right, out bool left);
            int horizontalCount = 1;
            int verticalCount = 1;
            if (right)
            {
                var cell1 = gridData.Cells[PositionUtils.GetCurrent(cell.Position, gridData.Size)];
                for (int i = cell1.Position.Y; i < gridData.Size.Y - 1; i++)
                {
                    if (!CheckDirection(gridData, cell1, out cell1, ref horizontalCount, PositionUtils.GetRight))
                    {
                        break;
                    }
                }
            }

            if (left)
            {
                var cell1 = gridData.Cells[PositionUtils.GetCurrent(cell.Position, gridData.Size)];
                for (int i = cell1.Position.Y; i > 0; i--)
                {
                    if (!CheckDirection(gridData, cell1, out cell1, ref horizontalCount, PositionUtils.GetLeft))
                    {
                        break;
                    }
                }
            }

            if (up)
            {
                var cell1 = gridData.Cells[PositionUtils.GetCurrent(cell.Position, gridData.Size)];
                for (int i = cell1.Position.X; i > 0; i--)
                {
                    if (!CheckDirection(gridData, cell1, out cell1, ref verticalCount, PositionUtils.GetUp))
                    {
                        break;
                    }
                }
            }

            if (down)
            {
                var cell1 = gridData.Cells[PositionUtils.GetCurrent(cell.Position, gridData.Size)];
                for (int i = cell1.Position.X; i < gridData.Size.X - 1; i++)
                {
                    if (!CheckDirection(gridData, cell1, out cell1, ref verticalCount, PositionUtils.GetDown))
                    {
                        break;
                    }
                }
            }

            int count = 0;
            count += horizontalCount >= CellsConst.MinCountToMatch ? horizontalCount : 0;
            count += verticalCount >= CellsConst.MinCountToMatch ? verticalCount : 0;
            if (count >= CellsConst.MinNumberToGenerateSpecialCell)
            {
                SetSpecial(gridData, cell, count);
            }
        }

        private void SetSpecial(GridData gridData, Cell cell, int count)
        {
            SpecialType specialType;
            if (count == CellsConst.MinNumberToGenerateSpecialCell)
            {
                specialType = SpecialType.Bomb;
            }
            else
            {
                specialType = SpecialType.Color;
            }

            gridData.Cells[PositionUtils.GetCurrent(cell.Position, gridData.Size)] =
                new Cell(cell.Position, cell.Id, cell.Direction, specialType);
            gridData.Grid[cell.Position.Y] = (byte)(gridData.Grid[cell.Position.Y] ^ (1 << cell.Position.X));
        }

        private static bool CheckDirection(GridData gridData,
            Cell cell,
            out Cell resultCell,
            ref int count, Func<Vector2Byte, Vector2Byte, int> positionFunc)
        {
            resultCell = gridData.Cells[positionFunc.Invoke(cell.Position, gridData.Size)];
            if (resultCell.Id != cell.Id)
            {
                return false;
            }

            count++;
            return true;
        }
    }
}