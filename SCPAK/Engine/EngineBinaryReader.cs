using System;
using System.IO;
using System.Text;
namespace Engine
{
    public class EngineBinaryReader : BinaryReader
    {
        public EngineBinaryReader(Stream stream, bool leaveOpen = false) : base(stream, Encoding.UTF8, leaveOpen)
        {
        }
        public virtual BoundingBox ReadBoundingBox()
        {
            return new BoundingBox(this.ReadVector3(), this.ReadVector3());
        }

        public virtual Matrix ReadMatrix()
        {
            return new Matrix(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
        }
        public virtual Vector3 ReadVector3()
        {
            return new Vector3(this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
        }
        public virtual Vector2 ReadVector2()
        {
            return new Vector2(this.ReadSingle(), this.ReadSingle());
        }
    }
}
