using System;
namespace Engine
{
    public struct Vector2 : IEquatable<Vector2>
    {
        //
        // Static Fields
        //
        public static readonly Vector2 Zero = new Vector2(0f);

        public static readonly Vector2 One = new Vector2(1f);

        public static readonly Vector2 UnitX = new Vector2(1f, 0f);

        public static readonly Vector2 UnitY = new Vector2(0f, 1f);

        //
        // Fields
        //
        public float X;

        public float Y;

        //
        // Constructors
        //
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        //public Vector2(Point2 p)
        //{
        //    this.X = (float)p.X;
        //    this.Y = (float)p.Y;
        //}

        public Vector2(float v)
        {
            this.X = v;
            this.Y = v;
        }

        //
        // Static Methods
        //
        public static float Angle(Vector2 v1, Vector2 v2)
        {
            float num = MathUtils.Atan2(v1.Y, v1.X);
            float num2 = MathUtils.Atan2(v2.Y, v2.X) - num;
            if (num2 > 3.14159274f)
            {
                num2 -= 6.28318548f;
            }
            else if (num2 <= -3.14159274f)
            {
                num2 += 6.28318548f;
            }
            return num2;
        }

        public static Vector2 Ceiling(Vector2 v)
        {
            return new Vector2(MathUtils.Ceiling(v.X), MathUtils.Ceiling(v.Y));
        }

        public static Vector2 Clamp(Vector2 v, float min, float max)
        {
            return new Vector2(MathUtils.Clamp(v.X, min, max), MathUtils.Clamp(v.Y, min, max));
        }

        public static Vector2 CreateFromAngle(float angle)
        {
            float y = MathUtils.Cos(angle);
            return new Vector2(-MathUtils.Sin(angle), y);
        }

        public static float Cross(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        public static float Distance(Vector2 v1, Vector2 v2)
        {
            return MathUtils.Sqrt(Vector2.DistanceSquared(v1, v2));
        }

        public static float DistanceSquared(Vector2 v1, Vector2 v2)
        {
            return MathUtils.Sqr(v1.X - v2.X) + MathUtils.Sqr(v1.Y - v2.Y);
        }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static Vector2 Floor(Vector2 v)
        {
            return new Vector2(MathUtils.Floor(v.X), MathUtils.Floor(v.Y));
        }

        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float f)
        {
            return new Vector2(MathUtils.Lerp(v1.X, v2.X, f), MathUtils.Lerp(v1.Y, v2.Y, f));
        }

        public static Vector2 Max(Vector2 v, float f)
        {
            return new Vector2(MathUtils.Max(v.X, f), MathUtils.Max(v.Y, f));
        }

        public static Vector2 Max(Vector2 v1, Vector2 v2)
        {
            return new Vector2(MathUtils.Max(v1.X, v2.X), MathUtils.Max(v1.Y, v2.Y));
        }

        public static Vector2 Min(Vector2 v1, Vector2 v2)
        {
            return new Vector2(MathUtils.Min(v1.X, v2.X), MathUtils.Min(v1.Y, v2.Y));
        }

        public static Vector2 Min(Vector2 v, float f)
        {
            return new Vector2(MathUtils.Min(v.X, f), MathUtils.Min(v.Y, f));
        }

        public static Vector2 Normalize(Vector2 v)
        {
            float num = v.Length();
            if (num <= 0f)
            {
                return Vector2.UnitX;
            }
            return v / num;
        }

        public static Vector2 Perpendicular(Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }

        public static Vector2 Rotate(Vector2 v, float angle)
        {
            float num = MathUtils.Cos(angle);
            float num2 = MathUtils.Sin(angle);
            return new Vector2(num * v.X + num2 * v.Y, -num2 * v.X + num * v.Y);
        }

        public static Vector2 Round(Vector2 v)
        {
            return new Vector2(MathUtils.Round(v.X), MathUtils.Round(v.Y));
        }

        public static Vector2 Saturate(Vector2 v)
        {
            return new Vector2(MathUtils.Saturate(v.X), MathUtils.Saturate(v.Y));
        }

        public static void Transform(ref Vector2 v, ref Quaternion q, out Vector2 result)
        {
            float num = q.X + q.X;
            float num2 = q.Y + q.Y;
            float num3 = q.Z + q.Z;
            float num4 = q.W * num3;
            float num5 = q.X * num;
            float num6 = q.X * num2;
            float num7 = q.Y * num2;
            float num8 = q.Z * num3;
            result = new Vector2(v.X * (1f - num7 - num8) + v.Y * (num6 - num4), v.X * (num6 + num4) + v.Y * (1f - num5 - num8));
        }

        public static Vector2 Transform(Vector2 v, Matrix m)
        {
            return new Vector2(v.X * m.M11 + v.Y * m.M21 + m.M41, v.X * m.M12 + v.Y * m.M22 + m.M42);
        }

        public static void Transform(ref Vector2 v, ref Matrix m, out Vector2 result)
        {
            result = new Vector2(v.X * m.M11 + v.Y * m.M21 + m.M41, v.X * m.M12 + v.Y * m.M22 + m.M42);
        }

        public static Vector2 Transform(Vector2 v, Quaternion q)
        {
            float num = q.X + q.X;
            float num2 = q.Y + q.Y;
            float num3 = q.Z + q.Z;
            float num4 = q.W * num3;
            float num5 = q.X * num;
            float num6 = q.X * num2;
            float num7 = q.Y * num2;
            float num8 = q.Z * num3;
            return new Vector2(v.X * (1f - num7 - num8) + v.Y * (num6 - num4), v.X * (num6 + num4) + v.Y * (1f - num5 - num8));
        }

        public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Matrix m, Vector2[] destinationArray, int destinationIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 vector = sourceArray[sourceIndex + i];
                destinationArray[destinationIndex + i] = new Vector2(vector.X * m.M11 + vector.Y * m.M21 + m.M41, vector.X * m.M12 + vector.Y * m.M22 + m.M42);
            }
        }

        public static void TransformNormal(Vector2[] sourceArray, int sourceIndex, ref Matrix m, Vector2[] destinationArray, int destinationIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 vector = sourceArray[sourceIndex + i];
                destinationArray[destinationIndex + i] = new Vector2(vector.X * m.M11 + vector.Y * m.M21, vector.X * m.M12 + vector.Y * m.M22);
            }
        }

        public static void TransformNormal(ref Vector2 v, ref Matrix m, out Vector2 result)
        {
            result = new Vector2(v.X * m.M11 + v.Y * m.M21, v.X * m.M12 + v.Y * m.M22);
        }

        public static Vector2 TransformNormal(Vector2 v, Matrix m)
        {
            return new Vector2(v.X * m.M11 + v.Y * m.M21, v.X * m.M12 + v.Y * m.M22);
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

        public float Length()
        {
            return MathUtils.Sqrt(this.X * this.X + this.Y * this.Y);
        }

        public float LengthSquared()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", this.X, this.Y);
        }

        //
        // Operators
        //
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
        }

        public static Vector2 operator /(Vector2 v, float d)
        {
            float num = 1f / d;
            return new Vector2(v.X * num, v.Y * num);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !v1.Equals(v2);
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }

        public static Vector2 operator *(Vector2 v, float s)
        {
            return new Vector2(v.X * s, v.Y * s);
        }

        public static Vector2 operator *(float s, Vector2 v)
        {
            return new Vector2(v.X * s, v.Y * s);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }
    }
}
