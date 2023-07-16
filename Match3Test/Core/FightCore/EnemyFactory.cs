using LevelConstructor.Data;
using Match3Test.Core.FightCore.Data;

namespace Match3Test.Core.FightCore
{
    public class EnemyFactory
    {
        private EnemyGridPattern _defaultPattern;
        private int _enemyInstanceId = Int32.MinValue;
        private readonly Dictionary<string, UnitLevelData> _unitLevelDatas;

        public EnemyFactory(UnitLevelData[] unitLevelDatas)
        {
            _unitLevelDatas = unitLevelDatas.ToDictionary(k => k.Id, v => v);
            _defaultPattern = new EnemyGridPattern()
            {
                Positions = new()
                {
                    new EnemyPosition(6, new Vector2Byte(0, 2)),
                    new EnemyPosition(4, new Vector2Byte(1, 1)),
                    new EnemyPosition(4, new Vector2Byte(1, 4)),
                    new EnemyPosition(4, new Vector2Byte(2, 0)),
                    new EnemyPosition(4, new Vector2Byte(2, 5))
                }
            }; 
        }
        
        public void CreateEnemies(FightData fightData, StageData stageData)
        {
            for (int i = 0; i < fightData.EnemiesOrder.Length; i++)
            {
                fightData.EnemiesOrder[i] = 0;
            }

            for (int i = 0; i < stageData.UnitStagesData.Count; i++)
            {
                CreateEnemy(fightData, stageData.UnitStagesData[i], i);
            }

            for (int i = stageData.UnitStagesData.Count; i < fightData.Enemies.Length; i++)
            {
                CreateEmpty(fightData, i);
            }
        }

        private void CreateEmpty(FightData fightData, int i)
        {
            fightData.Enemies[i] = Enemy.Empty;
        }

        private void CreateEnemy(FightData fightData, UnitStageData unit, int index)
        {
            var position = _defaultPattern.Positions[index];
            var data = _unitLevelDatas[unit.UnitId];
            EnemyUnitData enemyUnitData = new EnemyUnitData(data.ActionWaiting, data.CellId,
                data.Mana > 0, data.Defence);
            fightData.Enemies[index] = new Enemy()
            {
                EnemyUnitData = enemyUnitData,
                AttackCounter = 0,
                DebuffScale = 1,
                Defence = enemyUnitData.Defence,
                Health = data.Health,
                IsDead = false,
                Mana = 0,
                Width = position.Width/2,
                Position = position.Position
            };
            fightData.EnemiesOrder[position.Position.X] |= (1 << index);
        }
    }

    public class EnemyGridPattern
    {
        public string PatternId;
        public string PatternType;
        public List<EnemyPosition> Positions;
    }

    public class EnemyPosition
    {
        public Vector2Byte Position;
        public int Width;

        public EnemyPosition()
        {
            
        }
        public EnemyPosition(int width, Vector2Byte position)
        {
            Width = width;
            Position = position;
        }
    }
}