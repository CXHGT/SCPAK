using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public sealed class VertexDeclaration
    {
        private static List<VertexElement[]> allElements = new List<VertexElement[]>();

        public VertexElement[] VertexElements;

        public int VertexStride
        {
            get;
            set;
        }
        public VertexDeclaration(params VertexElement[] elements)
        {
            if (elements.Length == 0)
            {
                throw new ArgumentException("There must be at least one VertexElement.");
            }
            for (int i = 0; i < VertexDeclaration.allElements.Count; i++)
            {
                if (elements.SequenceEqual(VertexDeclaration.allElements[i]))
                {
                    this.VertexElements = VertexDeclaration.allElements[i];
                    break;
                }
            }
            if (this.VertexElements == null)
            {
                this.VertexElements = elements.ToArray<VertexElement>();
                VertexDeclaration.allElements.Add(this.VertexElements);
            }
            for (int j = 0; j < this.VertexElements.Length; j++)
            {
                VertexElement vertexElement = elements[j];
                this.VertexStride = Math.Max(this.VertexStride, vertexElement.Offset + vertexElement.Format.GetSize());
            }
        }
    }
}
