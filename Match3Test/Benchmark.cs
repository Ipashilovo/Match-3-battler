using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using Match3Test.Core;
using Match3Test.Core.Data;
using Match3Test.Core.FightCore;
using Match3Test.Core.FightCore.Data;
using Match3Test.Core.FightCore.Heroes;
using Newtonsoft.Json;

namespace Match3Test
{
    [Config(typeof(AntiVirusFriendlyConfig))]
    [MemoryDiagnoser]
    [ReturnValueValidator(failOnError: true)]
    public class Benchmark
    {

        private string LoadBenchmark()
        {
            return File.ReadAllText("D:/UnityProjects/ep-playground/Match3Test/TestProject1/CustomTestResults/Benchmark.json");
        }
        
        [Benchmark]
        public void Load()
        {
            var json = LoadBenchmark();
            var moveData = JsonConvert.DeserializeObject<MoveData>(json);
            PlayerLevelData playerLevelData = new PlayerLevelData()
            {
                PlayerActions = moveData.MoveActions.Select(v => new PlayerAction() { MoveAction = v }).ToArray(),
                Seed = moveData.Seed
            };
        }
        
        [Benchmark]
        public void Simulate()
        {
            var json = LoadBenchmark();
            //TODO вставить конкретную levelData
            var moveData = JsonConvert.DeserializeObject<MoveData>(json);
            SimpleGame simpleGame = 
                new SimpleGame(
                    new AttackProvider(IdScaleHolder.Default, moveData.CalculateAttackData, moveData.ScaleFactors),
                    new HeroFactory(),
                    new EnemyFactory(moveData.UnitLevelData));
            PlayerLevelData playerLevelData = new PlayerLevelData()
            {
                PlayerActions = moveData.MoveActions.Select(v => new PlayerAction() { MoveAction = v }).ToArray(),
                Seed = moveData.Seed,
                HeroesAttackById = moveData.AttackById
            };
            simpleGame.Simulate(moveData.LevelData, playerLevelData);
        }
        
        private Cell GetNeighbor(Cell cell, Cell[,] cells, Direction direction)
        {
            if (direction.HasFlag(Direction.Down))
            {
                return cells[cell.Position.X + 1, cell.Position.Y];
            }
            if (direction.HasFlag(Direction.Up))
            {
                return cells[cell.Position.X - 1, cell.Position.Y ];
            }
            if (direction.HasFlag(Direction.Right))
            {
                return cells[cell.Position.X, cell.Position.Y + 1];
            }
            if (direction.HasFlag(Direction.Left))
            {
                return cells[cell.Position.X , cell.Position.Y -1];
            }

            return default;
        }

        private struct GridCell
        {
            public Direction Direction;
            public Cell Cell;

            public GridCell(Direction direction, Cell cell)
            {
                Direction = direction;
                Cell = cell;
            }
        }
    }
    

    public class AntiVirusFriendlyConfig : ManualConfig
    {
        public AntiVirusFriendlyConfig()
        {
            AddJob(Job.MediumRun
                .WithToolchain(InProcessNoEmitToolchain.Instance));
        }
    }
}