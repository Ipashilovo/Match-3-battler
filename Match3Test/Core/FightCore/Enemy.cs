using Match3Test.Core.FightCore.Data;

namespace Match3Test.Core.FightCore
{
    public struct Enemy
    {
        public EnemyUnitData EnemyUnitData;
        public int Width;
        public int Mana;
        public int AttackCounter;
        public int Health;
        public Vector2Byte Position;
        public int DebuffScale;
        public bool IsDead;
        public int Defence;

        public static Enemy Empty => new()
        {
            IsDead = true
        };
    }
}