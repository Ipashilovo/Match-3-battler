using LevelConstructor.Data;
using Match3Test.Core.Data;
using Match3Test.Core.FightCore;
using Match3Test.Core.FightCore.Data;
using Match3Test.Core.FightCore.Heroes;

namespace Match3Test.Core
{
    public class SimpleGame
    {
        private readonly AttackProvider _attackProvider;
        private readonly Vector2Byte _size;
        private readonly MapGenerator _mapGenerator;
        private readonly MapGravity _mapGravity;
        private readonly PossibleCellsChecker _possibleCellsChecker;
        private readonly FreeCellFiller _filler;
        private readonly MatchFinder _matchFinder;
        private readonly SpecialCellFinder _specialCellFinder;
        private readonly HeroFactory _heroFactory;
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemyLoop _enemyLoop;

        public SimpleGame(AttackProvider attackProvider, HeroFactory heroFactory, EnemyFactory enemyFactory)
        {
            _attackProvider = attackProvider;
            _heroFactory = heroFactory;
            _enemyFactory = enemyFactory;
            _enemyLoop = new EnemyLoop();
            _mapGenerator = new MapGenerator();
            _mapGravity = new MapGravity();
            _possibleCellsChecker = new PossibleCellsChecker();
            _filler = new FreeCellFiller();
            SpecialHandlersContainer specialHandlersContainer = new SpecialHandlersContainer();
            OneColorHandler oneColorHandler = new OneColorHandler(specialHandlersContainer);
            BombHandler bombHandler = new BombHandler(specialHandlersContainer);
            specialHandlersContainer.SetSpecial(new ISpecialCellHandler[]{bombHandler, oneColorHandler});
            _matchFinder = new MatchFinder(specialHandlersContainer);
            _size = new Vector2Byte(5, 7);
            _specialCellFinder = new SpecialCellFinder();
        }

        public SimpleGame(EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            _mapGenerator = new MapGenerator();
            _mapGravity = new MapGravity();
            _possibleCellsChecker = new PossibleCellsChecker();
            _filler = new FreeCellFiller();
            SpecialHandlersContainer specialHandlersContainer = new SpecialHandlersContainer();
            OneColorHandler oneColorHandler = new OneColorHandler(specialHandlersContainer);
            BombHandler bombHandler = new BombHandler(specialHandlersContainer);
            specialHandlersContainer.SetSpecial(new ISpecialCellHandler[]{bombHandler, oneColorHandler});
            _matchFinder = new MatchFinder(specialHandlersContainer);
            _size = new Vector2Byte(5, 7);
            _specialCellFinder = new SpecialCellFinder();
        }

        public Cell[] GetCellsNonFightSimulate(PlayerLevelData playerLevelData)
        {
            Random random = new Random(playerLevelData.Seed);
            Span<Cell> cells = stackalloc Cell[_size.X * _size.Y];
            Span<byte> grid = stackalloc byte[_size.Y];
            GridData gridData = new GridData()
            {
                Cells = cells,
                Grid = grid,
                Size = _size,
                Ids = 4
            };

            _mapGenerator.Generate(gridData, random);
            while (!_possibleCellsChecker.TryFindActionCellPosition(gridData.Cells, _size))
            {
                _mapGenerator.Generate(gridData, random);
            }
            foreach (var action in playerLevelData.PlayerActions)
            {
                if (_matchFinder.TryMove(gridData, action.MoveAction))
                {
                    GridUtils.ChangePosition( gridData.Cells, _size, action.MoveAction);
                    _specialCellFinder.FindSpecialOnMove(gridData, action.MoveAction);
                    _mapGravity.FindFreeCell(gridData);
                    _filler.SimpleFill(gridData, random);
                    while (_matchFinder.FindMatchInUpdatable(gridData))
                    {
                        _specialCellFinder.FindSpecialOnChain(gridData, random);
                        _mapGravity.FindFreeCell(gridData);
                        _filler.SimpleFill(gridData, random);
                    }
                }
                while (!_possibleCellsChecker.TryFindActionCellPosition(gridData.Cells, _size))
                {
                    _mapGenerator.Generate(gridData, random);
                }
            }

            return gridData.Cells.ToArray();

        }
        
        public void NonFightSimulate(PlayerLevelData playerLevelData)
        {
            Random random = new Random(playerLevelData.Seed);
            Span<Cell> cells = stackalloc Cell[_size.X * _size.Y];
            Span<byte> grid = stackalloc byte[_size.Y];
            GridData gridData = new GridData()
            {
                Cells = cells,
                Grid = grid,
                Size = _size,
                Ids = 4
            };
            
            _mapGenerator.Generate(gridData, random);
            while (!_possibleCellsChecker.TryFindActionCellPosition(cells, _size))
            {
                _mapGenerator.Generate(gridData, random);
            }
            foreach (var action in playerLevelData.PlayerActions)
            {
                if (_matchFinder.TryMove(gridData, action.MoveAction))
                {
                    GridUtils.ChangePosition( gridData.Cells, _size, action.MoveAction);
                    _specialCellFinder.FindSpecialOnMove(gridData, action.MoveAction);
                    _mapGravity.FindFreeCell(gridData);
                    _filler.SimpleFill(gridData, random);
                    while (_matchFinder.FindMatchInUpdatable(gridData))
                    {
                        _specialCellFinder.FindSpecialOnChain(gridData, random);
                        _mapGravity.FindFreeCell(gridData);
                        _filler.SimpleFill(gridData, random);
                    }
                }
                while (!_possibleCellsChecker.TryFindActionCellPosition(cells, _size))
                {
                    _mapGenerator.Generate(gridData, random);
                }
            }
        }
        
        public (Cell[] cells, Enemy[] enemies) GetCellsAfterSimulate(LevelData levelData, PlayerLevelData playerLevelData)
        {
            Random random = new Random(playerLevelData.Seed);
            Span<Cell> cells = stackalloc Cell[_size.X * _size.Y];
            Span<byte> grid = stackalloc byte[_size.Y];
            GridData gridData = new GridData()
            {
                Cells = cells,
                Grid = grid,
                Size = _size,
                Ids = 4
            };
            FightData fightData = new FightData()
            {
                Enemies = stackalloc Enemy[5],
                EnemiesGridSize = gridData.Size.Y,
                EnemiesOrder = stackalloc int[3],
                WaveNumber = 0,
                Heroes = stackalloc Hero[0],
                AttackById = new Span<int>(playerLevelData.HeroesAttackById)
                
            };
            
            _mapGenerator.Generate(gridData, random);
            _enemyFactory.CreateEnemies(fightData, levelData.Stages[fightData.WaveNumber++]);
            while (!_possibleCellsChecker.TryFindActionCellPosition(cells, _size))
            {
                _mapGenerator.Generate(gridData, random);
            }
            int scale;
            foreach (var action in playerLevelData.PlayerActions)
            {
                scale = 0;
                if (_matchFinder.TryMove(gridData, action.MoveAction))
                {
                    GridUtils.ChangePosition( gridData.Cells, _size, action.MoveAction);
                    _attackProvider.TryDealDamageToEnemyByGrid(gridData, random, fightData, scale++);
                    _enemyLoop.SetEnemyDead(fightData);
                    _specialCellFinder.FindSpecialOnMove(gridData, action.MoveAction);
                    _mapGravity.FindFreeCell(gridData);
                    _filler.SimpleFill(gridData, random);
                    while (_matchFinder.FindMatchInUpdatable(gridData))
                    {
                        _attackProvider.TryDealDamageToEnemyByGrid(gridData, random, fightData, scale++);
                        _enemyLoop.SetEnemyDead(fightData);
                        _specialCellFinder.FindSpecialOnChain(gridData, random);
                        _mapGravity.FindFreeCell(gridData);
                        _filler.SimpleFill(gridData, random);
                    }
                }

                bool isAllDead = true;
                foreach (var enemy in fightData.Enemies)
                {
                    if (!enemy.IsDead)
                    {
                        isAllDead = false;
                        break;
                    }
                }

                if (isAllDead)
                {
                    if (fightData.WaveNumber >= levelData.Stages.Count)
                    {
                        return (gridData.Cells.ToArray(), fightData.Enemies.ToArray());
                    }
                    else
                    {
                        _enemyFactory.CreateEnemies(fightData, levelData.Stages[fightData.WaveNumber++]);
                    }
                }
            }

            return (gridData.Cells.ToArray(), fightData.Enemies.ToArray());
        }
        
        public void Simulate(LevelData levelData, PlayerLevelData playerLevelData)
        {
            Random random = new Random(playerLevelData.Seed);
            Span<Cell> cells = stackalloc Cell[_size.X * _size.Y];
            Span<byte> grid = stackalloc byte[_size.Y];
            GridData gridData = new GridData()
            {
                Cells = cells,
                Grid = grid,
                Size = _size,
                Ids = 4
            };
            FightData fightData = new FightData()
            {
                Enemies = stackalloc Enemy[5],
                EnemiesGridSize = gridData.Size.Y,
                EnemiesOrder = stackalloc int[3],
                WaveNumber = 0,
                Heroes = stackalloc Hero[0],
                AttackById = new Span<int>(playerLevelData.HeroesAttackById)
                
            };
            
            _mapGenerator.Generate(gridData, random);
            _enemyFactory.CreateEnemies(fightData, levelData.Stages[fightData.WaveNumber++]);
            while (!_possibleCellsChecker.TryFindActionCellPosition(cells, _size))
            {
                _mapGenerator.Generate(gridData, random);
            }
            int scale;
            foreach (var action in playerLevelData.PlayerActions)
            {
                scale = 0;
                if (_matchFinder.TryMove(gridData, action.MoveAction))
                {
                    GridUtils.ChangePosition(gridData.Cells, _size, action.MoveAction);
                    _attackProvider.TryDealDamageToEnemyByGrid(gridData, random, fightData, scale++);
                    _enemyLoop.SetEnemyDead(fightData);
                    _specialCellFinder.FindSpecialOnMove(gridData, action.MoveAction);
                    _mapGravity.FindFreeCell(gridData);
                    _filler.SimpleFill(gridData, random);
                    while (_matchFinder.FindMatchInUpdatable(gridData))
                    {
                        _attackProvider.TryDealDamageToEnemyByGrid(gridData, random, fightData, scale++);
                        _enemyLoop.SetEnemyDead(fightData);
                        _specialCellFinder.FindSpecialOnChain(gridData, random);
                        _mapGravity.FindFreeCell(gridData);
                        _filler.SimpleFill(gridData, random);
                    }
                }

                bool isAllDead = true;
                foreach (var enemy in fightData.Enemies)
                {
                    if (!enemy.IsDead)
                    {
                        isAllDead = false;
                        break;
                    }
                }

                if (isAllDead)
                {
                    if (fightData.WaveNumber >= levelData.Stages.Count)
                    {
                        return;
                    }
                    else
                    {
                        _enemyFactory.CreateEnemies(fightData, levelData.Stages[fightData.WaveNumber++]);
                    }
                }
            }
        }
    }
}