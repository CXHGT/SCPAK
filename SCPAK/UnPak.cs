using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCPAK
{
    public class UnPak
    {
        private static void isPakFile(BinaryReader binaryReader)
        {
            byte[] array = new byte[4];
            if (binaryReader.Read(array, 0, array.Length) != array.Length || array[0] != (byte)'P' || array[1] != (byte)'A' || array[2] != (byte)'K' || array[3] != (byte)'\0')
            {
                throw new FileLoadException("该文件不是Survivalcraft2的PAK文件！");
            }
        }
        public UnPak(string PakFile)
        {
            if (!File.Exists(PakFile))
            {
                throw new FileNotFoundException("文件不存在！");
            }
            string pakDirectory = Path.GetDirectoryName(PakFile) + "/" + Path.GetFileNameWithoutExtension(PakFile);
            FileStream stream = new FileStream(PakFile, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8, true);
            isPakFile(binaryReader);
            int num = binaryReader.ReadInt32();
            int num2 = binaryReader.ReadInt32();
            List<PAKInfo> listFileStream = new List<PAKInfo>(num2);
            for (int i = 0; i < num2; i++)
            {
                string fileName = binaryReader.ReadString();
                string typeName = binaryReader.ReadString();
                int num3 = binaryReader.ReadInt32() + num;
                int num4 = binaryReader.ReadInt32();
                long position = binaryReader.BaseStream.Position;
                listFileStream.Add(new PAKInfo
                {
                    fileStream = this.ContentFile(stream, num3, num4),
                    fileName = fileName,
                    typeName = typeName
                });

                binaryReader.BaseStream.Position = position;
            }
            UnPakData._UnPakData(listFileStream, pakDirectory);
            binaryReader.Dispose();
            stream.Dispose();
        }
        public Stream ContentFile(Stream stream, int position, int bytesCount)
        {
            if (stream is MemoryStream)
            {
                return new MemoryStream(((MemoryStream)stream).ToArray(), position, bytesCount);
            }
            stream.Position = position;
            byte[] array = new byte[bytesCount];
            stream.Read(array, 0, array.Length);
            return new MemoryStream(array, false);
        }
    }
}
