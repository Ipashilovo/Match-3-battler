using LevelConstructor.Data;
using Match3Test.Core.Data;

namespace Match3Test.Core
{
    public class LevelFactory
    {
        private LevelData[] _levelDatas;
        
        public LevelData GetLevel(int levelId)
        {
            return _levelDatas[levelId];
        }
    }
}