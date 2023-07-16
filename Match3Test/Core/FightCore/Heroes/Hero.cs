namespace Match3Test.Core.FightCore.Heroes
{
    public struct Hero
    {
        public int Attack;
        public int Defence;
        public SupportUnit SupportUnit;
        public int Health;
        public int ColorId;
        public ManaSpeed ManaSpeed;
        public ManaBuff ManaBuff;
        public int TargetMana;
        public int CurrentMana;
        public int CriticalChance;
        public BuffEffect AttackEffect;
        public BuffEffect DefeceEffect;
    }

    public readonly struct ManaBuff
    {
        public readonly int BuffPower;

        public static ManaBuff Default => new ManaBuff(0);

        public ManaBuff(int buffPower)
        {
            BuffPower = buffPower;
        }
    }

    public struct BuffEffect
    {
    }

    public enum ManaSpeed : byte
    {
        Hit = 1,
        Miss = 2
    }

    public struct SupportUnit
    {
    }
}