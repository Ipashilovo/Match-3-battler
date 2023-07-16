namespace Match3Test.Core.FightCore.Data
{
    public struct EnemyUnitData  
    {
        public int AttackSpeed { get; }
        public int ColorId { get; set; }
        public bool HaveMana { get;  }
        public int Defence { get;  }

        public EnemyUnitData(int attackSpeed, int colorId, bool haveMana, int defence)
        {
            AttackSpeed = attackSpeed;
            ColorId = colorId;
            HaveMana = haveMana;
            Defence = defence;
        }
    }
}