namespace Match3Test.Core
{
    public class SpecialHandlersContainer : ISpecialHandlerContainer
    {
        private Dictionary<SpecialType, ISpecialCellHandler> _specialCellHandlers;

        public void SetSpecial(IEnumerable<ISpecialCellHandler> specialCellHandler)
        {
            _specialCellHandlers = specialCellHandler.ToDictionary(k => k.Type, v => v);
        }

        public void DoSpecial(Cell cell, GridData gridData)
        {
            _specialCellHandlers[cell.SpecialType].Handle(gridData, cell);
        }
    }

    public interface ISpecialHandlerContainer
    {
        public void DoSpecial(Cell cell, GridData gridData);
    }
}