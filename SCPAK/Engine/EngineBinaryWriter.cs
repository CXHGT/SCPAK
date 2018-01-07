using System;
using System.IO;
using System.Text;

namespace Engine
{
    public class EngineBinaryWriter : BinaryWriter
    {
        public EngineBinaryWriter(Stream stream, bool leaveOpen = false) : base(stream, Encoding.UTF8, leaveOpen)
        {
        }
        public virtual void Write(Vector3 value)
        {
            this.Write(value.X);
            this.Write(value.Y);
            this.Write(value.Z);
        }
        public virtual void Write(BoundingBox value)
        {
            this.Write(value.Min);
            this.Write(value.Max);
        }

        public virtual void Write(Matrix value)
        {
            this.Write(value.M11);
            this.Write(value.M12);
            this.Write(value.M13);
            this.Write(value.M14);
            this.Write(value.M21);
            this.Write(value.M22);
            this.Write(value.M23);
            this.Write(value.M24);
            this.Write(value.M31);
            this.Write(value.M32);
            this.Write(value.M33);
            this.Write(value.M34);
            this.Write(value.M41);
            this.Write(value.M42);
            this.Write(value.M43);
            this.Write(value.M44);
        }
    }
}
