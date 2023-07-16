namespace Match3Test.Core
{
    public class OneColorHandler : ISpecialCellHandler
    {
        private readonly ISpecialHandlerContainer _specialHandlersContainer;

        public SpecialType Type => SpecialType.Color;

        public OneColorHandler(ISpecialHandlerContainer specialHandlersContainer)
        {
            _specialHandlersContainer = specialHandlersContainer;
        }

        public void Handle(GridData gridData, Cell cell)
        {
            int id = cell.Id;
            for (int i = 0; i < gridData.Cells.Length; i++)
            {
                var nextGrid = gridData.Cells[i];
                if (nextGrid.Id == id)
                {
                    //Предотвращение рекурсии спешл фишек
                    if ((gridData.Grid[nextGrid.Position.Y] & (1 << nextGrid.Position.X)) == 0)
                    {
                        if (nextGrid.SpecialType != SpecialType.None)
                        {
                            if (nextGrid.SpecialType != SpecialType.Color)
                            {
                                _specialHandlersContainer.DoSpecial(nextGrid, gridData);
                            }
                        }
                    }

                    gridData.Grid[nextGrid.Position.Y] |= (byte)(1 << nextGrid.Position.X);
                }
            }
        }
    }
}