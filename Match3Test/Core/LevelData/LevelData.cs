
using System.Collections.Generic;

namespace LevelConstructor.Data
{
    public class LevelData
    {
        public string Id;
        public List<StageData> Stages;
    }

    public class StageData
    {
        public List<UnitStageData> UnitStagesData;
    }

    public class UnitStageData
    {
        public string UnitId;
    }
}