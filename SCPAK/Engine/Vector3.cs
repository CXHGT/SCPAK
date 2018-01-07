using System;
namespace Engine
{
    public struct Vector3 : IEquatable<Vector3>
    {
        //
        // Static Fields
        //
        public static readonly Vector3 Zero = new Vector3(0f);

        public static readonly Vector3 One = new Vector3(1f);

        public static readonly Vector3 UnitX = new Vector3(1f, 0f, 0f);

        public static readonly Vector3 UnitY = new Vector3(0f, 1f, 0f);

        public static readonly Vector3 UnitZ = new Vector3(0f, 0f, 1f);

        //
        // Fields
        //
        public float X;

        public float Y;

        public float Z;
        //
        // Constructors
        //
        public Vector3(float v)
        {
            this.X = v;
            this.Y = v;
            this.Z = v;
        }

        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return new Vector3(MathUtils.Max(v1.X, v2.X), MathUtils.Max(v1.Y, v2.Y), MathUtils.Max(v1.Z, v2.Z));
        }
        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(MathUtils.Min(v1.X, v2.X), MathUtils.Min(v1.Y, v2.Y), MathUtils.Min(v1.Z, v2.Z));
        }

        public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Matrix m, Vector3[] destinationArray, int destinationIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 vector = sourceArray[sourceIndex + i];
                destinationArray[destinationIndex + i] = new Vector3(vector.X * m.M11 + vector.Y * m.M21 + vector.Z * m.M31 + m.M41, vector.X * m.M12 + vector.Y * m.M22 + vector.Z * m.M32 + m.M42, vector.X * m.M13 + vector.Y * m.M23 + vector.Z * m.M33 + m.M43);
            }
        }
        public bool Equals(Vector3 other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 && this.Equals((Vector3)obj);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", this.X, this.Y, this.Z);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !v1.Equals(v2);
        }
    }
}
