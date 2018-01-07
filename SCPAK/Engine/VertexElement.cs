using System;
namespace Engine
{
    public class VertexElement : IEquatable<VertexElement>
    {
        //
        // Fields
        //
        public readonly int Offset;

        public readonly VertexElementFormat Format;

        public readonly string Semantic;

        private int m_hashCode;

        //
        // Constructors
        //
        public VertexElement(int offset, VertexElementFormat format, string semantic)
        {
            if (offset < 0)
            {
                throw new ArgumentException("offset cannot be negative.");
            }
            if (string.IsNullOrEmpty(semantic))
            {
                throw new ArgumentException("semantic cannot be empty or null.");
            }
            this.Offset = offset;
            this.Format = format;
            this.Semantic = semantic;
            this.m_hashCode = this.Offset.GetHashCode() + this.Format.GetHashCode() + this.Semantic.GetHashCode();
        }

        public VertexElement(int offset, VertexElementFormat format, VertexElementSemantic semantic) : this(offset, format, semantic.GetSemanticString())
        {
        }

        //
        // Methods
        //
        public override bool Equals(object other)
        {
            return other is VertexElement && this.Equals((VertexElement)other);
        }

        public bool Equals(VertexElement other)
        {
            return !ReferenceEquals(null, other) && other.Offset == this.Offset && other.Format == this.Format && other.Semantic == this.Semantic;
        }

        public override int GetHashCode()
        {
            return this.m_hashCode;
        }

        //
        // Operators
        //
        public static bool operator ==(VertexElement ve1, VertexElement ve2)
        {
            if (!ReferenceEquals(null, ve1))
            {
                return ve1.Equals(ve2);
            }
            return ReferenceEquals(null, ve2);
        }

        public static bool operator !=(VertexElement ve1, VertexElement ve2)
        {
            return !(ve1 == ve2);
        }
    }

    public enum VertexElementSemantic
    {
        Position,
        Color,
        Normal,
        TextureCoordinate,
        TextureCoordinate0,
        TextureCoordinate1,
        TextureCoordinate2,
        TextureCoordinate3,
        TextureCoordinate4,
        TextureCoordinate5,
        TextureCoordinate6,
        TextureCoordinate7,
        Instance,
        BlendIndices,
        BlendWeights
    }
}
