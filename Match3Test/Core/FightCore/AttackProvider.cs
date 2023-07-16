using System;
using Match3Test.Core.FightCore.Data;
using Match3Test.Core.FightCore.Heroes;

namespace Match3Test.Core.FightCore
{
    public class AttackProvider
    {
        private IdScaleHolder _idScaleHolder;
        private CalculateAttackData _calculateAttackData;
        private double[] _scaleFactors;

        public AttackProvider(IdScaleHolder idScaleHolder, CalculateAttackData calculateAttackData, double[] scaleFactors)
        {
            _idScaleHolder = idScaleHolder;
            _calculateAttackData = calculateAttackData;
            _scaleFactors = scaleFactors;
        }

        public void TryDealDamageToEnemyByGrid(GridData gridData, Random random, FightData fightData, int scale = 0)
        {
            Span<byte> grid = stackalloc byte[gridData.Grid.Length];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = gridData.Grid[i];
            }
            double scaleFactor = _scaleFactors[Math.Clamp(scale, 0, _scaleFactors.Length - 1)];
            for (int i = 0; i < fightData.EnemiesOrder.Length; i++)
            {
                if (fightData.EnemiesOrder[i] != 0)
                {
                    int line = fightData.EnemiesOrder[i];
                    HandleLine(line, gridData, scaleFactor , fightData, random, grid);
                }
            }

            var min = grid.GetMin();
            var max = grid.GetMax();
            byte missManaIncrement = 2;
            for (int i = min; i <= max; i++)
            {
                for (int j = 0; j < gridData.Size.X; j++)
                {
                    if ((grid[i] & (1 << j)) == 0)
                    {
                        continue;
                    }
                    var cell = gridData.Cells[PositionUtils.GetCurrent(j, i, gridData.Size)];
                    AddMana(fightData, ManaSpeed.Miss, cell.Id);
                }
            }
        }

        private void HandleLine(int line, GridData gridData, double scale, FightData fightData, Random random,
            Span<byte> grid)
        {
            for (int j = 0; j < fightData.EnemiesGridSize; j++)
            {
                if ((line & (1 << j)) != 0)
                {
                    line &= ~(line & (1 << j));
                    ref var enemy = ref fightData.Enemies[j];
                    DealDamageToEnemy(gridData, scale, fightData, random, ref grid, ref enemy);
                    

                    
                    int lastPosition = enemy.Position.Y + (enemy.Width - 1);

                    if (grid[lastPosition] != 0)
                    {
                        Span<int> damagesById = stackalloc int[gridData.Ids + 1];
                        LineDamage lineDamage = new LineDamage(damagesById);
                        for (int i = 0; i < gridData.Size.X; i++)
                        {
                            if ((grid[lastPosition] & (1 << i)) != 0)
                            {
                                var id = gridData.Cells[PositionUtils.GetCurrent(i, lastPosition, gridData.Size)].Id;
                                lineDamage.Damage[id] += CalculateDamage(fightData.AttackById[id], enemy.Defence, random);
                            }
                        }

                        grid[lastPosition] = 0;
                        
                        if (line != 0)
                        {
                            if (TryFindSecondTargetToDealDamage(fightData.EnemiesGridSize, line, j, fightData.Enemies))
                            {
                                for (int l = 0; l < lineDamage.Damage.Length; l++)
                                {
                                    lineDamage.Damage[l] /= 2;
                                }
                                DealLineDamageToEnemy(ref fightData.Enemies[j], lineDamage, scale);
                            }
                        }

                        DealLineDamageToEnemy(ref enemy, lineDamage, scale);
                    }
                }
            }
        }

        private void DealDamageToEnemy(GridData gridData, double scale, FightData fightData, Random random, ref Span<byte> grid,
            ref Enemy enemy)
        {
            for (int k = enemy.Position.Y;
                 k < (enemy.Position.Y + (enemy.Width - 1));
                 k++)
            {
                if (grid[k] != 0)
                {
                    Span<int> damagesById = stackalloc int[gridData.Ids + 1];
                    LineDamage lineDamage = new LineDamage(damagesById);
                    lineDamage = GetLineDamage(gridData, fightData, random, grid, k, lineDamage, enemy);
                    grid[k] = 0;
                    DealLineDamageToEnemy(ref enemy, lineDamage, scale);
                }
            }
        }

        private LineDamage GetLineDamage(GridData gridData, FightData fightData, Random random, Span<byte> grid, int k,
            LineDamage lineDamage, Enemy enemy)
        {
            for (int i = 0; i < gridData.Size.X; i++)
            {
                if ((grid[k] & (1 << i)) != 0)
                {
                    var id = gridData.Cells[PositionUtils.GetCurrent(i, k, gridData.Size)].Id;
                    lineDamage.Damage[id] += CalculateDamage(fightData.AttackById[id], enemy.Defence, random);
                    AddMana(fightData, ManaSpeed.Hit, id);
                }
            }

            return lineDamage;
        }


        private int CalculateDamage(int attack, int defence, Random random)
        {
            StaticCounters.Counter++;
            var result = Math.Pow(GetExp(random) * attack / defence, _calculateAttackData.ResultDegree) *_calculateAttackData.Base;
            return (int)Math.Floor(result);
        }

        private void AddMana(FightData fightData, ManaSpeed manaSpeed, int cellId)
        {
            byte addValue = (byte)manaSpeed;
            for (int k = 0; k < fightData.Heroes.Length; k++)
            {
                if (fightData.Heroes[k].ColorId == cellId)
                {
                    fightData.Heroes[k].CurrentMana += addValue;
                }
            }
        }

        private double GetExp(Random random)
        {
            double value = _calculateAttackData.MaxExpDegree - _calculateAttackData.MinExpDegree;
            var randomValue = random.NextDouble();
            randomValue %= value;
            return Math.Pow(Math.E, _calculateAttackData.MinExpDegree + randomValue);
        }

        private bool TryFindSecondTargetToDealDamage(byte enemyGridSize, int line,
            int startIndex, Span<Enemy> enemies)
        {
            if ((line & (1 << startIndex)) != 0)
            {
                var nextEnemy =  enemies[startIndex];
                if (nextEnemy.Position.Y <= startIndex)
                {
                    return true;
                }
            }
            return false;
        }

        private void DealLineDamageToEnemy(ref Enemy enemy, LineDamage lineDamage, double comboScale)
        {
            var id = enemy.EnemyUnitData.ColorId;
            for (int i = 0; i < lineDamage.Damage.Length; i++)
            {
                if (lineDamage.Damage[i] == 0)
                {
                    continue;
                }
                
                var scale = _idScaleHolder.GetScale(id, i);
                int debuffScale = enemy.DebuffScale;
                int damage = (int)Math.Floor(lineDamage.Damage[i] * scale * debuffScale * comboScale);
                if (enemy.EnemyUnitData.HaveMana)
                {
                    DealDamageWithMana(ref enemy, damage);
                }
                else
                {
                    enemy.Health -= damage;
                }
            }
        }

        private void DealDamageWithMana(ref Enemy enemy, int damage)
        {
            //TODO add mana handle
        }

        private ref struct LineDamage
        {
            public Span<int> Damage;

            public LineDamage(Span<int> damage)
            {
                Damage = damage;
            }
        }
    }
}