using System;
namespace Engine
{
    public struct Quaternion : IEquatable<Quaternion>
    {
        //
        // Static Fields
        //
        public static readonly Quaternion Identity = new Quaternion(0f, 0f, 0f, 1f);

        //
        // Fields
        //
        public float X;

        public float Y;

        public float Z;

        public float W;

        //
        // Constructors
        //
        public Quaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public static Quaternion CreateFromRotationMatrix(Matrix m)
        {
            float num = m.M11 + m.M22 + m.M33;
            Quaternion result = default(Quaternion);
            if (num > 0f)
            {
                float num2 = MathUtils.Sqrt(num + 1f);
                result.W = num2 * 0.5f;
                num2 = 0.5f / num2;
                result.X = (m.M23 - m.M32) * num2;
                result.Y = (m.M31 - m.M13) * num2;
                result.Z = (m.M12 - m.M21) * num2;
                return result;
            }
            if (m.M11 >= m.M22 && m.M11 >= m.M33)
            {
                float num3 = MathUtils.Sqrt(1f + m.M11 - m.M22 - m.M33);
                float num4 = 0.5f / num3;
                result.X = 0.5f * num3;
                result.Y = (m.M12 + m.M21) * num4;
                result.Z = (m.M13 + m.M31) * num4;
                result.W = (m.M23 - m.M32) * num4;
                return result;
            }
            if (m.M22 > m.M33)
            {
                float num5 = MathUtils.Sqrt(1f + m.M22 - m.M11 - m.M33);
                float num6 = 0.5f / num5;
                result.X = (m.M21 + m.M12) * num6;
                result.Y = 0.5f * num5;
                result.Z = (m.M32 + m.M23) * num6;
                result.W = (m.M31 - m.M13) * num6;
                return result;
            }
            float num7 = MathUtils.Sqrt(1f + m.M33 - m.M11 - m.M22);
            float num8 = 0.5f / num7;
            result.X = (m.M31 + m.M13) * num8;
            result.Y = (m.M32 + m.M23) * num8;
            result.Z = 0.5f * num7;
            result.W = (m.M12 - m.M21) * num8;
            return result;
        }
        public override bool Equals(object obj)
        {
            return obj is Quaternion && this.Equals((Quaternion)obj);
        }

        public bool Equals(Quaternion other)
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
                this.X.ToString (),
                this.Y.ToString (),
                this.Z.ToString (),
                this.W.ToString ()
            });
        }
    }
}
