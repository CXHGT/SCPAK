using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public static class ExtensionMethods
    {
        public static int GetElementsCount(this VertexElementFormat format)
        {
            switch (format)
            {
                case VertexElementFormat.Single:
                    return 1;
                case VertexElementFormat.Vector2:
                    return 2;
                case VertexElementFormat.Vector3:
                    return 3;
                case VertexElementFormat.Vector4:
                    return 4;
                case VertexElementFormat.Byte4:
                    return 4;
                case VertexElementFormat.NormalizedByte4:
                    return 4;
                case VertexElementFormat.Short2:
                    return 2;
                case VertexElementFormat.NormalizedShort2:
                    return 2;
                case VertexElementFormat.Short4:
                    return 4;
                case VertexElementFormat.NormalizedShort4:
                    return 4;
                default:
                    throw new InvalidOperationException("Unsupported VertexElementFormat.");
            }
        }

        public static int GetElementSize(this VertexElementFormat format)
        {
            switch (format)
            {
                case VertexElementFormat.Single:
                    return 4;
                case VertexElementFormat.Vector2:
                    return 4;
                case VertexElementFormat.Vector3:
                    return 4;
                case VertexElementFormat.Vector4:
                    return 4;
                case VertexElementFormat.Byte4:
                    return 1;
                case VertexElementFormat.NormalizedByte4:
                    return 1;
                case VertexElementFormat.Short2:
                    return 2;
                case VertexElementFormat.NormalizedShort2:
                    return 2;
                case VertexElementFormat.Short4:
                    return 2;
                case VertexElementFormat.NormalizedShort4:
                    return 2;
                default:
                    throw new InvalidOperationException("Unsupported VertexElementFormat.");
            }
        }

        public static int GetSize(this VertexElementFormat format)
        {
            return format.GetElementsCount() * format.GetElementSize();
        }
    }
}
