using Match3Test.Core.FightCore.Data;

namespace Match3Test.Core.FightCore
{
    public class EnemyLoop
    {
        public void UpdateEnemys(ref Span<Enemy> enemys, ref Span<int> damage)
        {
            for (int i = 0; i < enemys.Length; i++)
            {
                ref var enemy = ref enemys[i];
                if (enemy.IsDead)
                {
                    continue;
                }
                enemy.AttackCounter++;
                if (enemy.AttackCounter >= enemy.EnemyUnitData.AttackSpeed)
                {
                    
                }
            }
        }

        public void SetEnemyDead(FightData fightData)
        {
            for (int i = 0; i < fightData.Enemies.Length; i++)
            {
                ref var enemy = ref fightData.Enemies[i];
                if (enemy.Health <= 0)
                {
                    fightData.EnemiesOrder[enemy.Position.X] &= ~(fightData.EnemiesOrder[enemy.Position.X] & (1 << i));
                    enemy.IsDead = true;
                }
            }
        }
    }
}