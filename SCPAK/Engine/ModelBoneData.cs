using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
    public class ModelBoneData
    {
        //
        // Fields
        //
        public string Name;

        public int ParentBoneIndex;

        public Matrix Transform;
    }

    public class ModelBuffersData
    {
        //
        // Fields
        //
        public VertexDeclaration VertexDeclaration;

        public byte[] Vertices = new byte[0];

        public byte[] Indices = new byte[0];
    }

    public class ModelMeshData
    {
        //
        // Fields
        //
        public string Name;

        public int ParentBoneIndex;

        public List<ModelMeshPartData> MeshParts = new List<ModelMeshPartData>();

        public BoundingBox BoundingBox;
    }

    public class ModelMeshPartData
    {
        //
        // Fields
        //
        public int BuffersDataIndex;

        public int StartIndex;

        public int IndicesCount;

        public BoundingBox BoundingBox;
    }

    public sealed class VertexDeclaration : IEquatable<VertexDeclaration>
    {
        //
        // Static Fields
        //
        private static List<VertexElement[]> m_allElements = new List<VertexElement[]>();

        //
        // Fields
        //
        internal VertexElement[] m_elements;

        //
        // Properties
        //
        public ReadOnlyList<VertexElement> VertexElements
        {
            get
            {
                return new ReadOnlyList<VertexElement>(this.m_elements);
            }
        }

        public int VertexStride
        {
            get;
            private set;
        }

        //
        // Constructors
        //
        public VertexDeclaration(params VertexElement[] elements)
        {
            if (elements.Length == 0)
            {
                throw new ArgumentException("There must be at least one VertexElement.");
            }
            for (int i = 0; i < VertexDeclaration.m_allElements.Count; i++)
            {
                if (elements.SequenceEqual(VertexDeclaration.m_allElements[i]))
                {
                    this.m_elements = VertexDeclaration.m_allElements[i];
                    break;
                }
            }
            if (this.m_elements == null)
            {
                this.m_elements = elements.ToArray<VertexElement>();
                VertexDeclaration.m_allElements.Add(this.m_elements);
            }
            for (int j = 0; j < this.m_elements.Length; j++)
            {
                VertexElement vertexElement = elements[j];
                this.VertexStride = MathUtils.Max(this.VertexStride, vertexElement.Offset + vertexElement.Format.GetSize());
            }
        }

        //
        // Methods
        //
        public override bool Equals(object other)
        {
            return other is VertexDeclaration && this.Equals((VertexDeclaration)other);
        }

        public bool Equals(VertexDeclaration other)
        {
            return !ReferenceEquals(null, other) && this.m_elements == other.m_elements;
        }

        public override int GetHashCode()
        {
            return this.m_elements.GetHashCode();
        }

        //
        // Operators
        //
        public static bool operator ==(VertexDeclaration vd1, VertexDeclaration vd2)
        {
            if (!ReferenceEquals(null, vd1))
            {
                return vd1.Equals(vd2);
            }
            return ReferenceEquals(null, vd2);
        }

        public static bool operator !=(VertexDeclaration vd1, VertexDeclaration vd2)
        {
            return !(vd1 == vd2);
        }
    }
}
