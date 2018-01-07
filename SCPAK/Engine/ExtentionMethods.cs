using System;
using System.Reflection;
namespace Engine
{
    public static class ExtentionMethods
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

        public static string GetSemanticString(this VertexElementSemantic semantic)
        {
            switch (semantic)
            {
                case VertexElementSemantic.Position:
                    return "POSITION";
                case VertexElementSemantic.Color:
                    return "COLOR";
                case VertexElementSemantic.Normal:
                    return "NORMAL";
                case VertexElementSemantic.TextureCoordinate:
                    return "TEXCOORD";
                case VertexElementSemantic.TextureCoordinate0:
                    return "TEXCOORD0";
                case VertexElementSemantic.TextureCoordinate1:
                    return "TEXCOORD1";
                case VertexElementSemantic.TextureCoordinate2:
                    return "TEXCOORD2";
                case VertexElementSemantic.TextureCoordinate3:
                    return "TEXCOORD3";
                case VertexElementSemantic.TextureCoordinate4:
                    return "TEXCOORD4";
                case VertexElementSemantic.TextureCoordinate5:
                    return "TEXCOORD5";
                case VertexElementSemantic.TextureCoordinate6:
                    return "TEXCOORD6";
                case VertexElementSemantic.TextureCoordinate7:
                    return "TEXCOORD7";
                case VertexElementSemantic.Instance:
                    return "INSTANCE";
                case VertexElementSemantic.BlendIndices:
                    return "BLENDINDICES";
                case VertexElementSemantic.BlendWeights:
                    return "BLENDWEIGHTS";
                default:
                    throw new InvalidOperationException("Unrecognized vertex semantic.");
            }
        }

        public static int GetSize(this VertexElementFormat format)
        {
            return format.GetElementsCount() * format.GetElementSize();
        }
    }
}
