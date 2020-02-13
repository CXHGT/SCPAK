using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public class PadStream : Stream
    {
        public byte[] keys = null;

        private readonly Stream stream;

        private readonly bool leaveOpen;

        public PadStream(Stream stream,byte[] keys = null, bool leaveOpen = false)
        {
            this.stream = stream;
            this.leaveOpen = leaveOpen;
            this.keys = keys;
        }
        public override bool CanRead => this.stream.CanRead;

        public override bool CanSeek => this.stream.CanSeek;

        public override bool CanWrite => this.stream.CanWrite;

        public override long Length => this.stream.Length;

        public override long Position
        {
            get => this.stream.Position;
            set => this.stream.Position = value;
        }

        public override void Flush()
        {
            this.stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num = this.stream.Read(buffer, offset, count);
            if (this.keys != null)
            {
                PadStream.OnPad(buffer, offset, count, this.Position - (long)num, this.keys);
            }
            return num;
        }

        public override int ReadByte()
        {
            if (this.keys == null)
            {
                return this.stream.ReadByte();
            }
            int num = this.stream.ReadByte();
            if (num < 0)
            {
                return -1;
            }
            return PadStream.OnPad((byte)num, this.Position - 1L, this.keys);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.stream.Seek(offset,origin);
        }

        public override void SetLength(long value)
        {
            this.stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.keys != null)
            {
                byte[] array = new byte[count];
                Array.Copy(buffer, offset, array, 0, count);
                PadStream.OnPad(array, 0, count, this.Position, this.keys);
                this.stream.Write(array, offset, count);
                return;
            }
            this.stream.Write(buffer, offset, count);
        }

        private static void OnPad(byte[] buffer, int offset, int count, long position, byte[] pad)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[offset + i] = PadStream.OnPad(buffer[offset + i], position + i, pad);
            }
        }

        private static byte OnPad(byte b, long position, byte[] pad)
        {
            int a = (b ^ pad[(int)checked(position % unchecked(pad.Length))]);
            return (byte)a;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!this.leaveOpen)
            {
                this.stream.Dispose();
            }
        }

    }
}
