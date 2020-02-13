using System;
using System.IO;
namespace Engine
{
    public class ModelDataContentReader
    {
        public static ModelData ReadModelData(Stream stream)
        {
            EngineBinaryReader engineBinaryReader = new EngineBinaryReader(stream, false);
            ModelData modelData = new ModelData();
            modelData.Bones.Capacity = engineBinaryReader.ReadInt32();
            for (int i = 0; i < modelData.Bones.Capacity; i++)
            {
                ModelBoneData modelBoneData = new ModelBoneData();
                modelData.Bones.Add(modelBoneData);
                modelBoneData.ParentBoneIndex = engineBinaryReader.ReadInt32();
                modelBoneData.Name = engineBinaryReader.ReadString();
                modelBoneData.Transform = engineBinaryReader.ReadMatrix();
            }




            modelData.Meshes.Capacity = engineBinaryReader.ReadInt32();
            for (int j = 0; j < modelData.Meshes.Capacity; j++)
            {
                ModelMeshData modelMeshData = new ModelMeshData();
                modelData.Meshes.Add(modelMeshData);
                modelMeshData.ParentBoneIndex = engineBinaryReader.ReadInt32();
                modelMeshData.Name = engineBinaryReader.ReadString();
                modelMeshData.MeshParts.Capacity = engineBinaryReader.ReadInt32();
                modelMeshData.BoundingBox = engineBinaryReader.ReadBoundingBox();
                for (int k = 0; k < modelMeshData.MeshParts.Capacity; k++)
                {
                    ModelMeshPartData modelMeshPartData = new ModelMeshPartData();
                    modelMeshData.MeshParts.Add(modelMeshPartData);
                    modelMeshPartData.BuffersDataIndex = engineBinaryReader.ReadInt32();
                    modelMeshPartData.StartIndex = engineBinaryReader.ReadInt32();
                    modelMeshPartData.IndicesCount = engineBinaryReader.ReadInt32();
                    modelMeshPartData.BoundingBox = engineBinaryReader.ReadBoundingBox();
                }
            }





            modelData.Buffers.Capacity = engineBinaryReader.ReadInt32();
            for (int l = 0; l < modelData.Buffers.Capacity; l++)
            {
                ModelBuffersData modelBuffersData = new ModelBuffersData();
                modelData.Buffers.Add(modelBuffersData);
                VertexElement[] array = new VertexElement[engineBinaryReader.ReadInt32()];
                for (int m = 0; m < array.Length; m++)
                {
                    array[m] = new VertexElement(engineBinaryReader.ReadInt32(), (VertexElementFormat)engineBinaryReader.ReadInt32(), engineBinaryReader.ReadString());
                }
                modelBuffersData.VertexDeclaration = new VertexDeclaration(array);
                modelBuffersData.Vertices = engineBinaryReader.ReadBytes(engineBinaryReader.ReadInt32());
                modelBuffersData.Indices = engineBinaryReader.ReadBytes(engineBinaryReader.ReadInt32());
            }







            return modelData;
        }
    }
}
