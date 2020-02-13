using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public struct BoundingBox
    {
        public Vector3 Min;

        public Vector3 Max;

        public BoundingBox(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            this.Min = new Vector3(x1, y1, z1);
            this.Max = new Vector3(x2, y2, z2);
        }

        public BoundingBox(Vector3 Min,Vector3 Max)
        {
            this.Min = Min;
            this.Max = Max;
        }

        public static BoundingBox Union(BoundingBox b, Vector3 p)
        {
            Vector3 arg_1A_0 = Vector3.Min(b.Min, p);
            Vector3 max = Vector3.Max(b.Max, p);
            return new BoundingBox(arg_1A_0, max);
        }

        public static BoundingBox Union(BoundingBox b1, BoundingBox b2)
        {
            Vector3 arg_24_0 = Vector3.Min(b1.Min, b2.Min);
            Vector3 max = Vector3.Max(b1.Max, b2.Max);
            return new BoundingBox(arg_24_0, max);
        }
    }
}
