using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public struct Color
    {
        public uint PackedValue;
        public Color(byte r, byte g, byte b, byte a)
        {
            this.PackedValue = (uint)((int)a << 24 | (int)b << 16 | (int)g << 8 | (int)r);
        }
        public Color(float r, float g, float b, float a)
        {
            this = new Color((byte)(Saturate(r) * 255f), (byte)(Saturate(g) * 255f), (byte)(Saturate(b) * 255f), (byte)(Saturate(a) * 255f));
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
    }
}
