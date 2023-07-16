using Match3Test.Core;
using Match3Test.Core.Data;
using Match3Test.Core.FightCore;
using Match3Test.Core.FightCore.Heroes;
using Newtonsoft.Json;

namespace Match3Test.Tests
{
    public class UnityToConsoleTest
    {
        [Test]
        public void UnityToConsole()
        {
            var files = Directory.GetFiles("D:/UnityProjects/ep-playground/Match3UnityTest/Assets/Matches");
            foreach (var file in files)
            {
                StaticCounters.Counter = 0;
                if (file.Contains(".meta"))
                {
                    continue;
                }
                var json = File.ReadAllText(file);
                var moveData = JsonConvert.DeserializeObject<MoveDataCheck>(json);
                PlayerLevelData playerLevelData = new PlayerLevelData()
                {
                    PlayerActions = moveData.MoveData.MoveActions.Select(v => new PlayerAction() { MoveAction = v }).ToArray(),
                    Seed = moveData.MoveData.Seed,
                    HeroesAttackById = moveData.MoveData.AttackById
                };
                SimpleGame simpleGame = 
                    new SimpleGame(
                        new AttackProvider(IdScaleHolder.Default, moveData.MoveData.CalculateAttackData, moveData.MoveData.ScaleFactors),
                        new HeroFactory(),
                        new EnemyFactory(moveData.MoveData.UnitLevelData));
                //TODO вставить конкретную levelData
                var result = simpleGame.GetCellsAfterSimulate(moveData.MoveData.LevelData, playerLevelData);
                // var cells = simpleGame.GetCellsNonFightSimulate(playerLevelData);
                Console.WriteLine(StaticCounters.Counter);
                for (int i = 0; i < result.cells.Length; i++)
                {
                    Assert.IsTrue(result.cells[i].Id == moveData.EndCells[i].Id);
                    Assert.IsTrue(result.cells[i].Position == moveData.EndCells[i].Position);
                    Assert.IsTrue(result.cells[i].Direction == moveData.EndCells[i].Direction);
                }
            }
        }
        
        private class MoveDataCheck
        {
            public MoveData MoveData;
            public Cell[] EndCells;
        }
    }
}