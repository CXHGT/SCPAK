using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
namespace Engine

{
    public class ColladaExporter
    {
        public const string COLLADA = "http://www.collada.org/2005/11/COLLADASchema";
        public const string INSTANCE = "http://www.w3.org/2001/XMLSchema-instance";
        XNamespace colladaNS = COLLADA;
        XNamespace instanceNS = INSTANCE;
        XDocument document;

        XElement root;
        XElement visualScene;

        public static void Import(Stream input, Stream output)
        {
            ModelContentWriter.Write(output, Collada.Load(input), Vector3.One);
        }

        public static void Export(Stream input, Stream output)
        {
            var exporter = new ColladaExporter();
            exporter.AddModel(ModelContentReader.Read(input, out bool keepTags));
            exporter.Save(output);
        }

        public ColladaExporter()
        {
            document = new XDocument(
                new XDeclaration("1.0", "UTF-8", null)
            );

            root = new XElement(
                colladaNS + "COLLADA",
                new XAttribute(XNamespace.Xmlns + "xsi", instanceNS),
                new XAttribute("version", "1.4.1"),

                new XElement(
                colladaNS + "asset",
                new XElement(colladaNS + "contributor",
                             new XElement(colladaNS + "author", "Survivalcraft Moder"),
                             new XElement(colladaNS + "authoring_tool", "Engine 0.0.0")),
                new XElement(colladaNS + "created", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:00")),
                new XElement(colladaNS + "modified", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:00")),
                new XElement(colladaNS + "up_axis", "Y_UP")),

                new XElement(colladaNS + "library_images"),
                new XElement(colladaNS + "library_effects"),
                new XElement(colladaNS + "library_materials"),
                new XElement(colladaNS + "library_geometries"),
                new XElement(colladaNS + "library_controllers"),
                new XElement(colladaNS + "library_visual_scenes"),
                new XElement(colladaNS + "scene")
            );

            visualScene = InitVisualScene("Scene");

            document.Add(root);
        }

        public void Save(Stream stream)
        {
            document.Save(stream);
        }

        public void AddModel(ModelData model)
        {
            XElement[] nodes = new XElement[model.Bones.Count];
            for (int i = 0; i < nodes.Length; i++)
            {
                var data = model.Bones[i];
                nodes[i] = GetNode(data.Name, data.Transform);
            }
            for (int i = 0; i < nodes.Length; i++)
            {
                var data = model.Bones[i];
                if (data.ParentBoneIndex != -1)
                    nodes[data.ParentBoneIndex].Add(nodes[i]);
                else
                    visualScene.Add(nodes[i]);
            }

            foreach (ModelMeshData data in model.Meshes)
            {
                var geometry = GetGeometry(model, data);
                nodes[data.ParentBoneIndex].Add(GetGeometryInstance(data.Name, geometry));
            }
        }

        XElement GetGeometry(ModelData model, ModelMeshData data)
        {
            ModelMeshPartData meshPart = data.MeshParts[0];
            ModelBoneData boneData = model.Bones[data.ParentBoneIndex];
            string meshId = data.Name;

            var vertexBuffer = model.Buffers[meshPart.BuffersDataIndex];
            int count = meshPart.IndicesCount;
            var vertexDeclaration = vertexBuffer.VertexDeclaration;

            byte[] original = new byte[count * 32];
            using (BinaryReader reader = new BinaryReader(new MemoryStream(vertexBuffer.Indices)))
            {
                reader.BaseStream.Position = meshPart.StartIndex * 2;
                for (int i = 0; i < count; i++)
                {
                    short index = reader.ReadInt16();
                    Buffer.BlockCopy(vertexBuffer.Vertices, index * 32, original, i * 32, 32);
                }
            }

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            float[] textureCord = new float[count * 2];
            int[] indexs = new int[count * 3];
            using (EngineBinaryReader reader = new EngineBinaryReader(new MemoryStream(original)))
            {
                foreach (VertexElement elem in vertexDeclaration.VertexElements)
                {
                    if (elem.Semantic.StartsWith("POSITION"))
                    {
                        Vector3[] triangle = new Vector3[3];
                        for (int i = 0; i < count; i++)
                        {
                            reader.BaseStream.Position = vertexDeclaration.VertexStride * i + elem.Offset;
                            var p = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                            if (!vertices.Contains(p))
                                vertices.Add(p);
                            var ii = i % 3;
                            triangle[ii] = p;
                            if (ii == 2)
                            {
                                indexs[i * 3 - 6] = vertices.IndexOf(triangle[0]);
                                indexs[i * 3 - 3] = vertices.IndexOf(triangle[2]);
                                indexs[i * 3] = vertices.IndexOf(triangle[1]);
                            }
                        }
                    }
                    else if (elem.Semantic.StartsWith("NORMAL"))
                    {
                        Vector3[] triangle = new Vector3[3];
                        for (int i = 0; i < count; i++)
                        {
                            reader.BaseStream.Position = vertexDeclaration.VertexStride * i + elem.Offset;
                            var p = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                            if (!normals.Contains(p))
                                normals.Add(p);
                            var ii = i % 3;
                            triangle[ii] = p;
                            if (ii == 2)
                            {
                                indexs[i * 3 - 5] = normals.IndexOf(triangle[0]);
                                indexs[i * 3 - 2] = normals.IndexOf(triangle[2]);
                                indexs[i * 3 + 1] = normals.IndexOf(triangle[1]);
                            }
                        }
                    }
                    else if (elem.Semantic.StartsWith("TEXCOORD"))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            reader.BaseStream.Position = vertexDeclaration.VertexStride * i + elem.Offset;
                            textureCord[i * 2] = reader.ReadSingle();
                            textureCord[i * 2 + 1] = 1f - reader.ReadSingle();
                            if (i % 3 == 2)
                            {
                                indexs[i * 3 - 4] = i - 2;
                                indexs[i * 3 - 1] = i;
                                indexs[i * 3 + 2] = i - 1;
                            }
                        }
                    }
                }
            }

            XElement positionSource;

            XElement normalSource;

            XElement texturecoorSource;

            XElement vertexSource;

            XElement meshElem = new XElement(
                colladaNS + "mesh",
                positionSource = GetSourceArray(
                    meshId,
                    "-mesh-positions",
                    string.Join(" ", vertices.ConvertAll(v => string.Format("{0} {1} {2}", v.X.ToString("R"), v.Y.ToString("R"), v.Z.ToString("R")))),
                    vertices.Count * 3,
                    3,
                    XYZParam()
                ),
                normalSource = GetSourceArray(
                    meshId,
                    "-mesh-normals",
                    string.Join(" ", normals.ConvertAll(v => string.Format("{0} {1} {2}", v.X.ToString("R"), v.Y.ToString("R"), v.Z.ToString("R")))),
                    normals.Count * 3,
                    3,
                    XYZParam()
                ),
                texturecoorSource = GetSourceArray(
                    meshId,
                    "-mesh-map",
                    string.Join(" ", textureCord.Select(f => f.ToString("R"))),
                    textureCord.Length,
                    2,
                    STParam()
                ),
                vertexSource = new XElement(
                    colladaNS + "vertices",
                    new XAttribute("id", meshId + "-mesh-vertices"),
                    GetInput("POSITION", positionSource)
                ),
                new XElement(
                    colladaNS + "triangles",
                    new XAttribute("count", count / 3),
                    GetInput("VERTEX", vertexSource, 0),
                    GetInput("NORMAL", normalSource, 1),
                    GetInput("TEXCOORD", texturecoorSource, 2),
                    new XElement(colladaNS + "p", string.Join(" ", indexs))
                )
            );

            XElement geometry;

            root.Element(colladaNS + "library_geometries").Add(
                geometry = new XElement(
                    colladaNS + "geometry",
                    new XAttribute("id", meshId + "-mesh"),
                    new XAttribute("name", meshId),
                    meshElem
                )
            );

            return geometry;
        }

        XElement GetSourceArray(string id, string type, string array, int count, int stride, params XElement[] param)
        {
            return new XElement(
                colladaNS + "source",
                new XAttribute("id", id + type),
                new XElement(
                    colladaNS + "float_array",
                    new XAttribute("id", id + type + "-array"),
                    new XAttribute("count", count),
                    array
                ),
                new XElement(
                    colladaNS + "technique_common",
                    new XElement(
                        colladaNS + "accessor",
                        new XAttribute("source", string.Format("#{0}{1}-array", id, type)),
                        new XAttribute("count", count / stride),
                        new XAttribute("stride", stride),
                        param
                    )
                )
            );
        }

        XElement[] XYZParam()
        {
            return new XElement[]
            {
                new XElement(
                    colladaNS + "param",
                    new XAttribute("name", "X"),
                    new XAttribute("type", "float")
                ),
                new XElement(
                    colladaNS + "param",
                    new XAttribute("name", "Y"),
                    new XAttribute("type", "float")
                ),
                new XElement(
                    colladaNS + "param",
                    new XAttribute("name", "Z"),
                    new XAttribute("type", "float")
                )
            };
        }

        XElement[] STParam()
        {
            return new XElement[]
            {
                new XElement(
                    colladaNS + "param",
                    new XAttribute("name", "S"),
                    new XAttribute("type", "float")
                ),
                new XElement(
                    colladaNS + "param",
                    new XAttribute("name", "T"),
                    new XAttribute("type", "float")
                )
            };
        }

        XElement GetInput(string semantic, XElement source)
        {
            return new XElement(
                colladaNS + "input",
                new XAttribute("semantic", semantic),
                new XAttribute("source", "#" + source.Attribute("id").Value)
            );
        }

        XElement GetInput(string semantic, XElement source, int offset)
        {
            return new XElement(
                colladaNS + "input",
                new XAttribute("semantic", semantic),
                new XAttribute("source", "#" + source.Attribute("id").Value),
                new XAttribute("offset", offset)
            );
        }

        XElement InitVisualScene(string name)
        {
            XElement scene;
            root.Element(colladaNS + "library_visual_scenes")
                .Add(scene = new XElement(colladaNS + "visual_scene", new XAttribute("id", name), new XAttribute("name", name)));

            root.Element(colladaNS + "scene")
                .Add(new XElement(colladaNS + "instance_visual_scene", new XAttribute("url", "#" + name)));
            return scene;
        }

        XElement GetGeometryInstance(string id, XElement geometry)
        {
            return new XElement(
                colladaNS + "instance_geometry",
                new XAttribute("url", "#" + geometry.Attribute("id").Value),
                new XAttribute("name", id)
            );
        }

        XElement GetNode(string id, Matrix transform)
        {
            return new XElement(
                colladaNS + "node",
                new XAttribute("id", id),
                new XAttribute("name", id),
                new XAttribute("type", "NODE"),
                new XElement(
                    colladaNS + "matrix",
                    new XAttribute("sid", "transform"),
                    string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15}", new object[] {
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
                )
            );
        }
    }
}
