using System;
namespace Engine
{
    public struct Vector4 : IEquatable<Vector4>
    {
        //
        // Static Fields
        //
        public static readonly Vector4 UnitW = new Vector4(0f, 0f, 0f, 1f);

        public static readonly Vector4 UnitZ = new Vector4(0f, 0f, 1f, 0f);

        public static readonly Vector4 UnitY = new Vector4(0f, 1f, 0f, 0f);

        public static readonly Vector4 UnitX = new Vector4(1f, 0f, 0f, 0f);

        public static readonly Vector4 Zero = new Vector4(0f);

        public static readonly Vector4 One = new Vector4(1f);

        public float X;

        public float Z;

        public float Y;

        public float W;

        public Vector4(float v)
        {
            this.X = v;
            this.Y = v;
            this.Z = v;
            this.W = v;
        }

        public Vector4(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }
        public override bool Equals(object obj)
        {
            return obj is Vector4 && this.Equals((Vector4)obj);
        }

        public bool Equals(Vector4 other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode() + this.W.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}", new object[] {
                this.X,
                this.Y,
                this.Z,
                this.W
            });
        }
    }
}
