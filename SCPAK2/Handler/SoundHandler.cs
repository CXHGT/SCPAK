using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public static class SoundHandler
    {
        public static void WriteSound(Stream mainStream, Stream soundStream)
        {
            BinaryReader binaryReader = new BinaryReader(soundStream);
            binaryReader.BaseStream.Position = 22L;
            int channelsCount = binaryReader.ReadInt16();
            int samplingFrequency = binaryReader.ReadInt32();
            binaryReader.BaseStream.Position += 12L;
            int bytesCount = binaryReader.ReadInt32();
            byte[] array = new byte[bytesCount];
            if (soundStream.Read(array, 0, array.Length) != array.Length)
            {
                throw new Exception("解析wav文件错误");
            }
            binaryReader.Dispose();
            BinaryWriter binaryWriter = new BinaryWriter(mainStream);
            binaryWriter.Write(false);
            binaryWriter.Write(channelsCount);
            binaryWriter.Write(samplingFrequency);
            binaryWriter.Write(bytesCount);
            binaryWriter.Write(array);
        }
        public static void RecoverSound(Stream targetFileStream, Stream soundStream)
        {
            BinaryReader binaryReader = new BinaryReader(soundStream);
            bool flat = binaryReader.ReadBoolean();
            int channelsCount = binaryReader.ReadInt32();
            int samplingFrequency = binaryReader.ReadInt32();
            int bytesCount = binaryReader.ReadInt32();
            byte[] array = new byte[bytesCount];
            if (soundStream.Read(array, 0, array.Length) != array.Length)
            {
                throw new Exception("还原wav文件错误");
            }
            BinaryWriter binaryWriter = new BinaryWriter(targetFileStream);
            binaryWriter.Write((byte)'R');
            binaryWriter.Write((byte)'I');
            binaryWriter.Write((byte)'F');
            binaryWriter.Write((byte)'F');
            binaryWriter.Write(44 + bytesCount / 2);
            binaryWriter.Write((byte)'W');
            binaryWriter.Write((byte)'A');
            binaryWriter.Write((byte)'V');
            binaryWriter.Write((byte)'E');
            binaryWriter.Write((byte)'f');
            binaryWriter.Write((byte)'m');
            binaryWriter.Write((byte)'t');
            binaryWriter.Write((byte)' ');
            binaryWriter.Write(16);
            binaryWriter.Write((short)1);
            binaryWriter.Write((short)channelsCount);
            binaryWriter.Write(samplingFrequency);
            binaryWriter.Write(channelsCount * 2 * samplingFrequency);
            binaryWriter.Write((short)(channelsCount * 2));
            binaryWriter.Write((short)16);
            binaryWriter.Write((byte)'d');
            binaryWriter.Write((byte)'a');
            binaryWriter.Write((byte)'t');
            binaryWriter.Write((byte)'a');
            binaryWriter.Write(bytesCount);
            binaryWriter.Write(array);
        }


    }
}
