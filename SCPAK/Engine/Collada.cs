using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Globalization;
namespace Engine
{
    public static class Collada
    {
        //
        // Static Methods
        //
        static T[] ExtendArray<T>(T[] array, int extensionLength)
        {
            T[] array2 = new T[array.Length + extensionLength];
            Array.Copy(array, array2, array.Length);
            return array2;
        }

        static void IndexVertices(int vertexStride, byte[] vertices, out byte[] resultVertices, out byte[] resultIndices)
        {
            int num = vertices.Length / vertexStride;
            Dictionary<Vertex, ushort> dictionary = new Dictionary<Vertex, ushort>();
            resultIndices = new byte[2 * num];
            for (int i = 0; i < num; i++)
            {
                Vertex key = new Vertex(vertices, i * vertexStride, vertexStride);
                ushort num2;
                if (!dictionary.TryGetValue(key, out num2))
                {
                    num2 = (ushort)dictionary.Count;
                    dictionary.Add(key, num2);
                }
                resultIndices[i * 2] = (byte)num2;
                resultIndices[i * 2 + 1] = (byte)(num2 >> 8);
            }
            resultVertices = new byte[dictionary.Count * vertexStride];
            foreach (KeyValuePair<Vertex, ushort> current in dictionary)
            {
                Vertex key2 = current.Key;
                ushort value = current.Value;
                Array.Copy(key2.Data, key2.Start, resultVertices, (int)value * vertexStride, key2.Count);
            }
        }

        public static ModelData Load(Stream stream)
        {
            ModelData modelData = new ModelData();
            ColladaRoot colladaRoot = new ColladaRoot(XElement.Load(stream));
            if (colladaRoot.Scene.VisualScene.Nodes.Count > 1)
            {
                ModelBoneData modelBoneData = new ModelBoneData();
                modelData.Bones.Add(modelBoneData);
                modelBoneData.ParentBoneIndex = -1;
                modelBoneData.Name = string.Empty;
                modelBoneData.Transform = Matrix.Identity;
                using (List<ColladaNode>.Enumerator enumerator = colladaRoot.Scene.VisualScene.Nodes.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ColladaNode current = enumerator.Current;
                        LoadNode(modelData, modelBoneData, current, colladaRoot.Asset.Meter);
                    }
                    goto IL_F7;
                }
            }
            foreach (ColladaNode current2 in colladaRoot.Scene.VisualScene.Nodes)
            {
                LoadNode(modelData, null, current2, colladaRoot.Asset.Meter);
            }
        IL_F7:
            foreach (ModelBuffersData current3 in modelData.Buffers)
            {
                IndexVertices(current3.VertexDeclaration.VertexStride, current3.Vertices, out current3.Vertices, out current3.Indices);
            }
            return modelData;
        }

        static ModelMeshData LoadGeometry(ModelData data, ModelBoneData parentBoneData, ColladaGeometry geometry)
        {
            ModelMeshData modelMeshData = new ModelMeshData();
            data.Meshes.Add(modelMeshData);
            modelMeshData.Name = parentBoneData.Name;
            modelMeshData.ParentBoneIndex = data.Bones.IndexOf(parentBoneData);
            bool flag = false;
            foreach (ColladaPolygons current in geometry.Mesh.Polygons)
            {
                ModelMeshPartData modelMeshPartData = LoadPolygons(data, current);
                modelMeshData.MeshParts.Add(modelMeshPartData);
                modelMeshData.BoundingBox = (flag ? BoundingBox.Union(modelMeshData.BoundingBox, modelMeshPartData.BoundingBox) : modelMeshPartData.BoundingBox);
                flag = true;
            }
            return modelMeshData;
        }

        static ModelBoneData LoadNode(ModelData data, ModelBoneData parentBoneData, ColladaNode node, float meter)
        {
            ModelBoneData modelBoneData = new ModelBoneData();
            data.Bones.Add(modelBoneData);
            modelBoneData.ParentBoneIndex = ((parentBoneData != null) ? data.Bones.IndexOf(parentBoneData) : (-1));
            modelBoneData.Name = node.Name;
            modelBoneData.Transform = node.Transform * Matrix.CreateScale(meter);
            foreach (ColladaNode current in node.Nodes)
            {
                LoadNode(data, modelBoneData, current, 1f);
            }
            foreach (ColladaGeometry current2 in node.Geometries)
            {
                LoadGeometry(data, modelBoneData, current2);
            }
            return modelBoneData;
        }

        static ModelMeshPartData LoadPolygons(ModelData data, ColladaPolygons polygons)
        {
            c__DisplayClass3_0 c__DisplayClass3_ = new c__DisplayClass3_0();
            ModelMeshPartData modelMeshPartData = new ModelMeshPartData();
            int num = 0;
            Dictionary<VertexElement, ColladaInput> dictionary = new Dictionary<VertexElement, ColladaInput>();
            foreach (ColladaInput current in polygons.Inputs)
            {
                string str = (current.Set == 0) ? string.Empty : current.Set.ToString(CultureInfo.InvariantCulture);
                if (current.Semantic == "POSITION")
                {
                    dictionary[new VertexElement(num, VertexElementFormat.Vector3, "POSITION" + str)] = current;
                    num += 12;
                }
                else if (current.Semantic == "NORMAL")
                {
                    dictionary[new VertexElement(num, VertexElementFormat.Vector3, "NORMAL" + str)] = current;
                    num += 12;
                }
                else if (current.Semantic == "TEXCOORD")
                {
                    dictionary[new VertexElement(num, VertexElementFormat.Vector2, "TEXCOORD" + str)] = current;
                    num += 8;
                }
                else if (current.Semantic == "COLOR")
                {
                    dictionary[new VertexElement(num, VertexElementFormat.NormalizedByte4, "COLOR" + str)] = current;
                    num += 4;
                }
            }
            c__DisplayClass3_.vertexDeclaration = new VertexDeclaration(dictionary.Keys.ToArray<VertexElement>());
            ModelBuffersData modelBuffersData = data.Buffers.FirstOrDefault(new Func<ModelBuffersData, bool>(c__DisplayClass3_.b__0));
            if (modelBuffersData == null)
            {
                modelBuffersData = new ModelBuffersData();
                data.Buffers.Add(modelBuffersData);
                modelBuffersData.VertexDeclaration = c__DisplayClass3_.vertexDeclaration;
            }
            modelMeshPartData.BuffersDataIndex = data.Buffers.IndexOf(modelBuffersData);
            int num2 = polygons.P.Count / polygons.Inputs.Count;
            List<int> list = new List<int>();
            if (polygons.VCount.Count == 0)
            {
                int num3 = 0;
                for (int i = 0; i < num2 / 3; i++)
                {
                    list.Add(num3);
                    list.Add(num3 + 2);
                    list.Add(num3 + 1);
                    num3 += 3;
                }
            }
            else
            {
                int num4 = 0;
                foreach (int current2 in polygons.VCount)
                {
                    if (current2 == 3)
                    {
                        list.Add(num4);
                        list.Add(num4 + 2);
                        list.Add(num4 + 1);
                        num4 += 3;
                    }
                    else
                    {
                        if (current2 != 4)
                        {
                            throw new NotSupportedException("Collada polygons with less than 3 or more than 4 vertices are not supported.");
                        }
                        list.Add(num4);
                        list.Add(num4 + 2);
                        list.Add(num4 + 1);
                        list.Add(num4 + 2);
                        list.Add(num4);
                        list.Add(num4 + 3);
                        num4 += 4;
                    }
                }
            }
            int vertexStride = modelBuffersData.VertexDeclaration.VertexStride;
            int num5 = modelBuffersData.Vertices.Length;
            modelBuffersData.Vertices = ExtendArray<byte>(modelBuffersData.Vertices, list.Count * vertexStride);
            using (BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(modelBuffersData.Vertices, num5, list.Count * vertexStride)))
            {
                bool flag = false;
                foreach (KeyValuePair<VertexElement, ColladaInput> current3 in dictionary)
                {
                    VertexElement key = current3.Key;
                    ColladaInput value = current3.Value;
                    if (key.Semantic.StartsWith("POSITION"))
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            float[] arg_3EF_0 = value.Source.Accessor.Source.Array;
                            int offset = value.Source.Accessor.Offset;
                            int stride = value.Source.Accessor.Stride;
                            int num6 = polygons.P[list[j] * polygons.Inputs.Count + value.Offset];
                            binaryWriter.BaseStream.Position = (long)(j * vertexStride + key.Offset);
                            float num7 = arg_3EF_0[offset + stride * num6];
                            float num8 = arg_3EF_0[offset + stride * num6 + 1];
                            float num9 = arg_3EF_0[offset + stride * num6 + 2];
                            modelMeshPartData.BoundingBox = (flag ? BoundingBox.Union(modelMeshPartData.BoundingBox, new Vector3(num7, num8, num9)) : new BoundingBox(num7, num8, num9, num7, num8, num9));
                            flag = true;
                            binaryWriter.Write(num7);
                            binaryWriter.Write(num8);
                            binaryWriter.Write(num9);
                        }
                    }
                    else if (key.Semantic.StartsWith("NORMAL"))
                    {
                        for (int k = 0; k < list.Count; k++)
                        {
                            float[] arg_51E_0 = value.Source.Accessor.Source.Array;
                            int offset2 = value.Source.Accessor.Offset;
                            int stride2 = value.Source.Accessor.Stride;
                            int num10 = polygons.P[list[k] * polygons.Inputs.Count + value.Offset];
                            binaryWriter.BaseStream.Position = (long)(k * vertexStride + key.Offset);
                            float num11 = arg_51E_0[offset2 + stride2 * num10];
                            float num12 = arg_51E_0[offset2 + stride2 * num10 + 1];
                            float num13 = arg_51E_0[offset2 + stride2 * num10 + 2];
                            float num14 = 1f / MathUtils.Sqrt(num11 * num11 + num12 * num12 + num13 * num13);
                            binaryWriter.Write(num14 * num11);
                            binaryWriter.Write(num14 * num12);
                            binaryWriter.Write(num14 * num13);
                        }
                    }
                    else if (key.Semantic.StartsWith("TEXCOORD"))
                    {
                        for (int l = 0; l < list.Count; l++)
                        {
                            float[] array = value.Source.Accessor.Source.Array;
                            int offset3 = value.Source.Accessor.Offset;
                            int stride3 = value.Source.Accessor.Stride;
                            int num15 = polygons.P[list[l] * polygons.Inputs.Count + value.Offset];
                            binaryWriter.BaseStream.Position = (long)(l * vertexStride + key.Offset);
                            binaryWriter.Write(array[offset3 + stride3 * num15]);
                            binaryWriter.Write(1f - array[offset3 + stride3 * num15 + 1]);
                        }
                    }
                    else
                    {
                        if (!key.Semantic.StartsWith("COLOR"))
                        {
                            throw new Exception();
                        }
                        for (int m = 0; m < list.Count; m++)
                        {
                            float[] array2 = value.Source.Accessor.Source.Array;
                            int offset4 = value.Source.Accessor.Offset;
                            int stride4 = value.Source.Accessor.Stride;
                            int num16 = polygons.P[list[m] * polygons.Inputs.Count + value.Offset];
                            binaryWriter.BaseStream.Position = (long)(m * vertexStride + key.Offset);
                            Color color = new Color(array2[offset4 + stride4 * num16], array2[offset4 + stride4 * num16 + 1], array2[offset4 + stride4 * num16 + 2], array2[offset4 + stride4 * num16 + 3]);
                            binaryWriter.Write(color.PackedValue);
                        }
                    }
                }
            }
            modelMeshPartData.StartIndex = num5 / vertexStride;
            modelMeshPartData.IndicesCount = list.Count;
            return modelMeshPartData;
        }

        //
        // Nested Types
        //
        class Asset
        {
            public readonly float Meter = 1f;

            public Asset(XElement node)
            {
                XElement xElement = node.Element(ColladaRoot.Namespace + "unit");
                if (xElement != null)
                {
                    XAttribute xAttribute = xElement.Attribute("meter");
                    if (xAttribute != null)
                    {
                        Meter = float.Parse(xAttribute.Value, CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        sealed class c__DisplayClass3_0
        {
            public VertexDeclaration vertexDeclaration;

            internal bool b__0(ModelBuffersData vd)
            {
                return vd.VertexDeclaration == vertexDeclaration;
            }
        }

        class ColladaAccessor
        {
            public ColladaFloatArray Source;

            public int Offset;

            public int Stride = 1;

            public ColladaAccessor(ColladaRoot collada, XElement node)
            {
                Source = (ColladaFloatArray)collada.ObjectsById[node.Attribute("source").Value.Substring(1)];
                XAttribute xAttribute = node.Attribute("offset");
                if (xAttribute != null)
                {
                    Offset = int.Parse(xAttribute.Value, CultureInfo.InvariantCulture);
                }
                XAttribute xAttribute2 = node.Attribute("stride");
                if (xAttribute2 != null)
                {
                    Stride = int.Parse(xAttribute2.Value, CultureInfo.InvariantCulture);
                }
            }
        }

        class ColladaFloatArray : ColladaNameId
        {

            [Serializable]
            sealed class c
            {
                public static readonly ColladaFloatArray.c a = new ColladaFloatArray.c();

                public static Func<string, float> a__1_0;

                internal float b__1_0(string s)
                {
                    return float.Parse(s, CultureInfo.InvariantCulture);
                }
            }

            public float[] Array;

            public ColladaFloatArray(ColladaRoot collada, XElement node) : base(collada, node, "")
            {
                IEnumerable<string> arg_3A_0 = node.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                Func<string, float> arg_3A_1;
                if ((arg_3A_1 = ColladaFloatArray.c.a__1_0) == null)
                {
                    arg_3A_1 = (ColladaFloatArray.c.a__1_0 = new Func<string, float>(ColladaFloatArray.c.a.b__1_0));
                }
                Array = arg_3A_0.Select(arg_3A_1).ToArray<float>();
            }
        }

        class ColladaGeometry : ColladaNameId
        {
            public ColladaMesh Mesh;

            public ColladaGeometry(ColladaRoot collada, XElement node) : base(collada, node, "")
            {
                XElement xElement = node.Element(ColladaRoot.Namespace + "mesh");
                if (xElement != null)
                {
                    Mesh = new ColladaMesh(collada, xElement);
                }
            }
        }

        class ColladaInput
        {
            public int Offset;

            public string Semantic;

            public int Set;

            public ColladaSource Source;

            public ColladaInput(ColladaRoot collada, XElement node)
            {
                Offset = int.Parse(node.Attribute("offset").Value, CultureInfo.InvariantCulture);
                Semantic = node.Attribute("semantic").Value;
                XAttribute xAttribute = node.Attribute("set");
                if (xAttribute != null)
                {
                    Set = int.Parse(xAttribute.Value, CultureInfo.InvariantCulture);
                }
                ColladaNameId colladaNameId = collada.ObjectsById[node.Attribute("source").Value.Substring(1)];
                if (colladaNameId is ColladaVertices)
                {
                    ColladaVertices colladaVertices = (ColladaVertices)colladaNameId;
                    Source = colladaVertices.Source;
                    Semantic = colladaVertices.Semantic;
                    return;
                }
                Source = (ColladaSource)colladaNameId;
            }
        }

        class ColladaLibraryGeometries
        {
            public List<ColladaGeometry> Geometries = new List<ColladaGeometry>();

            public ColladaLibraryGeometries(ColladaRoot collada, XElement node)
            {
                foreach (XElement current in node.Elements(ColladaRoot.Namespace + "geometry"))
                {
                    Geometries.Add(new ColladaGeometry(collada, current));
                }
            }
        }

        class ColladaLibraryVisualScenes
        {
            public List<ColladaVisualScene> VisualScenes = new List<ColladaVisualScene>();

            public ColladaLibraryVisualScenes(ColladaRoot collada, XElement node)
            {
                foreach (XElement current in node.Elements(ColladaRoot.Namespace + "visual_scene"))
                {
                    VisualScenes.Add(new ColladaVisualScene(collada, current));
                }
            }
        }

        class ColladaMesh
        {
            public List<ColladaSource> Sources = new List<ColladaSource>();

            public ColladaVertices Vertices;

            public List<ColladaPolygons> Polygons = new List<ColladaPolygons>();

            public ColladaMesh(ColladaRoot collada, XElement node)
            {
                foreach (XElement current in node.Elements(ColladaRoot.Namespace + "source"))
                {
                    Sources.Add(new ColladaSource(collada, current));
                }
                XElement node2 = node.Element(ColladaRoot.Namespace + "vertices");
                Vertices = new ColladaVertices(collada, node2);
                foreach (XElement current2 in node.Elements(ColladaRoot.Namespace + "polygons").Concat(node.Elements(ColladaRoot.Namespace + "polylist")).Concat(node.Elements(ColladaRoot.Namespace + "triangles")))
                {
                    Polygons.Add(new ColladaPolygons(collada, current2));
                }
            }
        }

        class ColladaNameId
        {
            public string Id;

            public string Name;

            public ColladaNameId(ColladaRoot collada, XElement node, string idPostfix = "")
            {
                XAttribute xAttribute = node.Attribute("id");
                if (xAttribute != null)
                {
                    Id = xAttribute.Value + idPostfix;
                    collada.ObjectsById.Add(Id, this);
                }
                XAttribute xAttribute2 = node.Attribute("name");
                if (xAttribute2 != null)
                {
                    Name = xAttribute2.Value;
                }
            }
        }

        class ColladaNode : ColladaNameId
        {
            [Serializable]
            sealed class c
            {
                public static readonly ColladaNode.c a = new ColladaNode.c();

                public static Func<string, float> a__3_0;

                public static Func<string, float> a__3_1;

                public static Func<string, float> a__3_2;

                public static Func<string, float> a__3_3;

                internal float b__3_0(string s)
                {
                    return float.Parse(s, CultureInfo.InvariantCulture);
                }

                internal float b__3_1(string s)
                {
                    return float.Parse(s, CultureInfo.InvariantCulture);
                }

                internal float b__3_2(string s)
                {
                    return float.Parse(s, CultureInfo.InvariantCulture);
                }

                internal float b__3_3(string s)
                {
                    return float.Parse(s, CultureInfo.InvariantCulture);
                }
            }

            public Matrix Transform = Matrix.Identity;

            public List<ColladaNode> Nodes = new List<ColladaNode>();

            public List<ColladaGeometry> Geometries = new List<ColladaGeometry>();

            public ColladaNode(ColladaRoot collada, XElement node) : base(collada, node, "")
            {
                foreach (XElement current in node.Elements())
                {
                    if (current.Name == ColladaRoot.Namespace + "matrix")
                    {
                        IEnumerable<string> arg_91_0 = current.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                        Func<string, float> arg_91_1;
                        if ((arg_91_1 = ColladaNode.c.a__3_0) == null)
                        {
                            arg_91_1 = (ColladaNode.c.a__3_0 = new Func<string, float>(ColladaNode.c.a.b__3_0));
                        }
                        float[] array = arg_91_0.Select(arg_91_1).ToArray<float>();
                        Transform = Matrix.Transpose(new Matrix(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15])) * Transform;
                    }
                    else if (current.Name == ColladaRoot.Namespace + "translate")
                    {
                        IEnumerable<string> arg_13B_0 = current.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                        Func<string, float> arg_13B_1;
                        if ((arg_13B_1 = ColladaNode.c.a__3_1) == null)
                        {
                            arg_13B_1 = (ColladaNode.c.a__3_1 = new Func<string, float>(ColladaNode.c.a.b__3_1));
                        }
                        float[] array2 = arg_13B_0.Select(arg_13B_1).ToArray<float>();
                        Transform = Matrix.CreateTranslation(array2[0], array2[1], array2[2]) * Transform;
                    }
                    else if (current.Name == ColladaRoot.Namespace + "rotate")
                    {
                        IEnumerable<string> arg_1B2_0 = current.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                        Func<string, float> arg_1B2_1;
                        if ((arg_1B2_1 = ColladaNode.c.a__3_2) == null)
                        {
                            arg_1B2_1 = (ColladaNode.c.a__3_2 = new Func<string, float>(ColladaNode.c.a.b__3_2));
                        }
                        float[] array3 = arg_1B2_0.Select(arg_1B2_1).ToArray<float>();
                        Transform = Matrix.CreateFromAxisAngle(new Vector3(array3[0], array3[1], array3[2]), MathUtils.DegToRad(array3[3])) * Transform;
                    }
                    else if (current.Name == ColladaRoot.Namespace + "scale")
                    {
                        IEnumerable<string> arg_238_0 = current.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                        Func<string, float> arg_238_1;
                        if ((arg_238_1 = ColladaNode.c.a__3_3) == null)
                        {
                            arg_238_1 = (ColladaNode.c.a__3_3 = new Func<string, float>(ColladaNode.c.a.b__3_3));
                        }
                        float[] array4 = arg_238_0.Select(arg_238_1).ToArray<float>();
                        Transform = Matrix.CreateScale(array4[0], array4[1], array4[2]) * Transform;
                    }
                }
                foreach (XElement current2 in node.Elements(ColladaRoot.Namespace + "node"))
                {
                    Nodes.Add(new ColladaNode(collada, current2));
                }
                foreach (XElement current3 in node.Elements(ColladaRoot.Namespace + "instance_geometry"))
                {
                    Geometries.Add((ColladaGeometry)collada.ObjectsById[current3.Attribute("url").Value.Substring(1)]);
                }
            }
        }

        class ColladaPolygons
        {
            [Serializable]
            sealed class c
            {
                public static readonly ColladaPolygons.c a = new ColladaPolygons.c();

                public static Func<string, int> a__3_1;

                internal int b__3_0(string s)
                {
                    return int.Parse(s, CultureInfo.InvariantCulture);
                }

                internal int b__3_1(string s)
                {
                    return int.Parse(s, CultureInfo.InvariantCulture);
                }
            }

            public List<ColladaInput> Inputs = new List<ColladaInput>();

            public List<int> VCount = new List<int>();

            public List<int> P = new List<int>();

            public ColladaPolygons(ColladaRoot collada, XElement node)
            {
                foreach (XElement current in node.Elements(ColladaRoot.Namespace + "input"))
                {
                    Inputs.Add(new ColladaInput(collada, current));
                }
                foreach (XElement current2 in node.Elements(ColladaRoot.Namespace + "vcount"))
                {
                    List<int> arg_CC_0 = VCount;
                    IEnumerable<string> arg_C7_0 = current2.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    arg_CC_0.AddRange(arg_C7_0.Select(s => int.Parse(s, CultureInfo.InvariantCulture)));
                }
                foreach (XElement current3 in node.Elements(ColladaRoot.Namespace + "p"))
                {
                    List<int> arg_140_0 = P;
                    IEnumerable<string> arg_13B_0 = current3.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    Func<string, int> arg_13B_1;
                    if ((arg_13B_1 = ColladaPolygons.c.a__3_1) == null)
                    {
                        arg_13B_1 = (ColladaPolygons.c.a__3_1 = new Func<string, int>(ColladaPolygons.c.a.b__3_1));
                    }
                    arg_140_0.AddRange(arg_13B_0.Select(arg_13B_1));
                }
            }
        }

        class ColladaRoot
        {
            public static readonly XNamespace Namespace = "http://www.collada.org/2005/11/COLLADASchema";

            public readonly Dictionary<string, ColladaNameId> ObjectsById = new Dictionary<string, ColladaNameId>();

            public readonly Asset Asset;

            public readonly List<ColladaLibraryGeometries> LibraryGeometries = new List<ColladaLibraryGeometries>();

            public readonly List<ColladaLibraryVisualScenes> LibraryVisualScenes = new List<ColladaLibraryVisualScenes>();

            public readonly ColladaScene Scene;

            public ColladaRoot(XElement node)
            {
                Asset = new Asset(node.Element(ColladaRoot.Namespace + "asset"));
                foreach (XElement current in node.Elements(ColladaRoot.Namespace + "library_geometries"))
                {
                    LibraryGeometries.Add(new ColladaLibraryGeometries(this, current));
                }
                foreach (XElement current2 in node.Elements(ColladaRoot.Namespace + "library_visual_scenes"))
                {
                    LibraryVisualScenes.Add(new ColladaLibraryVisualScenes(this, current2));
                }
                Scene = new ColladaScene(this, node.Element(ColladaRoot.Namespace + "scene"));
            }
        }

        class ColladaScene
        {
            public ColladaVisualScene VisualScene;

            public ColladaScene(ColladaRoot collada, XElement node)
            {
                XElement xElement = node.Element(ColladaRoot.Namespace + "instance_visual_scene");
                VisualScene = (ColladaVisualScene)collada.ObjectsById[xElement.Attribute("url").Value.Substring(1) + "-ColladaVisualScene"];
            }
        }

        class ColladaSource : ColladaNameId
        {
            public ColladaFloatArray FloatArray;

            public ColladaAccessor Accessor;

            public ColladaSource(ColladaRoot collada, XElement node) : base(collada, node, "")
            {
                XElement xElement = node.Element(ColladaRoot.Namespace + "float_array");
                if (xElement != null)
                {
                    FloatArray = new ColladaFloatArray(collada, xElement);
                }
                XElement xElement2 = node.Element(ColladaRoot.Namespace + "technique_common");
                if (xElement2 != null)
                {
                    XElement xElement3 = xElement2.Element(ColladaRoot.Namespace + "accessor");
                    if (xElement3 != null)
                    {
                        Accessor = new ColladaAccessor(collada, xElement3);
                    }
                }
            }
        }

        class ColladaVertices : ColladaNameId
        {
            public string Semantic;

            public ColladaSource Source;

            public ColladaVertices(ColladaRoot collada, XElement node) : base(collada, node, "")
            {
                XElement xElement = node.Element(ColladaRoot.Namespace + "input");
                Semantic = xElement.Attribute("semantic").Value;
                Source = (ColladaSource)collada.ObjectsById[xElement.Attribute("source").Value.Substring(1)];
            }
        }

        class ColladaVisualScene : ColladaNameId
        {
            public List<ColladaNode> Nodes = new List<ColladaNode>();

            public ColladaVisualScene(ColladaRoot collada, XElement node) : base(collada, node, "-ColladaVisualScene")
            {
                foreach (XElement current in node.Elements(ColladaRoot.Namespace + "node"))
                {
                    Nodes.Add(new ColladaNode(collada, current));
                }
            }
        }

        struct Vertex : IEquatable<Vertex>
        {
            public byte[] Data;

            public int Start;

            public int Count;

            int m_hashCode;

            public Vertex(byte[] data, int start, int count)
            {
                Data = data;
                Start = start;
                Count = count;
                m_hashCode = 0;
                for (int i = 0; i < Count; i++)
                {
                    m_hashCode += (7919 * i + 977) * (int)Data[i + Start];
                }
            }

            public bool Equals(Vertex other)
            {
                if (m_hashCode != other.m_hashCode || Data.Length != other.Data.Length)
                {
                    return false;
                }
                for (int i = 0; i < Count; i++)
                {
                    if (Data[i + Start] != other.Data[i + other.Start])
                    {
                        return false;
                    }
                }
                return true;
            }

            public override bool Equals(object obj)
            {
                return obj is Color && Equals((Color)obj);
            }

            public override int GetHashCode()
            {
                return m_hashCode;
            }
        }
    }
}
