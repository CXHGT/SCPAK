using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
namespace SCPAK
{
    public class ModelExporter
    {
        public const string COLLADA = "http://www.collada.org/2005/11/COLLADASchema";

        public const string INSTANCE = "http://www.w3.org/2001/XMLSchema-instance";

        private XNamespace colladaNS = "http://www.collada.org/2005/11/COLLADASchema";

        private XNamespace instanceNS = "http://www.w3.org/2001/XMLSchema-instance";

        private XDocument document;

        private XElement root;

        private XElement visualScene;
        public ModelExporter(ModelData modelData)
        {
            this.document = new XDocument(new XDeclaration("1.0", "UTF-8", null), new object[0]);
            this.root = new XElement(this.colladaNS + "COLLADA", new object[]
            {
                new XAttribute(XNamespace.Xmlns + "xsi", this.instanceNS),
                new XAttribute("version", "1.4.1"),
                new XElement(this.colladaNS + "asset", new object[]
                {
                    new XElement(this.colladaNS + "contributor", new object[]
                    {
                        new XElement(this.colladaNS + "author", "Survivalcraft Moder"),
                        new XElement(this.colladaNS + "authoring_tool", "Engine 0.0.0")
                    }),
                    new XElement(this.colladaNS + "created", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:00")),
                    new XElement(this.colladaNS + "modified", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:00")),
                    new XElement(this.colladaNS + "up_axis", "Y_UP")
                }),
                new XElement(this.colladaNS + "library_images"),
                new XElement(this.colladaNS + "library_effects"),
                new XElement(this.colladaNS + "library_materials"),
                new XElement(this.colladaNS + "library_geometries"),
                new XElement(this.colladaNS + "library_controllers"),
                new XElement(this.colladaNS + "library_visual_scenes"),
                new XElement(this.colladaNS + "scene")
            });
            this.visualScene = this.InitVisualScene("Scene");
            this.document.Add(this.root);

            XElement[] array = new XElement[modelData.Bones.Count];
            for (int i = 0; i < array.Length; i++)
            {
                ModelBoneData modelBoneData = modelData.Bones[i];
                array[i] = this.GetNode(modelBoneData.Name, modelBoneData.Transform);
            }
            for (int j = 0; j < array.Length; j++)
            {
                ModelBoneData modelBoneData2 = modelData.Bones[j];
                if (modelBoneData2.ParentBoneIndex != -1)
                {
                    array[modelBoneData2.ParentBoneIndex].Add(array[j]);
                }
                else
                {
                    this.visualScene.Add(array[j]);
                }
            }
            foreach (ModelMeshData modelMeshData in modelData.Meshes)
            {
                XElement geometry = this.GetGeometry(modelData, modelMeshData);
                array[modelMeshData.ParentBoneIndex].Add(this.GetGeometryInstance(modelMeshData.Name, geometry));
            }
        }

        public void Save(Stream stream)
        {
            this.document.Save(stream);
        }

        private XElement GetGeometry(ModelData model, ModelMeshData data)
        {
            ModelMeshPartData modelMeshPartData = data.MeshParts[0];
            ModelBoneData modelBoneData = model.Bones[data.ParentBoneIndex];
            string name = data.Name;
            ModelBuffersData modelBuffersData = model.Buffers[modelMeshPartData.BuffersDataIndex];
            int indicesCount = modelMeshPartData.IndicesCount;
            VertexDeclaration vertexDeclaration = modelBuffersData.VertexDeclaration;
            byte[] array = new byte[indicesCount * 32];
            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(modelBuffersData.Indices)))
            {
                binaryReader.BaseStream.Position = (long)(modelMeshPartData.StartIndex * 2);
                for (int i = 0; i < indicesCount; i++)
                {
                    short num = binaryReader.ReadInt16();
                    Buffer.BlockCopy(modelBuffersData.Vertices, (int)(num * 32), array, i * 32, 32);
                }
            }
            List<Vector3> list = new List<Vector3>();
            List<Vector3> list2 = new List<Vector3>();
            float[] array2 = new float[indicesCount * 2];
            int[] array3 = new int[indicesCount * 3];
            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(array)))
            {
                foreach (VertexElement vertexElement in vertexDeclaration.VertexElements)
                {
                    if (vertexElement.Semantic.StartsWith("POSITION"))
                    {
                        Vector3[] array4 = new Vector3[3];
                        for (int j = 0; j < indicesCount; j++)
                        {
                            binaryReader.BaseStream.Position = (long)(vertexDeclaration.VertexStride * j + vertexElement.Offset);
                            Vector3 vector = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            if (!list.Contains(vector))
                            {
                                list.Add(vector);
                            }
                            int num2 = j % 3;
                            array4[num2] = vector;
                            if (num2 == 2)
                            {
                                array3[j * 3 - 6] = list.IndexOf(array4[0]);
                                array3[j * 3 - 3] = list.IndexOf(array4[2]);
                                array3[j * 3] = list.IndexOf(array4[1]);
                            }
                        }
                    }
                    else if (vertexElement.Semantic.StartsWith("NORMAL"))
                    {
                        Vector3[] array5 = new Vector3[3];
                        for (int k = 0; k < indicesCount; k++)
                        {
                            binaryReader.BaseStream.Position = (long)(vertexDeclaration.VertexStride * k + vertexElement.Offset);
                            Vector3 vector2 = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                            if (!list2.Contains(vector2))
                            {
                                list2.Add(vector2);
                            }
                            int num3 = k % 3;
                            array5[num3] = vector2;
                            if (num3 == 2)
                            {
                                array3[k * 3 - 5] = list2.IndexOf(array5[0]);
                                array3[k * 3 - 2] = list2.IndexOf(array5[2]);
                                array3[k * 3 + 1] = list2.IndexOf(array5[1]);
                            }
                        }
                    }
                    else if (vertexElement.Semantic.StartsWith("TEXCOORD"))
                    {
                        for (int l = 0; l < indicesCount; l++)
                        {
                            binaryReader.BaseStream.Position = (long)(vertexDeclaration.VertexStride * l + vertexElement.Offset);
                            array2[l * 2] = binaryReader.ReadSingle();
                            array2[l * 2 + 1] = 1f - binaryReader.ReadSingle();
                            if (l % 3 == 2)
                            {
                                array3[l * 3 - 4] = l - 2;
                                array3[l * 3 - 1] = l;
                                array3[l * 3 + 2] = l - 1;
                            }
                        }
                    }
                }
            }
            XName name2 = this.colladaNS + "mesh";
            object[] array6 = new object[5];
            XElement source = this.GetSourceArray(name, "-mesh-positions", string.Join(" ", list.ConvertAll<string>((Vector3 v) => string.Format("{0} {1} {2}", v.X.ToString("R"), v.Y.ToString("R"), v.Z.ToString("R")))), list.Count * 3, 3, this.XYZParam());
            array6[0] = source;
            XElement source2 = this.GetSourceArray(name, "-mesh-normals", string.Join(" ", list2.ConvertAll<string>((Vector3 v) => string.Format("{0} {1} {2}", v.X.ToString("R"), v.Y.ToString("R"), v.Z.ToString("R")))), list2.Count * 3, 3, this.XYZParam());
            array6[1] = source2;
            XElement source3 = this.GetSourceArray(name, "-mesh-map", string.Join(" ", from f in array2 select f.ToString("R")), array2.Length, 2, this.STParam());
            array6[2] = source3;
            XElement source4 = new XElement(this.colladaNS + "vertices", new object[]
            {
                new XAttribute("id", name + "-mesh-vertices"),
                this.GetInput("POSITION", source)
            });
            array6[3] = source4;
            array6[4] = new XElement(this.colladaNS + "triangles", new object[]
            {
                new XAttribute("count", indicesCount / 3),
                this.GetInput("VERTEX", source4, 0),
                this.GetInput("NORMAL", source2, 1),
                this.GetInput("TEXCOORD", source3, 2),
                new XElement(this.colladaNS + "p", string.Join<int>(" ", array3))
            });
            XElement xelement = new XElement(name2, array6);
            XElement result;
            this.root.Element(this.colladaNS + "library_geometries").Add(result = new XElement(this.colladaNS + "geometry", new object[]
            {
                new XAttribute("id", name + "-mesh"),
                new XAttribute("name", name),
                xelement
            }));
            return result;
        }
        private XElement GetSourceArray(string id, string type, string array, int count, int stride, params XElement[] param)
        {
            return new XElement(this.colladaNS + "source", new object[]
            {
                new XAttribute("id", id + type),
                new XElement(this.colladaNS + "float_array", new object[]
                {
                    new XAttribute("id", id + type + "-array"),
                    new XAttribute("count", count),
                    array
                }),
                new XElement(this.colladaNS + "technique_common", new XElement(this.colladaNS + "accessor", new object[]
                {
                    new XAttribute("source", string.Format("#{0}{1}-array", id, type)),
                    new XAttribute("count", count / stride),
                    new XAttribute("stride", stride),
                    param
                }))
            });
        }
        private XElement[] XYZParam()
        {
            return new XElement[]
            {
                new XElement(this.colladaNS + "param", new object[]
                {
                    new XAttribute("name", "X"),
                    new XAttribute("type", "float")
                }),
                new XElement(this.colladaNS + "param", new object[]
                {
                    new XAttribute("name", "Y"),
                    new XAttribute("type", "float")
                }),
                new XElement(this.colladaNS + "param", new object[]
                {
                    new XAttribute("name", "Z"),
                    new XAttribute("type", "float")
                })
            };
        }

        private XElement[] STParam()
        {
            return new XElement[]
            {
                new XElement(this.colladaNS + "param", new object[]
                {
                    new XAttribute("name", "S"),
                    new XAttribute("type", "float")
                }),
                new XElement(this.colladaNS + "param", new object[]
                {
                    new XAttribute("name", "T"),
                    new XAttribute("type", "float")
                })
            };
        }

        private XElement GetInput(string semantic, XElement source)
        {
            return new XElement(this.colladaNS + "input", new object[]
            {
                new XAttribute("semantic", semantic),
                new XAttribute("source", "#" + source.Attribute("id").Value)
            });
        }

        private XElement GetInput(string semantic, XElement source, int offset)
        {
            return new XElement(this.colladaNS + "input", new object[]
            {
                new XAttribute("semantic", semantic),
                new XAttribute("source", "#" + source.Attribute("id").Value),
                new XAttribute("offset", offset)
            });
        }

        private XElement InitVisualScene(string name)
        {
            XElement result;
            this.root.Element(this.colladaNS + "library_visual_scenes").Add(result = new XElement(this.colladaNS + "visual_scene", new object[]
            {
                new XAttribute("id", name),
                new XAttribute("name", name)
            }));
            this.root.Element(this.colladaNS + "scene").Add(new XElement(this.colladaNS + "instance_visual_scene", new XAttribute("url", "#" + name)));
            return result;
        }

        private XElement GetGeometryInstance(string id, XElement geometry)
        {
            return new XElement(this.colladaNS + "instance_geometry", new object[]
            {
                new XAttribute("url", "#" + geometry.Attribute("id").Value),
                new XAttribute("name", id)
            });
        }

        private XElement GetNode(string id, Matrix transform)
        {
            return new XElement(this.colladaNS + "node", new object[]
            {
                new XAttribute("id", id),
                new XAttribute("name", id),
                new XAttribute("type", "NODE"),
                new XElement(this.colladaNS + "matrix", new object[]
                {
                    new XAttribute("sid", "transform"),
                    string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15}", new object[]
                    {
                        transform.M11,
                        transform.M21,
                        transform.M31,
                        transform.M41,
                        transform.M12,
                        transform.M22,
                        transform.M32,
                        transform.M42,
                        transform.M13,
                        transform.M23,
                        transform.M33,
                        transform.M43,
                        transform.M14,
                        transform.M24,
                        transform.M34,
                        transform.M44
                    })
                })
            });
        }
    }
}