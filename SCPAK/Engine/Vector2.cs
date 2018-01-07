using System;
namespace Engine
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public static readonly Vector2 Zero = new Vector2(0f);

        public static readonly Vector2 One = new Vector2(1f);

        public static readonly Vector2 UnitX = new Vector2(1f, 0f);

        public static readonly Vector2 UnitY = new Vector2(0f, 1f);

        public float X;

        public float Y;
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2(float v)
        {
            this.X = v;
            this.Y = v;
        }

        //
        // Methods
        //
        public bool Equals(Vector2 other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 && this.Equals((Vector2)obj);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", this.X, this.Y);
        }
    }
}
