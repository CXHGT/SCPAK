using System;
using System.IO;
using System.Collections.Generic;
namespace Engine
{
    public class ModelData
    {
        public List<ModelBoneData> Bones = new List<ModelBoneData>();

        public List<ModelMeshData> Meshes = new List<ModelMeshData>();

        public List<ModelBuffersData> Buffers = new List<ModelBuffersData>();
    }
}
