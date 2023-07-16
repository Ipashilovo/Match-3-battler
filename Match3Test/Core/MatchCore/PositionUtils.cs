using System.Runtime.CompilerServices;

namespace Match3Test.Core
{
    public static class PositionUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetUp(Vector2Byte position, Vector2Byte size)
        {
            return (position.X - 1) * size.Y + position.Y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetCurrent(Vector2Byte position, Vector2Byte size)
        {
            return position.X * size.Y + position.Y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetUp(int x, int y, Vector2Byte size)
        {
            return (x - 1) * size.Y + y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetCurrent(int x, int y, Vector2Byte size)
        {
            return x * size.Y + y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetDown(int x, int y, Vector2Byte size)
        {
            return (x + 1) * size.Y + y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetRight(int x, int y, Vector2Byte size)
        {
            return x * size.Y + y + 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetLeft(int x, int y, Vector2Byte size)
        {
            return x * size.Y + y - 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetDown(Vector2Byte position, Vector2Byte size)
        {
            return (position.X + 1) * size.Y + position.Y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetRight(Vector2Byte position, Vector2Byte size)
        {
            return position.X * size.Y + position.Y + 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static int GetLeft(Vector2Byte position, Vector2Byte size)
        {
            return position.X * size.Y + position.Y - 1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining & MethodImplOptions.AggressiveOptimization)]
        public static Direction GetNormalDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Direction.Up & Direction.Right;
                case Direction.Left:
                    return Direction.Up & Direction.Right;
                case Direction.Up:
                    return Direction.Right & Direction.Left;
                case Direction.Down:
                    return Direction.Right & Direction.Left;
                default: throw new Exception("Direction is none");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DirectionToBool(this Direction direction, out bool up, out bool down, out bool right,
            out bool left)
        {
            up = direction.HasFlag(Direction.Up);
            down = direction.HasFlag(Direction.Down);
            left = direction.HasFlag(Direction.Left);
            right = direction.HasFlag(Direction.Right);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DirectionToBoolHorizontal(this Direction direction, out bool right,
            out bool left)
        {
            left = direction.HasFlag(Direction.Left);
            right = direction.HasFlag(Direction.Right);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DirectionToBoolVertical(this Direction direction, out bool up, out bool down)
        {
            up = direction.HasFlag(Direction.Up);
            down = direction.HasFlag(Direction.Down);
        } 
            
        public static Direction Revert(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                default: throw new Exception("Direction is none");
            }
        }
    }
}