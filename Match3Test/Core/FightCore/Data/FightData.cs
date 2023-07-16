using Match3Test.Core.FightCore.Heroes;

namespace Match3Test.Core.FightCore.Data
{
    public ref struct FightData
    {
        public Span<Enemy> Enemies;
        public Span<Hero> Heroes;
        public byte EnemiesGridSize;
        public Span<int> AttackById;
        public Span<int> EnemiesOrder;
        public int WaveNumber;
    }
}