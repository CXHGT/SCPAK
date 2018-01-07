using System;
using System.IO;
namespace Engine
{
    public class ModelContentReader
    {
        public static ModelData Read(Stream s, out bool keepSourceVertexDataInTags)
        {
            keepSourceVertexDataInTags = new BinaryReader(s).ReadBoolean();
            return ModelDataContentReader.ReadModelData(s);
        }
    }
}
