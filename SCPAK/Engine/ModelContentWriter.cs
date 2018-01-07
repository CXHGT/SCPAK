using System;
using System.IO;
namespace Engine
{
    public class ModelContentWriter
    {
        public static void Write(Stream stream, ModelData data, Vector3 scale, bool keepSourceVertexDataInTags = true)
        {
            new BinaryWriter(stream).Write(keepSourceVertexDataInTags);
            ModelDataContentWriter.WriteModelData(stream, data, scale);
        }
    }
}
