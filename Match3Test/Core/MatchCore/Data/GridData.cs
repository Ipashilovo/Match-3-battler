namespace Match3Test.Core
{
    public ref struct GridData
    {
        public Span<byte> Grid; 
        public Span<Cell> Cells;
        public Vector2Byte Size;
        public int Ids;
    }
}