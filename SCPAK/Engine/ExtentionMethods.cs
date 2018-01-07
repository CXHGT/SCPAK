using System;
using System.Reflection;
namespace Engine
{
    public static class ExtensionMethods
    {
        //
        // Static Methods
        //
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

        //public static int GetPrimitivesCount(this PrimitiveType primitiveType, int indicesCount)
        //{
        //    switch (primitiveType)
        //    {
        //        case PrimitiveType.LineList:
        //            return indicesCount / 2;
        //        case PrimitiveType.LineStrip:
        //            return MathUtils.Max(indicesCount - 1, 0);
        //        case PrimitiveType.TriangleList:
        //            return indicesCount / 3;
        //        case PrimitiveType.TriangleStrip:
        //            return MathUtils.Max(indicesCount - 2, 0);
        //        default:
        //            throw new InvalidOperationException("Unsupported PrimitiveType.");
        //    }
        //}

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

        //public static int GetSize(this ColorFormat format)
        //{
        //    switch (format)
        //    {
        //        case ColorFormat.Rgba8888:
        //            return 4;
        //        case ColorFormat.Rgba5551:
        //            return 2;
        //        case ColorFormat.Rgb565:
        //            return 2;
        //        default:
        //            throw new InvalidOperationException("Unsupported ColorFormat.");
        //    }
        //}

        //public static int GetSize(this DepthFormat format)
        //{
        //    switch (format)
        //    {
        //        case DepthFormat.None:
        //            return 0;
        //        case DepthFormat.Depth16:
        //            return 2;
        //        case DepthFormat.Depth24Stencil8:
        //            return 4;
        //        default:
        //            throw new InvalidOperationException("Unsupported DepthFormat.");
        //    }
        //}

        //public static int GetSize(this IndexFormat format)
        //{
        //    if (format == IndexFormat.SixteenBits)
        //    {
        //        return 2;
        //    }
        //    if (format != IndexFormat.ThirtyTwoBits)
        //    {
        //        throw new InvalidOperationException("Unsupported IndexFormat.");
        //    }
        //    return 4;
        //}

        //public static int GetSize(this ShaderParameterType type)
        //{
        //    switch (type)
        //    {
        //        case ShaderParameterType.Float:
        //            return 4;
        //        case ShaderParameterType.Vector2:
        //            return 8;
        //        case ShaderParameterType.Vector3:
        //            return 12;
        //        case ShaderParameterType.Vector4:
        //            return 16;
        //        case ShaderParameterType.Matrix:
        //            return 64;
        //        default:
        //            throw new InvalidOperationException("Unsupported ShaderParameterType.");
        //    }
        //}

        public static int GetSize(this VertexElementFormat format)
        {
            return format.GetElementsCount() * format.GetElementSize();
        }
    }
}
