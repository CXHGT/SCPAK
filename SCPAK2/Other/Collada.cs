using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SCPAK
{
	public static class Collada
	{

		public static ModelData Load(Stream stream)
		{
			ModelData modelData = new ModelData();
			Collada.ColladaRoot colladaRoot = new Collada.ColladaRoot(XElement.Load(stream));
			if (colladaRoot.Scene.VisualScene.Nodes.Count > 1)
			{
				ModelBoneData modelBoneData = new ModelBoneData();
				modelData.Bones.Add(modelBoneData);
				modelBoneData.ParentBoneIndex = -1;
				modelBoneData.Name = string.Empty;
				modelBoneData.Transform = Matrix.Identity;
				using (List<Collada.ColladaNode>.Enumerator enumerator = colladaRoot.Scene.VisualScene.Nodes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Collada.ColladaNode node = enumerator.Current;
						Collada.LoadNode(modelData, modelBoneData, node, Matrix.CreateScale(colladaRoot.Asset.Meter));
					}
					goto IL_104;
				}
			}
			foreach (Collada.ColladaNode node2 in colladaRoot.Scene.VisualScene.Nodes)
			{
				Collada.LoadNode(modelData, null, node2, Matrix.CreateScale(colladaRoot.Asset.Meter));
			}
		IL_104:
			foreach (ModelBuffersData modelBuffersData in modelData.Buffers)
			{
				Collada.IndexVertices(modelBuffersData.VertexDeclaration.VertexStride, modelBuffersData.Vertices, out modelBuffersData.Vertices, out modelBuffersData.Indices);
			}
			return modelData;
		}

		private static ModelBoneData LoadNode(ModelData data, ModelBoneData parentBoneData, Collada.ColladaNode node, Matrix transform)
		{
			ModelBoneData modelBoneData = new ModelBoneData();
			data.Bones.Add(modelBoneData);
			modelBoneData.ParentBoneIndex = ((parentBoneData != null) ? data.Bones.IndexOf(parentBoneData) : -1);
			modelBoneData.Name = node.Name;
			modelBoneData.Transform = node.Transform * transform;
			foreach (Collada.ColladaNode node2 in node.Nodes)
			{
				Collada.LoadNode(data, modelBoneData, node2, Matrix.Identity);
			}
			foreach (Collada.ColladaGeometry geometry in node.Geometries)
			{
				Collada.LoadGeometry(data, modelBoneData, geometry);
			}
			return modelBoneData;
		}

		private static ModelMeshData LoadGeometry(ModelData data, ModelBoneData parentBoneData, Collada.ColladaGeometry geometry)
		{
			ModelMeshData modelMeshData = new ModelMeshData();
			data.Meshes.Add(modelMeshData);
			modelMeshData.Name = parentBoneData.Name;
			modelMeshData.ParentBoneIndex = data.Bones.IndexOf(parentBoneData);
			bool flag = false;
			foreach (Collada.ColladaPolygons polygons in geometry.Mesh.Polygons)
			{
				ModelMeshPartData modelMeshPartData = Collada.LoadPolygons(data, polygons);
				modelMeshData.MeshParts.Add(modelMeshPartData);
				modelMeshData.BoundingBox = (flag ? BoundingBox.Union(modelMeshData.BoundingBox, modelMeshPartData.BoundingBox) : modelMeshPartData.BoundingBox);
				flag = true;
			}
			return modelMeshData;
		}

		private static ModelMeshPartData LoadPolygons(ModelData data, Collada.ColladaPolygons polygons)
		{
			ModelMeshPartData modelMeshPartData = new ModelMeshPartData();
			int num = 0;
			Dictionary<VertexElement, Collada.ColladaInput> dictionary = new Dictionary<VertexElement, Collada.ColladaInput>();
			foreach (Collada.ColladaInput colladaInput in polygons.Inputs)
			{
				string str = (colladaInput.Set == 0) ? string.Empty : colladaInput.Set.ToString(CultureInfo.InvariantCulture);
				if (colladaInput.Semantic == "POSITION")
				{
					dictionary[new VertexElement(num, VertexElementFormat.Vector3, "POSITION" + str)] = colladaInput;
					num += 12;
				}
				else if (colladaInput.Semantic == "NORMAL")
				{
					dictionary[new VertexElement(num, VertexElementFormat.Vector3, "NORMAL" + str)] = colladaInput;
					num += 12;
				}
				else if (colladaInput.Semantic == "TEXCOORD")
				{
					dictionary[new VertexElement(num, VertexElementFormat.Vector2, "TEXCOORD" + str)] = colladaInput;
					num += 8;
				}
				else if (colladaInput.Semantic == "COLOR")
				{
					dictionary[new VertexElement(num, VertexElementFormat.NormalizedByte4, "COLOR" + str)] = colladaInput;
					num += 4;
				}
			}
			VertexDeclaration vertexDeclaration = new VertexDeclaration(Enumerable.ToArray<VertexElement>(dictionary.Keys));
			ModelBuffersData modelBuffersData = Enumerable.FirstOrDefault<ModelBuffersData>(data.Buffers, (ModelBuffersData vd) => vd.VertexDeclaration == vertexDeclaration);
			if (modelBuffersData == null)
			{
				modelBuffersData = new ModelBuffersData();
				data.Buffers.Add(modelBuffersData);
				modelBuffersData.VertexDeclaration = vertexDeclaration;
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
				foreach (int num5 in polygons.VCount)
				{
					if (num5 == 3)
					{
						list.Add(num4);
						list.Add(num4 + 2);
						list.Add(num4 + 1);
						num4 += 3;
					}
					else
					{
						if (num5 != 4)
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
			int num6 = modelBuffersData.Vertices.Length;
			modelBuffersData.Vertices = Collada.ExtendArray<byte>(modelBuffersData.Vertices, list.Count * vertexStride);
			using (BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(modelBuffersData.Vertices, num6, list.Count * vertexStride)))
			{
				bool flag = false;
				foreach (KeyValuePair<VertexElement, Collada.ColladaInput> keyValuePair in dictionary)
				{
					VertexElement key = keyValuePair.Key;
					Collada.ColladaInput value = keyValuePair.Value;
					if (key.Semantic.StartsWith("POSITION"))
					{
						for (int j = 0; j < list.Count; j++)
						{
							float[] array = value.Source.Accessor.Source.Array;
							int offset = value.Source.Accessor.Offset;
							int stride = value.Source.Accessor.Stride;
							int num7 = polygons.P[list[j] * polygons.Inputs.Count + value.Offset];
							binaryWriter.BaseStream.Position = (long)(j * vertexStride + key.Offset);
							float num8 = array[offset + stride * num7];
							float num9 = array[offset + stride * num7 + 1];
							float num10 = array[offset + stride * num7 + 2];
							modelMeshPartData.BoundingBox = (flag ? BoundingBox.Union(modelMeshPartData.BoundingBox, new Vector3(num8, num9, num10)) : new BoundingBox(num8, num9, num10, num8, num9, num10));
							flag = true;
							binaryWriter.Write(num8);
							binaryWriter.Write(num9);
							binaryWriter.Write(num10);
						}
					}
					else if (key.Semantic.StartsWith("NORMAL"))
					{
						for (int k = 0; k < list.Count; k++)
						{
							float[] array2 = value.Source.Accessor.Source.Array;
							int offset2 = value.Source.Accessor.Offset;
							int stride2 = value.Source.Accessor.Stride;
							int num11 = polygons.P[list[k] * polygons.Inputs.Count + value.Offset];
							binaryWriter.BaseStream.Position = (long)(k * vertexStride + key.Offset);
							float num12 = array2[offset2 + stride2 * num11];
							float num13 = array2[offset2 + stride2 * num11 + 1];
							float num14 = array2[offset2 + stride2 * num11 + 2];
							float num15 = 1f / (float)Math.Sqrt(num12 * num12 + num13 * num13 + num14 * num14);
							binaryWriter.Write(num15 * num12);
							binaryWriter.Write(num15 * num13);
							binaryWriter.Write(num15 * num14);
						}
					}
					else if (key.Semantic.StartsWith("TEXCOORD"))
					{
						for (int l = 0; l < list.Count; l++)
						{
							float[] array3 = value.Source.Accessor.Source.Array;
							int offset3 = value.Source.Accessor.Offset;
							int stride3 = value.Source.Accessor.Stride;
							int num16 = polygons.P[list[l] * polygons.Inputs.Count + value.Offset];
							binaryWriter.BaseStream.Position = (long)(l * vertexStride + key.Offset);
							binaryWriter.Write(array3[offset3 + stride3 * num16]);
							binaryWriter.Write(1f - array3[offset3 + stride3 * num16 + 1]);
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
							float[] array4 = value.Source.Accessor.Source.Array;
							int offset4 = value.Source.Accessor.Offset;
							int stride4 = value.Source.Accessor.Stride;
							int num17 = polygons.P[list[m] * polygons.Inputs.Count + value.Offset];
							binaryWriter.BaseStream.Position = (long)(m * vertexStride + key.Offset);
							Color color = new Color(array4[offset4 + stride4 * num17], array4[offset4 + stride4 * num17 + 1], array4[offset4 + stride4 * num17 + 2], array4[offset4 + stride4 * num17 + 3]);
							binaryWriter.Write(color.PackedValue);
						}
					}
				}
			}
			modelMeshPartData.StartIndex = num6 / vertexStride;
			modelMeshPartData.IndicesCount = list.Count;
			return modelMeshPartData;
		}

		private static T[] ExtendArray<T>(T[] array, int extensionLength)
		{
			T[] array2 = new T[array.Length + extensionLength];
			Array.Copy(array, array2, array.Length);
			return array2;
		}

		private static void IndexVertices(int vertexStride, byte[] vertices, out byte[] resultVertices, out byte[] resultIndices)
		{
			int num = vertices.Length / vertexStride;
			Dictionary<Collada.Vertex, ushort> dictionary = new Dictionary<Collada.Vertex, ushort>();
			resultIndices = new byte[2 * num];
			for (int i = 0; i < num; i++)
			{
				Collada.Vertex key = new Collada.Vertex(vertices, i * vertexStride, vertexStride);
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
			foreach (KeyValuePair<Collada.Vertex, ushort> keyValuePair in dictionary)
			{
				Collada.Vertex key2 = keyValuePair.Key;
				ushort value = keyValuePair.Value;
				Array.Copy(key2.Data, key2.Start, resultVertices, (int)value * vertexStride, key2.Count);
			}
		}

		private struct Vertex : IEquatable<Collada.Vertex>
		{
			public Vertex(byte[] data, int start, int count)
			{
				this.Data = data;
				this.Start = start;
				this.Count = count;
				this.m_hashCode = 0;
				for (int i = 0; i < this.Count; i++)
				{
					this.m_hashCode += (7919 * i + 977) * (int)this.Data[i + this.Start];
				}
			}

			public bool Equals(Collada.Vertex other)
			{
				if (this.m_hashCode != other.m_hashCode || this.Data.Length != other.Data.Length)
				{
					return false;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (this.Data[i + this.Start] != other.Data[i + other.Start])
					{
						return false;
					}
				}
				return true;
			}

			public override bool Equals(object obj)
			{
				return obj is Color && this.Equals((Color)obj);
			}

			public override int GetHashCode()
			{
				return this.m_hashCode;
			}

			public byte[] Data;

			public int Start;

			public int Count;

			private int m_hashCode;
		}

		private class Asset
		{
			public Asset(XElement node)
			{
				XElement xelement = node.Element(Collada.ColladaRoot.Namespace + "unit");
				if (xelement != null)
				{
					XAttribute xattribute = xelement.Attribute("meter");
					if (xattribute != null)
					{
						this.Meter = float.Parse(xattribute.Value, CultureInfo.InvariantCulture);
					}
				}
			}

			public readonly float Meter = 1f;
		}

		private class ColladaRoot
		{
			public ColladaRoot(XElement node)
			{
				this.Asset = new Collada.Asset(node.Element(Collada.ColladaRoot.Namespace + "asset"));
				foreach (XElement node2 in node.Elements(Collada.ColladaRoot.Namespace + "library_geometries"))
				{
					this.LibraryGeometries.Add(new Collada.ColladaLibraryGeometries(this, node2));
				}
				foreach (XElement node3 in node.Elements(Collada.ColladaRoot.Namespace + "library_visual_scenes"))
				{
					this.LibraryVisualScenes.Add(new Collada.ColladaLibraryVisualScenes(this, node3));
				}
				this.Scene = new Collada.ColladaScene(this, node.Element(Collada.ColladaRoot.Namespace + "scene"));
			}

			public static readonly XNamespace Namespace = "http://www.collada.org/2005/11/COLLADASchema";

			public readonly Dictionary<string, Collada.ColladaNameId> ObjectsById = new Dictionary<string, Collada.ColladaNameId>();

			public readonly Collada.Asset Asset;

			public readonly List<Collada.ColladaLibraryGeometries> LibraryGeometries = new List<Collada.ColladaLibraryGeometries>();

			public readonly List<Collada.ColladaLibraryVisualScenes> LibraryVisualScenes = new List<Collada.ColladaLibraryVisualScenes>();

			public readonly Collada.ColladaScene Scene;
		}

		private class ColladaNameId
		{
			public ColladaNameId(Collada.ColladaRoot collada, XElement node, string idPostfix = "")
			{
				XAttribute xattribute = node.Attribute("id");
				if (xattribute != null)
				{
					this.Id = xattribute.Value + idPostfix;
					collada.ObjectsById.Add(this.Id, this);
				}
				XAttribute xattribute2 = node.Attribute("name");
				if (xattribute2 != null)
				{
					this.Name = xattribute2.Value;
				}
			}

			public string Id;

			public string Name;
		}

		private class ColladaLibraryVisualScenes
		{
			public ColladaLibraryVisualScenes(Collada.ColladaRoot collada, XElement node)
			{
				foreach (XElement node2 in node.Elements(Collada.ColladaRoot.Namespace + "visual_scene"))
				{
					this.VisualScenes.Add(new Collada.ColladaVisualScene(collada, node2));
				}
			}

			public List<Collada.ColladaVisualScene> VisualScenes = new List<Collada.ColladaVisualScene>();
		}

		private class ColladaLibraryGeometries
		{
			public ColladaLibraryGeometries(Collada.ColladaRoot collada, XElement node)
			{
				foreach (XElement node2 in node.Elements(Collada.ColladaRoot.Namespace + "geometry"))
				{
					this.Geometries.Add(new Collada.ColladaGeometry(collada, node2));
				}
			}

			public List<Collada.ColladaGeometry> Geometries = new List<Collada.ColladaGeometry>();
		}

		private class ColladaScene
		{
			public ColladaScene(Collada.ColladaRoot collada, XElement node)
			{
				XElement xelement = node.Element(Collada.ColladaRoot.Namespace + "instance_visual_scene");
				this.VisualScene = (Collada.ColladaVisualScene)collada.ObjectsById[xelement.Attribute("url").Value.Substring(1) + "-ColladaVisualScene"];
			}

			public Collada.ColladaVisualScene VisualScene;
		}

		private class ColladaVisualScene : Collada.ColladaNameId
		{
			public ColladaVisualScene(Collada.ColladaRoot collada, XElement node) : base(collada, node, "-ColladaVisualScene")
			{
				foreach (XElement node2 in node.Elements(Collada.ColladaRoot.Namespace + "node"))
				{
					this.Nodes.Add(new Collada.ColladaNode(collada, node2));
				}
			}

			public List<Collada.ColladaNode> Nodes = new List<Collada.ColladaNode>();
		}

		private class ColladaNode : Collada.ColladaNameId
		{
			public ColladaNode(Collada.ColladaRoot collada, XElement node) : base(collada, node, "")
			{
				foreach (XElement xelement in node.Elements())
				{
					if (xelement.Name == Collada.ColladaRoot.Namespace + "matrix")
					{
						float[] array = Enumerable.ToArray<float>(Enumerable.Select<string, float>(xelement.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries), (string s) => float.Parse(s, CultureInfo.InvariantCulture)));
						this.Transform = Matrix.Transpose(new Matrix(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15])) * this.Transform;
					}
					else if (xelement.Name == Collada.ColladaRoot.Namespace + "translate")
					{
						float[] array2 = Enumerable.ToArray<float>(Enumerable.Select<string, float>(xelement.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries), (string s) => float.Parse(s, CultureInfo.InvariantCulture)));
						this.Transform = Matrix.CreateTranslation(array2[0], array2[1], array2[2]) * this.Transform;
					}
					else if (xelement.Name == Collada.ColladaRoot.Namespace + "rotate")
					{
						float[] array3 = Enumerable.ToArray<float>(Enumerable.Select<string, float>(xelement.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries), (string s) => float.Parse(s, CultureInfo.InvariantCulture)));
						this.Transform = Matrix.CreateFromAxisAngle(new Vector3(array3[0], array3[1], array3[2]), array3[3] / 180f * 3.1415927f) * this.Transform;
					}
					else if (xelement.Name == Collada.ColladaRoot.Namespace + "scale")
					{
						float[] array4 = Enumerable.ToArray<float>(Enumerable.Select<string, float>(xelement.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries), (string s) => float.Parse(s, CultureInfo.InvariantCulture)));
						this.Transform = Matrix.CreateScale(array4[0], array4[1], array4[2]) * this.Transform;
					}
				}
				foreach (XElement node2 in node.Elements(Collada.ColladaRoot.Namespace + "node"))
				{
					this.Nodes.Add(new Collada.ColladaNode(collada, node2));
				}
				foreach (XElement xelement2 in node.Elements(Collada.ColladaRoot.Namespace + "instance_geometry"))
				{
					this.Geometries.Add((Collada.ColladaGeometry)collada.ObjectsById[xelement2.Attribute("url").Value.Substring(1)]);
				}
			}

			public Matrix Transform = Matrix.Identity;

			public List<Collada.ColladaNode> Nodes = new List<Collada.ColladaNode>();

			public List<Collada.ColladaGeometry> Geometries = new List<Collada.ColladaGeometry>();
		}

		private class ColladaGeometry : Collada.ColladaNameId
		{
			public ColladaGeometry(Collada.ColladaRoot collada, XElement node) : base(collada, node, "")
			{
				XElement xelement = node.Element(Collada.ColladaRoot.Namespace + "mesh");
				if (xelement != null)
				{
					this.Mesh = new Collada.ColladaMesh(collada, xelement);
				}
			}

			public Collada.ColladaMesh Mesh;
		}

		private class ColladaMesh
		{
			public ColladaMesh(Collada.ColladaRoot collada, XElement node)
			{
				foreach (XElement node2 in node.Elements(Collada.ColladaRoot.Namespace + "source"))
				{
					this.Sources.Add(new Collada.ColladaSource(collada, node2));
				}
				XElement node3 = node.Element(Collada.ColladaRoot.Namespace + "vertices");
				this.Vertices = new Collada.ColladaVertices(collada, node3);
				foreach (XElement node4 in Enumerable.Concat<XElement>(Enumerable.Concat<XElement>(node.Elements(Collada.ColladaRoot.Namespace + "polygons"), node.Elements(Collada.ColladaRoot.Namespace + "polylist")), node.Elements(Collada.ColladaRoot.Namespace + "triangles")))
				{
					this.Polygons.Add(new Collada.ColladaPolygons(collada, node4));
				}
			}

			public List<Collada.ColladaSource> Sources = new List<Collada.ColladaSource>();

			public Collada.ColladaVertices Vertices;

			public List<Collada.ColladaPolygons> Polygons = new List<Collada.ColladaPolygons>();
		}

		private class ColladaSource : Collada.ColladaNameId
		{
			public ColladaSource(Collada.ColladaRoot collada, XElement node) : base(collada, node, "")
			{
				XElement xelement = node.Element(Collada.ColladaRoot.Namespace + "float_array");
				if (xelement != null)
				{
					this.FloatArray = new Collada.ColladaFloatArray(collada, xelement);
				}
				XElement xelement2 = node.Element(Collada.ColladaRoot.Namespace + "technique_common");
				if (xelement2 != null)
				{
					XElement xelement3 = xelement2.Element(Collada.ColladaRoot.Namespace + "accessor");
					if (xelement3 != null)
					{
						this.Accessor = new Collada.ColladaAccessor(collada, xelement3);
					}
				}
			}

			public Collada.ColladaFloatArray FloatArray;

			public Collada.ColladaAccessor Accessor;
		}

		private class ColladaFloatArray : Collada.ColladaNameId
		{
			public ColladaFloatArray(Collada.ColladaRoot collada, XElement node) : base(collada, node, "")
			{
				this.Array = Enumerable.ToArray<float>(Enumerable.Select<string, float>(node.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries), (string s) => float.Parse(s, CultureInfo.InvariantCulture)));
			}

			public float[] Array;
		}

		private class ColladaAccessor
		{
			public ColladaAccessor(Collada.ColladaRoot collada, XElement node)
			{
				this.Source = (Collada.ColladaFloatArray)collada.ObjectsById[node.Attribute("source").Value.Substring(1)];
				XAttribute xattribute = node.Attribute("offset");
				if (xattribute != null)
				{
					this.Offset = int.Parse(xattribute.Value, CultureInfo.InvariantCulture);
				}
				XAttribute xattribute2 = node.Attribute("stride");
				if (xattribute2 != null)
				{
					this.Stride = int.Parse(xattribute2.Value, CultureInfo.InvariantCulture);
				}
			}

			public Collada.ColladaFloatArray Source;

			public int Offset;

			public int Stride = 1;
		}

		private class ColladaVertices : Collada.ColladaNameId
		{
			public ColladaVertices(Collada.ColladaRoot collada, XElement node) : base(collada, node, "")
			{
				XElement xelement = node.Element(Collada.ColladaRoot.Namespace + "input");
				this.Semantic = xelement.Attribute("semantic").Value;
				this.Source = (Collada.ColladaSource)collada.ObjectsById[xelement.Attribute("source").Value.Substring(1)];
			}

			public string Semantic;

			public Collada.ColladaSource Source;
		}

		private class ColladaPolygons
		{
			public ColladaPolygons(Collada.ColladaRoot collada, XElement node)
			{
				foreach (XElement node2 in node.Elements(Collada.ColladaRoot.Namespace + "input"))
				{
					this.Inputs.Add(new Collada.ColladaInput(collada, node2));
				}
				foreach (XElement xelement in node.Elements(Collada.ColladaRoot.Namespace + "vcount"))
				{
					this.VCount.AddRange(Enumerable.Select<string, int>(xelement.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries), (string s) => int.Parse(s, CultureInfo.InvariantCulture)));
				}
				foreach (XElement xelement2 in node.Elements(Collada.ColladaRoot.Namespace + "p"))
				{
					this.P.AddRange(Enumerable.Select<string, int>(xelement2.Value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries), (string s) => int.Parse(s, CultureInfo.InvariantCulture)));
				}
			}

			public List<Collada.ColladaInput> Inputs = new List<Collada.ColladaInput>();

			public List<int> VCount = new List<int>();

			public List<int> P = new List<int>();
		}

		private class ColladaInput
		{
			public ColladaInput(Collada.ColladaRoot collada, XElement node)
			{
				this.Offset = int.Parse(node.Attribute("offset").Value, CultureInfo.InvariantCulture);
				this.Semantic = node.Attribute("semantic").Value;
				XAttribute xattribute = node.Attribute("set");
				if (xattribute != null)
				{
					this.Set = int.Parse(xattribute.Value, CultureInfo.InvariantCulture);
				}
				Collada.ColladaNameId colladaNameId = collada.ObjectsById[node.Attribute("source").Value.Substring(1)];
				if (colladaNameId is Collada.ColladaVertices)
				{
					Collada.ColladaVertices colladaVertices = (Collada.ColladaVertices)colladaNameId;
					this.Source = colladaVertices.Source;
					this.Semantic = colladaVertices.Semantic;
					return;
				}
				this.Source = (Collada.ColladaSource)colladaNameId;
			}

			public int Offset;

			public string Semantic;

			public int Set;

			public Collada.ColladaSource Source;
		}
	}
}

