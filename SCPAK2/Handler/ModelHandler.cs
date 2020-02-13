using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SCPAK
{
    public static class ModelHandler
    {

        public static void WriteModel(Stream mainStream, Stream daeStream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(mainStream, Encoding.UTF8, true);
            binaryWriter.Write(true);
            Matrix m = Matrix.CreateScale(new Vector3(1f,1f,1f));
            ModelData modelData = Collada.Load(daeStream);
            binaryWriter.Write(modelData.Bones.Count);
            foreach (ModelBoneData current in modelData.Bones)
            {
                Matrix matrix = (current.ParentBoneIndex < 0) ? (current.Transform * m) : current.Transform;
                binaryWriter.Write(current.ParentBoneIndex);
                binaryWriter.Write(current.Name);
                binaryWriter.Write(matrix.M11);
                binaryWriter.Write(matrix.M12);
                binaryWriter.Write(matrix.M13);
                binaryWriter.Write(matrix.M14);
                binaryWriter.Write(matrix.M21);
                binaryWriter.Write(matrix.M22);
                binaryWriter.Write(matrix.M23);
                binaryWriter.Write(matrix.M24);
                binaryWriter.Write(matrix.M31);
                binaryWriter.Write(matrix.M32);
                binaryWriter.Write(matrix.M33);
                binaryWriter.Write(matrix.M34);
                binaryWriter.Write(matrix.M41);
                binaryWriter.Write(matrix.M42);
                binaryWriter.Write(matrix.M43);
                binaryWriter.Write(matrix.M44);
            }
            binaryWriter.Write(modelData.Meshes.Count);
            foreach (ModelMeshData current2 in modelData.Meshes)
            {
                binaryWriter.Write(current2.ParentBoneIndex);
                binaryWriter.Write(current2.Name);
                binaryWriter.Write(current2.MeshParts.Count);
                binaryWriter.Write(current2.BoundingBox.Min.X);
                binaryWriter.Write(current2.BoundingBox.Min.Y);
                binaryWriter.Write(current2.BoundingBox.Min.Z);
                binaryWriter.Write(current2.BoundingBox.Max.X);
                binaryWriter.Write(current2.BoundingBox.Max.Y);
                binaryWriter.Write(current2.BoundingBox.Max.Z);
                foreach (ModelMeshPartData current3 in current2.MeshParts)
                {
                    binaryWriter.Write(current3.BuffersDataIndex);
                    binaryWriter.Write(current3.StartIndex);
                    binaryWriter.Write(current3.IndicesCount);
                    binaryWriter.Write(current3.BoundingBox.Min.X);
                    binaryWriter.Write(current3.BoundingBox.Min.Y);
                    binaryWriter.Write(current3.BoundingBox.Min.Z);
                    binaryWriter.Write(current3.BoundingBox.Max.X);
                    binaryWriter.Write(current3.BoundingBox.Max.Y);
                    binaryWriter.Write(current3.BoundingBox.Max.Z);
                }
            }
            binaryWriter.Write(modelData.Buffers.Count);
            foreach (ModelBuffersData current4 in modelData.Buffers)
            {
                binaryWriter.Write(current4.VertexDeclaration.VertexElements.Length);
                foreach (VertexElement current5 in current4.VertexDeclaration.VertexElements)
                {
                    binaryWriter.Write(current5.Offset);
                    binaryWriter.Write((int)current5.Format);
                    binaryWriter.Write(current5.Semantic);
                }
                binaryWriter.Write(current4.Vertices.Length);
                binaryWriter.Write(current4.Vertices);
                binaryWriter.Write(current4.Indices.Length);
                binaryWriter.Write(current4.Indices);
            }
        }

        public static void RecoverModel(Stream targetFileStream, Stream modelStream)
        {
            ModelExporter modelExporter = new ModelExporter(new ModelData(modelStream));
            modelExporter.Save(targetFileStream);
        }
    }
}
