using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SCPAK
{
    public class ModelData
    {
        public static readonly XNamespace Namespace = "http://www.collada.org/2005/11/COLLADASchema";

        public List<ModelBoneData> Bones = new List<ModelBoneData>();

        public List<ModelMeshData> Meshes = new List<ModelMeshData>();

        public List<ModelBuffersData> Buffers = new List<ModelBuffersData>();

        public ModelData()
        {
        }

        public ModelData(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8);
            binaryReader.ReadBoolean();
            this.Bones.Capacity = binaryReader.ReadInt32();
            for (int i = 0; i < this.Bones.Capacity; i++)
            {
                ModelBoneData modelBoneData = new ModelBoneData();
                this.Bones.Add(modelBoneData);
                modelBoneData.ParentBoneIndex = binaryReader.ReadInt32();
                modelBoneData.Name = binaryReader.ReadString();
                modelBoneData.Transform = new Matrix(
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle(), 
                    binaryReader.ReadSingle());
            }
            this.Meshes.Capacity = binaryReader.ReadInt32();
            for (int j = 0; j < this.Meshes.Capacity; j++)
            {
                ModelMeshData modelMeshData = new ModelMeshData();
                this.Meshes.Add(modelMeshData);
                modelMeshData.ParentBoneIndex = binaryReader.ReadInt32();
                modelMeshData.Name = binaryReader.ReadString();
                modelMeshData.MeshParts.Capacity = binaryReader.ReadInt32();
                modelMeshData.BoundingBox = new BoundingBox(
                    new Vector3(binaryReader.ReadSingle(),binaryReader.ReadSingle(),binaryReader.ReadSingle()),
                    new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()));
                for (int k = 0; k < modelMeshData.MeshParts.Capacity; k++)
                {
                    ModelMeshPartData modelMeshPartData = new ModelMeshPartData();
                    modelMeshData.MeshParts.Add(modelMeshPartData);
                    modelMeshPartData.BuffersDataIndex = binaryReader.ReadInt32();
                    modelMeshPartData.StartIndex = binaryReader .ReadInt32();
                    modelMeshPartData.IndicesCount = binaryReader.ReadInt32();
                    modelMeshPartData.BoundingBox = new BoundingBox(
                        new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()),
                        new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle()));
                }
            }
            this.Buffers.Capacity = binaryReader.ReadInt32();
            for (int l = 0; l < this.Buffers.Capacity; l++)
            {
                ModelBuffersData modelBuffersData = new ModelBuffersData();
                this.Buffers.Add(modelBuffersData);
                VertexElement[] array = new VertexElement[binaryReader.ReadInt32()];
                for (int m = 0; m < array.Length; m++)
                {
                    array[m] = new VertexElement(binaryReader.ReadInt32(), (VertexElementFormat)binaryReader.ReadInt32(), binaryReader.ReadString());
                }
                modelBuffersData.VertexDeclaration = new VertexDeclaration(array);
                modelBuffersData.Vertices = binaryReader.ReadBytes(binaryReader.ReadInt32());
                modelBuffersData.Indices = binaryReader.ReadBytes(binaryReader.ReadInt32());
            }
        }
    }
}