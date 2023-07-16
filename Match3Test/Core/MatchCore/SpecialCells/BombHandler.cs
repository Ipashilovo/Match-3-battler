namespace Match3Test.Core
{
    public class BombHandler : ISpecialCellHandler
    {
        private ISpecialHandlerContainer _specialHandlerContainer;

        public SpecialType Type => SpecialType.Bomb;

        public BombHandler(ISpecialHandlerContainer specialHandlerContainer)
        {
            _specialHandlerContainer = specialHandlerContainer;
        }

        public void Handle(GridData gridData, Cell cell)
        {
            cell.Direction.DirectionToBool(out var up, out var dowm, out var right, out var left);
            gridData.Grid[cell.Position.Y] = (byte)(gridData.Grid[cell.Position.Y] | (1 << cell.Position.X));
            if (dowm)
            {
                HandleNeighborCell(gridData, cell, PositionUtils.GetDown);
            }

            if (up)
            {
                HandleNeighborCell(gridData, cell, PositionUtils.GetUp);
            }

            if (right)
            {
                HandleNeighborCell(gridData, cell, PositionUtils.GetRight);
            }

            if (left)
            {
                HandleNeighborCell(gridData, cell, PositionUtils.GetLeft);
            }
        }

        private void HandleNeighborCell(GridData gridData, Cell cell, Func<Vector2Byte, Vector2Byte, int> directionFunc)
        {
            var nextCell = gridData.Cells[directionFunc.Invoke(cell.Position, gridData.Size)];
            //Предотвращение рекурсии спешл фишек
            if ((gridData.Grid[nextCell.Position.Y] & (1 << nextCell.Position.X)) == 0)
            {
                if (nextCell.SpecialType != SpecialType.None)
                {
                    _specialHandlerContainer.DoSpecial(nextCell, gridData);
                }
            }

            gridData.Grid[nextCell.Position.Y] =
                (byte)(gridData.Grid[nextCell.Position.Y] | (1 << nextCell.Position.X));
        }
    }
}