using System;
namespace Engine
{
    public struct Color : IEquatable<Color>
    {
        //
        // Static Fields
        //
        public static Color Green = new Color(0, 255, 0, 255);

        public static Color LightMagenta = new Color(255, 128, 255, 255);

        public static Color LightBlue = new Color(128, 128, 255, 255);

        public static Color LightYellow = new Color(255, 255, 128, 255);

        public static Color LightGreen = new Color(128, 255, 128, 255);

        public static Color LightRed = new Color(255, 128, 128, 255);

        public static Color LightGray = new Color(192, 192, 192, 255);

        public static Color Cyan = new Color(0, 255, 255, 255);

        public static Color Magenta = new Color(255, 0, 255, 255);

        public static Color Blue = new Color(0, 0, 255, 255);

        public static Color Yellow = new Color(255, 255, 0, 255);

        public static Color LightCyan = new Color(128, 255, 255, 255);

        public static Color White = new Color(255, 255, 255, 255);

        public static Color Gray = new Color(128, 128, 128, 255);

        public static Color DarkCyan = new Color(0, 128, 128, 255);

        public static Color DarkMagenta = new Color(128, 0, 128, 255);

        public static Color DarkBlue = new Color(0, 0, 128, 255);

        public static Color DarkYellow = new Color(128, 128, 0, 255);

        public static Color DarkGreen = new Color(0, 128, 0, 255);

        public static Color DarkRed = new Color(128, 0, 0, 255);

        public static Color DarkGray = new Color(64, 64, 64, 255);

        public static Color Black = new Color(0, 0, 0, 255);

        public static Color Transparent = new Color(0, 0, 0, 0);

        public static Color Red = new Color(255, 0, 0, 255);

        //
        // Fields
        //
        public uint PackedValue;

        //
        // Properties
        //
        public byte A
        {
            get
            {
                return (byte)(this.PackedValue >> 24);
            }
            set
            {
                this.PackedValue = ((this.PackedValue & 16777215u) | (uint)((uint)value << 24));
            }
        }

        public byte B
        {
            get
            {
                return (byte)(this.PackedValue >> 16);
            }
            set
            {
                this.PackedValue = ((this.PackedValue & 4278255615u) | (uint)((uint)value << 16));
            }
        }

        public byte G
        {
            get
            {
                return (byte)(this.PackedValue >> 8);
            }
            set
            {
                this.PackedValue = ((this.PackedValue & 4294902015u) | (uint)((uint)value << 8));
            }
        }

        public byte R
        {
            get
            {
                return (byte)this.PackedValue;
            }
            set
            {
                this.PackedValue = ((this.PackedValue & 4294967040u) | (uint)value);
            }
        }

        public Color(float r, float g, float b)
        {
            this = new Color((byte)(MathUtils.Saturate(r) * 255f), (byte)(MathUtils.Saturate(g) * 255f), (byte)(MathUtils.Saturate(b) * 255f), (byte)255);
        }

        public Color(byte r, byte g, byte b, byte a)
        {
            this.PackedValue = (uint)((int)a << 24 | (int)b << 16 | (int)g << 8 | (int)r);
        }

        public Color(byte r, byte g, byte b)
        {
            this.PackedValue = (uint)(-16777216 | (int)b << 16 | (int)g << 8 | (int)r);
        }

        public Color(uint packedValue)
        {
            this.PackedValue = packedValue;
        }

        public Color(int r, int g, int b, int a)
        {
            this = new Color((byte)MathUtils.Clamp(r, 0, 255), (byte)MathUtils.Clamp(g, 0, 255), (byte)MathUtils.Clamp(b, 0, 255), (byte)MathUtils.Clamp(a, 0, 255));
        }
        public override bool Equals(object obj)
        {
            return obj is Color && this.Equals((Color)obj);
        }

        public bool Equals(Color other)
        {
            return this.PackedValue == other.PackedValue;
        }

        public override int GetHashCode()
        {
            return (int)this.PackedValue;
        }
        public Color(float r, float g, float b, float a)
        {
            this = new Color((byte)(MathUtils.Saturate(r) * 255f), (byte)(MathUtils.Saturate(g) * 255f), (byte)(MathUtils.Saturate(b) * 255f), (byte)(MathUtils.Saturate(a) * 255f));
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}", new object[] {
                this.R,
                this.G,
                this.B,
                this.A
            });
        }
    }
}
