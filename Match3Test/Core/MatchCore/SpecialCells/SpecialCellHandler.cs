namespace Match3Test.Core
{
    public interface ISpecialCellHandler
    {
        public SpecialType Type { get; }
        public void Handle(GridData gridData, Cell cell);
    }
}