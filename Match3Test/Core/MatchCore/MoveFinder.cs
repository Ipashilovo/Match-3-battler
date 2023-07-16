namespace Match3Test.Core
{
    public class MoveFinder
    {
        public MoveAction Find(GridData gridData, int action, MatchFinder matchFinder)
        {
            var cell = gridData.Cells[action];
            MoveAction result = default;
            if (cell.Direction.HasFlag(Direction.Down))
            {
                var newCell = gridData.Cells[PositionUtils.GetDown(cell.Position, gridData.Size)];
                if (SetMoveAction(gridData, matchFinder, cell, newCell, ref result))
                {
                    return result;
                }
            }
            if (cell.Direction.HasFlag(Direction.Up))
            {
                var newCell = gridData.Cells[PositionUtils.GetUp(cell.Position, gridData.Size)];
                if (SetMoveAction(gridData, matchFinder, cell, newCell, ref result))
                {
                    return result;
                }
            }
            if (cell.Direction.HasFlag(Direction.Right))
            {
                var newCell = gridData.Cells[PositionUtils.GetRight(cell.Position, gridData.Size)];
                if (SetMoveAction(gridData, matchFinder, cell, newCell, ref result))
                {
                    return result;
                }
            }
            if (cell.Direction.HasFlag(Direction.Left))
            {
                var newCell = gridData.Cells[PositionUtils.GetLeft(cell.Position, gridData.Size)];
                if (SetMoveAction(gridData, matchFinder, cell, newCell, ref result))
                {
                    return result;
                }
            }

            return default;
        }

        private bool SetMoveAction(GridData gridData, MatchFinder matchFinder,
            Cell cell, Cell newCell, ref MoveAction result)
        {
            result.StartPosition = cell.Position;
            result.EndPosition = newCell.Position;
            if (matchFinder.TryMoveNonSpecial(gridData, result))
            {
                return true;
            }
            return false;
        }
    }
}