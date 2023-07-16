using LevelConstructor.Data;
using Match3Test.Core.Data;
using Match3Test.Core.FightCore.Data;

namespace Match3Test.Core
{
    public class MoveData
    {
        public MoveAction[] MoveActions;
        public int Seed;
        public LevelData LevelData;
        public int[] AttackById;
        public CalculateAttackData CalculateAttackData;
        public double[] ScaleFactors;
        public UnitLevelData[] UnitLevelData;
    }
}