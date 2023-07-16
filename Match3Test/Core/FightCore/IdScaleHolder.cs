namespace Match3Test.Core.FightCore
{
    public class IdScaleHolder
    {
        private ScaleData[] _scaleData;
        
        public static IdScaleHolder Default
        {
            get
            {
                return new IdScaleHolder(new[]
                {
                    new ScaleData(1, 1.5, 2, 0.5),
                    new ScaleData(2, 1.5, 0, 0.5),
                    new ScaleData(0, 1.5, 1, 0.5),
                    new ScaleData(4, 1.5, -1, 1),
                    new ScaleData(3, 1.5, -1, 1),
                });
            }
        }
        
        public IdScaleHolder(ScaleData[] scaleData)
        {
            _scaleData = scaleData;
        }
        
        public double GetScale(int targetId, int Id)
        {
            var data = _scaleData[targetId];
            if (data.PositiveId == Id)
            {
                return data.PositiveScale;
            }

            if (data.NegativeId == Id)
            {
                return data.NegativeScale;
            }

            return 1;
        }
    }

    public class ScaleData
    {
        public double PositiveScale { get; }
        public double NegativeScale { get; }
        public int PositiveId { get; }
        public int NegativeId { get; }
        
        public ScaleData(int positiveId, double positiveScale, int negativeId, double negativeScale)
        {
            PositiveId = positiveId;
            PositiveScale = positiveScale;
            NegativeId = negativeId;
            NegativeScale = negativeScale;
        }
    }
}