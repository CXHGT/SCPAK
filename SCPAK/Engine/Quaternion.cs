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

        public Quaternion(Vector3 v, float s)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            this.W = s;
        }

        //
        // Static Methods
        //
        public static Quaternion Concatenate(Quaternion q1, Quaternion q2)
        {
            float x = q2.X;
            float y = q2.Y;
            float z = q2.Z;
            float w = q2.W;
            float x2 = q1.X;
            float y2 = q1.Y;
            float z2 = q1.Z;
            float w2 = q1.W;
            float num = y * z2 - z * y2;
            float num2 = z * x2 - x * z2;
            float num3 = x * y2 - y * x2;
            float num4 = x * x2 + y * y2 + z * z2;
            Quaternion result;
            result.X = x * w2 + x2 * w + num;
            result.Y = y * w2 + y2 * w + num2;
            result.Z = z * w2 + z2 * w + num3;
            result.W = w * w2 - num4;
            return result;
        }

        public static Quaternion Conjugate(Quaternion q)
        {
            return new Quaternion(-q.X, -q.Y, -q.Z, q.W);
        }

        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            float expr_07 = angle * 0.5f;
            float num = MathUtils.Sin(expr_07);
            float w = MathUtils.Cos(expr_07);
            Quaternion result;
            result.X = axis.X * num;
            result.Y = axis.Y * num;
            result.Z = axis.Z * num;
            result.W = w;
            return result;
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

        public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            float expr_07 = roll * 0.5f;
            float num = MathUtils.Sin(expr_07);
            float num2 = MathUtils.Cos(expr_07);
            float expr_1B = pitch * 0.5f;
            float num3 = MathUtils.Sin(expr_1B);
            float num4 = MathUtils.Cos(expr_1B);
            float expr_30 = yaw * 0.5f;
            float num5 = MathUtils.Sin(expr_30);
            float num6 = MathUtils.Cos(expr_30);
            Quaternion result;
            result.X = num6 * num3 * num2 + num5 * num4 * num;
            result.Y = num5 * num4 * num2 - num6 * num3 * num;
            result.Z = num6 * num4 * num - num5 * num3 * num2;
            result.W = num6 * num4 * num2 + num5 * num3 * num;
            return result;
        }

        public static float Dot(Quaternion q1, Quaternion q2)
        {
            return q1.X * q2.X + q1.Y * q2.Y + q1.Z * q2.Z + q1.W * q2.W;
        }

        public static Quaternion Inverse(Quaternion q)
        {
            float num = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
            float num2 = 1f / num;
            Quaternion result;
            result.X = -q.X * num2;
            result.Y = -q.Y * num2;
            result.Z = -q.Z * num2;
            result.W = q.W * num2;
            return result;
        }

        public static Quaternion Lerp(Quaternion q1, Quaternion q2, float f)
        {
            float num = 1f - f;
            Quaternion result;
            if (q1.X * q2.X + q1.Y * q2.Y + q1.Z * q2.Z + q1.W * q2.W >= 0f)
            {
                result.X = num * q1.X + f * q2.X;
                result.Y = num * q1.Y + f * q2.Y;
                result.Z = num * q1.Z + f * q2.Z;
                result.W = num * q1.W + f * q2.W;
            }
            else
            {
                result.X = num * q1.X - f * q2.X;
                result.Y = num * q1.Y - f * q2.Y;
                result.Z = num * q1.Z - f * q2.Z;
                result.W = num * q1.W - f * q2.W;
            }
            float num2 = 1f / result.Length();
            result.X *= num2;
            result.Y *= num2;
            result.Z *= num2;
            result.W *= num2;
            return result;
        }

        public static Quaternion Normalize(Quaternion q)
        {
            float num = q.Length();
            if (num == 0f)
            {
                return Quaternion.Identity;
            }
            return q / num;
        }

        public static Quaternion Slerp(Quaternion q1, Quaternion q2, float f)
        {
            float num = q1.X * q2.X + q1.Y * q2.Y + q1.Z * q2.Z + q1.W * q2.W;
            bool flag = false;
            if (num < 0f)
            {
                flag = true;
                num = -num;
            }
            float num2;
            float num3;
            if (num > 0.999999f)
            {
                num2 = 1f - f;
                num3 = (flag ? (-f) : f);
            }
            else
            {
                float num4 = MathUtils.Acos(num);
                float num5 = 1f / MathUtils.Sin(num4);
                num2 = MathUtils.Sin((1f - f) * num4) * num5;
                num3 = (flag ? (-MathUtils.Sin(f * num4) * num5) : (MathUtils.Sin(f * num4) * num5));
            }
            Quaternion result;
            result.X = num2 * q1.X + num3 * q2.X;
            result.Y = num2 * q1.Y + num3 * q2.Y;
            result.Z = num2 * q1.Z + num3 * q2.Z;
            result.W = num2 * q1.W + num3 * q2.W;
            return result;
        }

        //
        // Methods
        //
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

        public float Length()
        {
            return MathUtils.Sqrt(this.LengthSquared());
        }

        public float LengthSquared()
        {
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
        }

        public Vector3 ToForwardVector()
        {
            return new Vector3(-2f * (this.Y * this.W + this.X * this.Z), 2f * (this.X * this.W - this.Y * this.Z), 2f * (this.X * this.X + this.Y * this.Y) - 1f);
        }

        public Matrix ToMatrix()
        {
            float num = this.X * this.X;
            float num2 = this.Y * this.Y;
            float num3 = this.Z * this.Z;
            float num4 = this.X * this.Y;
            float num5 = this.Z * this.W;
            float num6 = this.X * this.Z;
            float num7 = this.Y * this.W;
            float num8 = this.Y * this.Z;
            float num9 = this.X * this.W;
            Matrix result;
            result.M11 = 1f - 2f * (num2 + num3);
            result.M12 = 2f * (num4 + num5);
            result.M13 = 2f * (num6 - num7);
            result.M14 = 0f;
            result.M21 = 2f * (num4 - num5);
            result.M22 = 1f - 2f * (num3 + num);
            result.M23 = 2f * (num8 + num9);
            result.M24 = 0f;
            result.M31 = 2f * (num6 + num7);
            result.M32 = 2f * (num8 - num9);
            result.M33 = 1f - 2f * (num2 + num);
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public Vector3 ToRightVector()
        {
            return new Vector3(1f - 2f * (this.Y * this.Y + this.Z * this.Z), 2f * (this.X * this.Y + this.Z * this.W), 2f * (this.X * this.Z - this.W * this.Y));
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

        public Vector3 ToUpVector()
        {
            return new Vector3(2f * (this.X * this.Y - this.Z * this.W), 1f - 2f * (this.X * this.X + this.Z * this.Z), 2f * (this.Y * this.Z + this.X * this.W));
        }

        public Vector3 ToYawPitchRoll()
        {
            float arg_1C_0 = this.X * this.Y;
            float num = this.Z * this.W;
            float num2 = arg_1C_0 + num;
            float x;
            float y;
            float z;
            if (num2 < -0.499023438f || num2 > 0.499023438f)
            {
                float expr_38 = MathUtils.Sign(num2);
                x = expr_38 * 2f * MathUtils.Atan2(this.X, this.W);
                y = expr_38 * 3.14159274f / 2f;
                z = 0f;
            }
            else
            {
                float num3 = this.X * this.X;
                float num4 = this.X * this.Z;
                float num5 = this.X * this.W;
                float num6 = this.Y * this.Y;
                float num7 = this.Y * this.W;
                float num8 = this.Y * this.Z;
                float num9 = this.Z * this.Z;
                x = MathUtils.Atan2(2f * num7 - 2f * num4, 1f - 2f * num6 - 2f * num9);
                y = MathUtils.Atan2(2f * num5 - 2f * num8, 1f - 2f * num3 - 2f * num9);
                z = MathUtils.Asin(2f * num2);
            }
            return new Vector3(x, y, z);
        }

        //
        // Operators
        //
        public static Quaternion operator +(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.X + q2.X, q1.Y + q2.Y, q1.Z + q2.Z, q1.W + q2.W);
        }

        public static Quaternion operator /(Quaternion q1, Quaternion q2)
        {
            float x = q1.X;
            float y = q1.Y;
            float z = q1.Z;
            float w = q1.W;
            float num = q2.X * q2.X + q2.Y * q2.Y + q2.Z * q2.Z + q2.W * q2.W;
            float num2 = 1f / num;
            float num3 = -q2.X * num2;
            float num4 = -q2.Y * num2;
            float num5 = -q2.Z * num2;
            float num6 = q2.W * num2;
            float num7 = y * num5 - z * num4;
            float num8 = z * num3 - x * num5;
            float num9 = x * num4 - y * num3;
            float num10 = x * num3 + y * num4 + z * num5;
            Quaternion result;
            result.X = x * num6 + num3 * w + num7;
            result.Y = y * num6 + num4 * w + num8;
            result.Z = z * num6 + num5 * w + num9;
            result.W = w * num6 - num10;
            return result;
        }

        public static Quaternion operator /(Quaternion q, float d)
        {
            float num = 1f / d;
            return new Quaternion(q.X * num, q.Y * num, q.Z * num, q.W * num);
        }

        public static bool operator ==(Quaternion q1, Quaternion q2)
        {
            return q1.Equals(q2);
        }

        public static bool operator !=(Quaternion q1, Quaternion q2)
        {
            return !q1.Equals(q2);
        }

        public static Quaternion operator *(Quaternion q1, Quaternion q2)
        {
            float x = q1.X;
            float y = q1.Y;
            float z = q1.Z;
            float w = q1.W;
            float x2 = q2.X;
            float y2 = q2.Y;
            float z2 = q2.Z;
            float w2 = q2.W;
            float num = y * z2 - z * y2;
            float num2 = z * x2 - x * z2;
            float num3 = x * y2 - y * x2;
            float num4 = x * x2 + y * y2 + z * z2;
            Quaternion result;
            result.X = x * w2 + x2 * w + num;
            result.Y = y * w2 + y2 * w + num2;
            result.Z = z * w2 + z2 * w + num3;
            result.W = w * w2 - num4;
            return result;
        }

        public static Quaternion operator *(Quaternion q, float s)
        {
            return new Quaternion(q.X * s, q.Y * s, q.Z * s, q.W * s);
        }

        public static Quaternion operator -(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.X - q2.X, q1.Y - q2.Y, q1.Z - q2.Z, q1.W - q2.W);
        }

        public static Quaternion operator -(Quaternion q)
        {
            return new Quaternion(-q.X, -q.Y, -q.Z, -q.W);
        }
    }
}
