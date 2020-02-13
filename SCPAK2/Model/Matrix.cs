using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public struct Matrix : IEquatable<Matrix>
    {
        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M21;
        public float M22;
        public float M23;
        public float M24;
        public float M31;
        public float M32;
        public float M33;
        public float M34;
        public float M41;
        public float M42;
        public float M43;
        public float M44;

        public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M14 = m14;
            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M24 = m24;
            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
            this.M34 = m34;
            this.M41 = m41;
            this.M42 = m42;
            this.M43 = m43;
            this.M44 = m44;
        }
        public static Matrix CreateFromAxisAngle(Vector3 axis, float angle)
        {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float num = (float)Math.Sin(angle);
            float num2 = (float)Math.Cos(angle);
            float num3 = x * x;
            float num4 = y * y;
            float num5 = z * z;
            float num6 = x * y;
            float num7 = x * z;
            float num8 = y * z;
            Matrix result;
            result.M11 = num3 + num2 * (1f - num3);
            result.M12 = num6 - num2 * num6 + num * z;
            result.M13 = num7 - num2 * num7 - num * y;
            result.M14 = 0f;
            result.M21 = num6 - num2 * num6 - num * z;
            result.M22 = num4 + num2 * (1f - num4);
            result.M23 = num8 - num2 * num8 + num * x;
            result.M24 = 0f;
            result.M31 = num7 - num2 * num7 + num * y;
            result.M32 = num8 - num2 * num8 - num * x;
            result.M33 = num5 + num2 * (1f - num5);
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }
        public static Matrix CreateScale(Vector3 scale)
        {
            return new Matrix(scale.X, 0f, 0f, 0f, 0f, scale.Y, 0f, 0f, 0f, 0f, scale.Z, 0f, 0f, 0f, 0f, 1f);
        }

        public static Matrix CreateScale(float scale)
        {
            return new Matrix(scale, 0f, 0f, 0f, 0f, scale, 0f, 0f, 0f, 0f, scale, 0f, 0f, 0f, 0f, 1f);
        }

        public static Matrix CreateScale(float x, float y, float z)
        {
            return new Matrix(x, 0f, 0f, 0f, 0f, y, 0f, 0f, 0f, 0f, z, 0f, 0f, 0f, 0f, 1f);
        }

        public static Matrix CreateTranslation(float x, float y, float z)
        {
            return new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, x, y, z, 1f);
        }
        public static void MultiplyRestricted(ref Matrix m1, ref Matrix m2, out Matrix result)
        {
            result.M11 = m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31 + m1.M14 * m2.M41;
            result.M12 = m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32 + m1.M14 * m2.M42;
            result.M13 = m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33 + m1.M14 * m2.M43;
            result.M14 = m1.M11 * m2.M14 + m1.M12 * m2.M24 + m1.M13 * m2.M34 + m1.M14 * m2.M44;
            result.M21 = m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31 + m1.M24 * m2.M41;
            result.M22 = m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32 + m1.M24 * m2.M42;
            result.M23 = m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33 + m1.M24 * m2.M43;
            result.M24 = m1.M21 * m2.M14 + m1.M22 * m2.M24 + m1.M23 * m2.M34 + m1.M24 * m2.M44;
            result.M31 = m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31 + m1.M34 * m2.M41;
            result.M32 = m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32 + m1.M34 * m2.M42;
            result.M33 = m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33 + m1.M34 * m2.M43;
            result.M34 = m1.M31 * m2.M14 + m1.M32 * m2.M24 + m1.M33 * m2.M34 + m1.M34 * m2.M44;
            result.M41 = m1.M41 * m2.M11 + m1.M42 * m2.M21 + m1.M43 * m2.M31 + m1.M44 * m2.M41;
            result.M42 = m1.M41 * m2.M12 + m1.M42 * m2.M22 + m1.M43 * m2.M32 + m1.M44 * m2.M42;
            result.M43 = m1.M41 * m2.M13 + m1.M42 * m2.M23 + m1.M43 * m2.M33 + m1.M44 * m2.M43;
            result.M44 = m1.M41 * m2.M14 + m1.M42 * m2.M24 + m1.M43 * m2.M34 + m1.M44 * m2.M44;
        }

        public static Matrix Transpose(Matrix m)
        {
            return new Matrix(m.M11, m.M21, m.M31, m.M41, m.M12, m.M22, m.M32, m.M42, m.M13, m.M23, m.M33, m.M43, m.M14, m.M24, m.M34, m.M44);
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix && this.Equals((Matrix)obj);
        }

        public bool Equals(Matrix other)
        {
            return this.M11 == other.M11 && this.M22 == other.M22 && this.M33 == other.M33 && this.M44 == other.M44 && this.M12 == other.M12 && this.M13 == other.M13 && this.M14 == other.M14 && this.M21 == other.M21 && this.M23 == other.M23 && this.M24 == other.M24 && this.M31 == other.M31 && this.M32 == other.M32 && this.M34 == other.M34 && this.M41 == other.M41 && this.M42 == other.M42 && this.M43 == other.M43;
        }

        public override int GetHashCode()
        {
            return this.M11.GetHashCode() + this.M12.GetHashCode() + this.M13.GetHashCode() + this.M14.GetHashCode() + this.M21.GetHashCode() + this.M22.GetHashCode() + this.M23.GetHashCode() + this.M24.GetHashCode() + this.M31.GetHashCode() + this.M32.GetHashCode() + this.M33.GetHashCode() + this.M34.GetHashCode() + this.M41.GetHashCode() + this.M42.GetHashCode() + this.M43.GetHashCode() + this.M44.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15}", new object[] {
                this.M11,
                this.M12,
                this.M13,
                this.M14,
                this.M21,
                this.M22,
                this.M23,
                this.M24,
                this.M31,
                this.M32,
                this.M33,
                this.M34,
                this.M41,
                this.M42,
                this.M43,
                this.M44
            });
        }
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix result;
            Matrix.MultiplyRestricted(ref m1, ref m2, out result);
            return result;
        }

        public static readonly Matrix Identity = new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
    }
}
