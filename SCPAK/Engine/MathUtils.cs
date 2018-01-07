using System;
namespace Engine
{
    public static class MathUtils
    {
        //
        // Static Fields
        //
        public const float PI = 3.14159274f;

        public const float E = 2.71828175f;

        //
        // Static Methods
        //
        public static double Abs(double x)
        {
            return Math.Abs(x);
        }

        public static float Abs(float x)
        {
            return Math.Abs(x);
        }
        public static float Acos(float x)
        {
            return (float)Math.Acos((double)x);
        }
        public static float Asin(float x)
        {
            return (float)Math.Asin((double)x);
        }

        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2((double)y, (double)x);
        }
        public static float Ceiling(float x)
        {
            return (float)Math.Ceiling((double)x);
        }

        public static int Clamp(int x, int min, int max)
        {
            if (x < min)
            {
                return min;
            }
            if (x <= max)
            {
                return x;
            }
            return max;
        }

        public static double Clamp(double x, double min, double max)
        {
            if (x < min)
            {
                return min;
            }
            if (x <= max)
            {
                return x;
            }
            return max;
        }

        public static float Clamp(float x, float min, float max)
        {
            if (x < min)
            {
                return min;
            }
            if (x <= max)
            {
                return x;
            }
            return max;
        }

        public static long Clamp(long x, long min, long max)
        {
            if (x < min)
            {
                return min;
            }
            if (x <= max)
            {
                return x;
            }
            return max;
        }

        public static double Cos(double x)
        {
            return Math.Cos(x);
        }

        public static float Cos(float x)
        {
            return (float)Math.Cos((double)x);
        }

        public static double DegToRad(double degrees)
        {
            return degrees / 180.0 * 3.1415926535897931;
        }

        public static float DegToRad(float degrees)
        {
            return degrees / 180f * 3.14159274f;
        }

        public static float Exp(float n)
        {
            return (float)Math.Exp((double)n);
        }

        public static double Exp(double n)
        {
            return Math.Exp(n);
        }

        public static double Floor(double x)
        {
            return Math.Floor(x);
        }

        public static float Floor(float x)
        {
            return (float)Math.Floor((double)x);
        }

        public static uint Hash(uint key)
        {
            key = ~key + (key << 15);
            key ^= key >> 12;
            key += key << 2;
            key ^= key >> 4;
            key *= 2057u;
            key ^= key >> 16;
            return key;
        }

        public static bool IsPowerOf2(long x)
        {
            return x > 0L && (x & x - 1L) == 0L;
        }

        public static bool IsPowerOf2(uint x)
        {
            return x > 0u && (x & x - 1u) == 0u;
        }

        public static float Lerp(float x1, float x2, float f)
        {
            return x1 + (x2 - x1) * f;
        }

        public static double Lerp(double x1, double x2, double f)
        {
            return x1 + (x2 - x1) * f;
        }

        public static float Log(float x)
        {
            return (float)Math.Log((double)x);
        }

        public static double Log(double x)
        {
            return Math.Log(x);
        }

        public static double Log10(double x)
        {
            return Math.Log10(x);
        }

        public static float Log10(float x)
        {
            return (float)Math.Log10((double)x);
        }

        public static double Max(double x1, double x2)
        {
            if (x1 <= x2)
            {
                return x2;
            }
            return x1;
        }

        public static double Max(double x1, double x2, double x3)
        {
            return MathUtils.Max(MathUtils.Max(x1, x2), x3);
        }

        public static double Max(double x1, double x2, double x3, double x4)
        {
            return MathUtils.Max(MathUtils.Max(MathUtils.Max(x1, x2), x3), x4);
        }

        public static int Max(int x1, int x2, int x3)
        {
            return MathUtils.Max(MathUtils.Max(x1, x2), x3);
        }

        public static int Max(int x1, int x2, int x3, int x4)
        {
            return MathUtils.Max(MathUtils.Max(MathUtils.Max(x1, x2), x3), x4);
        }

        public static int Max(int x1, int x2)
        {
            if (x1 <= x2)
            {
                return x2;
            }
            return x1;
        }

        public static float Max(float x1, float x2, float x3, float x4)
        {
            return MathUtils.Max(MathUtils.Max(MathUtils.Max(x1, x2), x3), x4);
        }

        public static float Max(float x1, float x2, float x3)
        {
            return MathUtils.Max(MathUtils.Max(x1, x2), x3);
        }

        public static float Max(float x1, float x2)
        {
            if (x1 <= x2)
            {
                return x2;
            }
            return x1;
        }

        public static long Max(long x1, long x2, long x3, long x4)
        {
            return MathUtils.Max(MathUtils.Max(MathUtils.Max(x1, x2), x3), x4);
        }

        public static long Max(long x1, long x2, long x3)
        {
            return MathUtils.Max(MathUtils.Max(x1, x2), x3);
        }

        public static long Max(long x1, long x2)
        {
            if (x1 <= x2)
            {
                return x2;
            }
            return x1;
        }

        public static int Min(int x1, int x2, int x3, int x4)
        {
            return MathUtils.Min(MathUtils.Min(MathUtils.Min(x1, x2), x3), x4);
        }

        public static double Min(double x1, double x2)
        {
            if (x1 >= x2)
            {
                return x2;
            }
            return x1;
        }

        public static int Min(int x1, int x2)
        {
            if (x1 >= x2)
            {
                return x2;
            }
            return x1;
        }

        public static long Min(long x1, long x2, long x3, long x4)
        {
            return MathUtils.Min(MathUtils.Min(MathUtils.Min(x1, x2), x3), x4);
        }

        public static float Min(float x1, float x2)
        {
            if (x1 >= x2)
            {
                return x2;
            }
            return x1;
        }

        public static double Min(double x1, double x2, double x3)
        {
            return MathUtils.Min(MathUtils.Min(x1, x2), x3);
        }

        public static float Min(float x1, float x2, float x3)
        {
            return MathUtils.Min(MathUtils.Min(x1, x2), x3);
        }

        public static long Min(long x1, long x2, long x3)
        {
            return MathUtils.Min(MathUtils.Min(x1, x2), x3);
        }

        public static float Min(float x1, float x2, float x3, float x4)
        {
            return MathUtils.Min(MathUtils.Min(MathUtils.Min(x1, x2), x3), x4);
        }

        public static long Min(long x1, long x2)
        {
            if (x1 >= x2)
            {
                return x2;
            }
            return x1;
        }

        public static int Min(int x1, int x2, int x3)
        {
            return MathUtils.Min(MathUtils.Min(x1, x2), x3);
        }

        public static double Min(double x1, double x2, double x3, double x4)
        {
            return MathUtils.Min(MathUtils.Min(MathUtils.Min(x1, x2), x3), x4);
        }

        public static uint NextPowerOf2(uint x)
        {
            x -= 1u;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x += 1u;
            return x;
        }

        public static ulong NextPowerOf2(ulong x)
        {
            x -= 1uL;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x |= x >> 32;
            x += 1uL;
            return x;
        }

        public static float NormalizeAngle(float angle)
        {
            angle = (float)Math.IEEERemainder((double)angle, 6.2831854820251465);
            if (angle > 3.14159274f)
            {
                angle -= 6.28318548f;
            }
            else if (angle <= -3.14159274f)
            {
                angle += 6.28318548f;
            }
            return angle;
        }

        public static double NormalizeAngle(double angle)
        {
            angle = Math.IEEERemainder(angle, 6.2831853071795862);
            if (angle > 3.1415927410125732)
            {
                angle -= 6.2831853071795862;
            }
            else if (angle <= -3.1415926535897931)
            {
                angle += 6.2831853071795862;
            }
            return angle;
        }

        public static float Pow(float x, float n)
        {
            return (float)Math.Pow((double)x, (double)n);
        }

        public static double Pow(double x, double n)
        {
            return Math.Pow(x, n);
        }

        public static double PowSign(double x, double n)
        {
            return MathUtils.Sign(x) * MathUtils.Pow(MathUtils.Abs(x), n);
        }

        public static float PowSign(float x, float n)
        {
            return MathUtils.Sign(x) * MathUtils.Pow(MathUtils.Abs(x), n);
        }

        public static double RadToDeg(double radians)
        {
            return radians * 180.0 / 3.1415926535897931;
        }

        public static float RadToDeg(float radians)
        {
            return radians * 180f / 3.14159274f;
        }

        public static double Remainder(double x, double y)
        {
            return x - MathUtils.Floor(x / y) * y;
        }

        public static float Remainder(float x, float y)
        {
            return x - MathUtils.Floor(x / y) * y;
        }

        public static double Round(double x)
        {
            return Math.Round(x);
        }

        public static float Round(float x)
        {
            return (float)Math.Round((double)x);
        }

        public static float Saturate(float x)
        {
            if (x < 0f)
            {
                return 0f;
            }
            if (x <= 1f)
            {
                return x;
            }
            return 1f;
        }

        public static double Saturate(double x)
        {
            if (x < 0.0)
            {
                return 0.0;
            }
            if (x <= 1.0)
            {
                return x;
            }
            return 1.0;
        }

        public static float Sigmoid(float x, float steepness)
        {
            if (x <= 0f)
            {
                return 0f;
            }
            if (x >= 1f)
            {
                return 1f;
            }
            float num = MathUtils.Exp(steepness);
            return (MathUtils.Exp(2f * steepness * x + steepness) - num) / ((num - 1f) * MathUtils.Exp(2f * steepness * x) + MathUtils.Exp(2f * steepness) - num);
        }

        public static double Sign(double x)
        {
            return (double)Math.Sign(x);
        }

        public static float Sign(float x)
        {
            return (float)Math.Sign(x);
        }

        public static int Sign(int x)
        {
            return Math.Sign(x);
        }

        public static long Sign(long x)
        {
            return (long)Math.Sign(x);
        }

        public static double Sin(double x)
        {
            return Math.Sin(x);
        }

        public static float Sin(float x)
        {
            return (float)Math.Sin((double)x);
        }

        public static float SmoothStep(float min, float max, float x)
        {
            x = MathUtils.Clamp((x - min) / (max - min), 0f, 1f);
            return x * x * (3f - 2f * x);
        }

        public static double SmoothStep(double min, double max, double x)
        {
            x = MathUtils.Clamp((x - min) / (max - min), 0.0, 1.0);
            return x * x * (3.0 - 2.0 * x);
        }

        public static float Sqr(float x)
        {
            return x * x;
        }

        public static double Sqr(double x)
        {
            return x * x;
        }

        public static float Sqrt(float x)
        {
            return (float)Math.Sqrt((double)x);
        }

        public static double Sqrt(double x)
        {
            return Math.Sqrt(x);
        }

        public static float Tan(float x)
        {
            return (float)Math.Tan((double)x);
        }

        public static double Tan(double x)
        {
            return Math.Tan(x);
        }
    }
}
