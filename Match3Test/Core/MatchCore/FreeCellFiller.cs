namespace Match3Test.Core
{
    public class FreeCellFiller
    {
        public void SimpleFill(GridData gridData, Random random)
        {
            int minY = gridData.Grid.GetMin();
            int maxY = gridData.Grid.GetMax();
            for (int i = minY; i <= maxY; i++)
            {
                for (int j = gridData.Size.X - 1; j >= 0; j--)
                {
                    if ((gridData.Grid[i] & (1 << j)) != 0)
                    {
                        var position = new Vector2Byte((byte)j, (byte)i);
                        int cellNumber = PositionUtils.GetCurrent(position, gridData.Size);
                        var cell = gridData.Cells[cellNumber];
                        gridData.Cells[cellNumber] = new Cell(position, random.Next(gridData.Ids), cell.Direction);
                    }
                }
            }
        }
    }
}