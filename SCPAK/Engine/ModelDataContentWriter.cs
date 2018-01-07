using System;
using System.IO;
namespace Engine
{
    public class ModelDataContentWriter
    {
        public static void WriteModelData(Stream stream, ModelData modelData, Vector3 scale)
        {
            Matrix m = Matrix.CreateScale(scale);
            EngineBinaryWriter engineBinaryWriter = new EngineBinaryWriter(stream, false);
            engineBinaryWriter.Write(modelData.Bones.Count);
            foreach (ModelBoneData current in modelData.Bones)
            {
                engineBinaryWriter.Write(current.ParentBoneIndex);
                engineBinaryWriter.Write(current.Name);
                engineBinaryWriter.Write((current.ParentBoneIndex < 0) ? (current.Transform * m) : current.Transform);
            }
            engineBinaryWriter.Write(modelData.Meshes.Count);
            foreach (ModelMeshData current2 in modelData.Meshes)
            {
                engineBinaryWriter.Write(current2.ParentBoneIndex);
                engineBinaryWriter.Write(current2.Name);
                engineBinaryWriter.Write(current2.MeshParts.Count);
                engineBinaryWriter.Write(current2.BoundingBox);
                foreach (ModelMeshPartData current3 in current2.MeshParts)
                {
                    engineBinaryWriter.Write(current3.BuffersDataIndex);
                    engineBinaryWriter.Write(current3.StartIndex);
                    engineBinaryWriter.Write(current3.IndicesCount);
                    engineBinaryWriter.Write(current3.BoundingBox);
                }
            }
            engineBinaryWriter.Write(modelData.Buffers.Count);
            foreach (ModelBuffersData current4 in modelData.Buffers)
            {
                engineBinaryWriter.Write(current4.VertexDeclaration.VertexElements.Count);
                foreach (VertexElement current5 in current4.VertexDeclaration.VertexElements)
                {
                    engineBinaryWriter.Write(current5.Offset);
                    engineBinaryWriter.Write((int)current5.Format);
                    engineBinaryWriter.Write(current5.Semantic);
                }
                engineBinaryWriter.Write(current4.Vertices.Length);
                engineBinaryWriter.Write(current4.Vertices);
                engineBinaryWriter.Write(current4.Indices.Length);
                engineBinaryWriter.Write(current4.Indices);
            }
        }
    }
}
