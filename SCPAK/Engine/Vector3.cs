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
        // Properties
        //
        public Vector2 XY
        {
            get
            {
                return new Vector2(this.X, this.Y);
            }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public Vector2 XZ
        {
            get
            {
                return new Vector2(this.X, this.Z);
            }
            set
            {
                this.X = value.X;
                this.Z = value.Y;
            }
        }

        public Vector2 YZ
        {
            get
            {
                return new Vector2(this.Y, this.Z);
            }
            set
            {
                this.Y = value.X;
                this.Z = value.Y;
            }
        }

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

        public Vector3(Vector2 xy, float z)
        {
            this.X = xy.X;
            this.Y = xy.Y;
            this.Z = z;
        }

        //public Vector3(Point3 p)
        //{
        //    this.X = (float)p.X;
        //    this.Y = (float)p.Y;
        //    this.Z = (float)p.Z;
        //}

        public Vector3(Color c)
        {
            this.X = (float)c.R / 255f;
            this.Y = (float)c.G / 255f;
            this.Z = (float)c.B / 255f;
        }

        //
        // Static Methods
        //
        public static Vector3 Ceiling(Vector3 v)
        {
            return new Vector3(MathUtils.Ceiling(v.X), MathUtils.Ceiling(v.Y), MathUtils.Ceiling(v.Z));
        }

        public static Vector3 Clamp(Vector3 v, float min, float max)
        {
            return new Vector3(MathUtils.Clamp(v.X, min, max), MathUtils.Clamp(v.Y, min, max), MathUtils.Clamp(v.Z, min, max));
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        public static float Distance(Vector3 v1, Vector3 v2)
        {
            return MathUtils.Sqrt(Vector3.DistanceSquared(v1, v2));
        }

        public static float DistanceSquared(Vector3 v1, Vector3 v2)
        {
            return MathUtils.Sqr(v1.X - v2.X) + MathUtils.Sqr(v1.Y - v2.Y) + MathUtils.Sqr(v1.Z - v2.Z);
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector3 Floor(Vector3 v)
        {
            return new Vector3(MathUtils.Floor(v.X), MathUtils.Floor(v.Y), MathUtils.Floor(v.Z));
        }

        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float f)
        {
            return new Vector3(MathUtils.Lerp(v1.X, v2.X, f), MathUtils.Lerp(v1.Y, v2.Y, f), MathUtils.Lerp(v1.Z, v2.Z, f));
        }

        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return new Vector3(MathUtils.Max(v1.X, v2.X), MathUtils.Max(v1.Y, v2.Y), MathUtils.Max(v1.Z, v2.Z));
        }

        public static Vector3 Max(Vector3 v, float f)
        {
            return new Vector3(MathUtils.Max(v.X, f), MathUtils.Max(v.Y, f), MathUtils.Max(v.Z, f));
        }

        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(MathUtils.Min(v1.X, v2.X), MathUtils.Min(v1.Y, v2.Y), MathUtils.Min(v1.Z, v2.Z));
        }

        public static Vector3 Min(Vector3 v, float f)
        {
            return new Vector3(MathUtils.Min(v.X, f), MathUtils.Min(v.Y, f), MathUtils.Min(v.Z, f));
        }

        public static Vector3 Normalize(Vector3 v)
        {
            float num = v.Length();
            if (num <= 0f)
            {
                return Vector3.UnitX;
            }
            return v / num;
        }

        public static Vector3 Round(Vector3 v)
        {
            return new Vector3(MathUtils.Round(v.X), MathUtils.Round(v.Y), MathUtils.Round(v.Z));
        }

        public static Vector3 Saturate(Vector3 v)
        {
            return new Vector3(MathUtils.Saturate(v.X), MathUtils.Saturate(v.Y), MathUtils.Saturate(v.Z));
        }

        public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Matrix m, Vector3[] destinationArray, int destinationIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 vector = sourceArray[sourceIndex + i];
                destinationArray[destinationIndex + i] = new Vector3(vector.X * m.M11 + vector.Y * m.M21 + vector.Z * m.M31 + m.M41, vector.X * m.M12 + vector.Y * m.M22 + vector.Z * m.M32 + m.M42, vector.X * m.M13 + vector.Y * m.M23 + vector.Z * m.M33 + m.M43);
            }
        }

        public static Vector3 Transform(Vector3 v, Matrix m)
        {
            return new Vector3(v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41, v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42, v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43);
        }

        public static void Transform(ref Vector3 v, ref Matrix m, out Vector3 result)
        {
            result = new Vector3(v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41, v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42, v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43);
        }

        public static Vector3 Transform(Vector3 v, Quaternion q)
        {
            float num = q.X + q.X;
            float num2 = q.Y + q.Y;
            float num3 = q.Z + q.Z;
            float num4 = q.W * num;
            float num5 = q.W * num2;
            float num6 = q.W * num3;
            float num7 = q.X * num;
            float num8 = q.X * num2;
            float num9 = q.X * num3;
            float num10 = q.Y * num2;
            float num11 = q.Y * num3;
            float num12 = q.Z * num3;
            return new Vector3(v.X * (1f - num10 - num12) + v.Y * (num8 - num6) + v.Z * (num9 + num5), v.X * (num8 + num6) + v.Y * (1f - num7 - num12) + v.Z * (num11 - num4), v.X * (num9 - num5) + v.Y * (num11 + num4) + v.Z * (1f - num7 - num10));
        }

        public static void Transform(ref Vector3 v, ref Quaternion q, out Vector3 result)
        {
            float num = q.X + q.X;
            float num2 = q.Y + q.Y;
            float num3 = q.Z + q.Z;
            float num4 = q.W * num;
            float num5 = q.W * num2;
            float num6 = q.W * num3;
            float num7 = q.X * num;
            float num8 = q.X * num2;
            float num9 = q.X * num3;
            float num10 = q.Y * num2;
            float num11 = q.Y * num3;
            float num12 = q.Z * num3;
            result = new Vector3(v.X * (1f - num10 - num12) + v.Y * (num8 - num6) + v.Z * (num9 + num5), v.X * (num8 + num6) + v.Y * (1f - num7 - num12) + v.Z * (num11 - num4), v.X * (num9 - num5) + v.Y * (num11 + num4) + v.Z * (1f - num7 - num10));
        }

        public static Vector3 TransformNormal(Vector3 v, Matrix m)
        {
            return new Vector3(v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31, v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32, v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33);
        }

        public static void TransformNormal(ref Vector3 v, ref Matrix m, out Vector3 result)
        {
            result = new Vector3(v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31, v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32, v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33);
        }

        public static void TransformNormal(Vector3[] sourceArray, int sourceIndex, ref Matrix m, Vector3[] destinationArray, int destinationIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 vector = sourceArray[sourceIndex + i];
                destinationArray[destinationIndex + i] = new Vector3(vector.X * m.M11 + vector.Y * m.M21 + vector.Z * m.M31, vector.X * m.M12 + vector.Y * m.M22 + vector.Z * m.M32, vector.X * m.M13 + vector.Y * m.M23 + vector.Z * m.M33);
            }
        }

        //
        // Methods
        //
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

        public float Length()
        {
            return MathUtils.Sqrt(this.LengthSquared());
        }

        public float LengthSquared()
        {
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", this.X, this.Y, this.Z);
        }

        //
        // Operators
        //
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static Vector3 operator /(Vector3 v, float d)
        {
            float num = 1f / d;
            return new Vector3(v.X * num, v.Y * num, v.Z * num);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !v1.Equals(v2);
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Vector3 operator *(Vector3 v, float s)
        {
            return new Vector3(v.X * s, v.Y * s, v.Z * s);
        }

        public static Vector3 operator *(float s, Vector3 v)
        {
            return new Vector3(v.X * s, v.Y * s, v.Z * s);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }
    }
}
