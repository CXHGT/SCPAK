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

        public static float Cos(float x)
        {
            return (float)Math.Cos((double)x);
        }

        public static float DegToRad(float degrees)
        {
            return degrees / 180f * 3.14159274f;
        }

        public static float Exp(float n)
        {
            return (float)Math.Exp((double)n);
        }

        public static double Floor(double x)
        {
            return Math.Floor(x);
        }

        public static float Floor(float x)
        {
            return (float)Math.Floor((double)x);
        }

        public static float Lerp(float x1, float x2, float f)
        {
            return x1 + (x2 - x1) * f;
        }
        public static double Max(double x1, double x2)
        {
            if (x1 <= x2)
            {
                return x2;
            }
            return x1;
        }
        public static int Max(int x1, int x2)
        {
            if (x1 <= x2)
            {
                return x2;
            }
            return x1;
        }
        public static float Max(float x1, float x2)
        {
            if (x1 <= x2)
            {
                return x2;
            }
            return x1;
        }
        public static long Max(long x1, long x2)
        {
            if (x1 <= x2)
            {
                return x2;
            }
            return x1;
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
        public static float Min(float x1, float x2)
        {
            if (x1 >= x2)
            {
                return x2;
            }
            return x1;
        }
        public static long Min(long x1, long x2)
        {
            if (x1 >= x2)
            {
                return x2;
            }
            return x1;
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

        public static float Sin(float x)
        {
            return (float)Math.Sin((double)x);
        }

        public static double Sqr(double x)
        {
            return x * x;
        }

        public static float Sqrt(float x)
        {
            return (float)Math.Sqrt((double)x);
        }
    }
}
