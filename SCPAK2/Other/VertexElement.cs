using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public class VertexElement
    {
        public readonly int Offset;

        public readonly VertexElementFormat Format;

        public readonly string Semantic;

        public int HashCode;

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
            this.HashCode = this.Offset.GetHashCode() + this.Format.GetHashCode() + this.Semantic.GetHashCode();
        }
    }
}
