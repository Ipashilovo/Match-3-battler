using Newtonsoft.Json;

namespace Match3Test.Core
{
    [Flags]
    public enum Direction : byte
    {
        Up = 1 << 0,
        Down = 1 << 1,
        Right = 1 << 2,
        Left = 1 << 3,
    }
    
    public struct Cell
    {
        public int Id { get; }
        public Vector2Byte Position { get; }

        public Direction Direction = 0;
        public SpecialType SpecialType { get; }


        [JsonConstructor]
        public Cell(Vector2Byte position, int id, Direction direction = 0, SpecialType specialType = SpecialType.None)
        {
            Direction = direction;
            Position = position;
            Id = id;
            SpecialType = specialType;
        }
    }

    public readonly struct Vector2Byte : IEquatable<Vector2Byte>
    {
        public readonly byte X;
        public readonly byte Y;


        [JsonConstructor]
        public Vector2Byte(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Vector2Byte a, Vector2Byte b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector2Byte a, Vector2Byte b)
        {
            return !(a == b);
        }

        public bool Equals(Vector2Byte other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2Byte other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

}