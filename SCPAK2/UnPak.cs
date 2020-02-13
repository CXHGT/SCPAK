using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SCPAK
{
    public class UnPak
    {
        private byte[] keys = Encoding.UTF8.GetBytes("tiTrKAXRpwuRhNI3gTkxIun6AyLxSZaIgEjVkyFWhD6w0QgwmN5YwykY2I79OHIolI1r4ewZ2uEfStqC7GRDM8CRTNQTdg91pkOkbnIPAiEp2EqkZWYPgPv6CNZpB3E1OuuBmR3ZzYEv8UMjQxjyXZy1CEOD8guk3uiiPvyFaf5pSznSNWXbnhmAzTbi1TEGCyhxejMTB23KUgqNiskGlrHaIVNz83DXVGkvm");
        public UnPak(string pakFile)
        {
            if (!File.Exists(pakFile))
            {
                throw new FileNotFoundException("文件不存在！");
            }
            string pakDirectory = Path.GetDirectoryName(pakFile) + "/" + Path.GetFileNameWithoutExtension(pakFile);
            Stream stream = new FileStream(pakFile, FileMode.Open);
            PadStream padStream = new PadStream(stream,this.keys);
            BinaryReader binaryReader = new BinaryReader(padStream, Encoding.UTF8, true);
            byte[] array = new byte[4];
            if (stream.Read(array, 0, array.Length) != array.Length || array[0] != (byte)'P' || array[1] != (byte)'K' || array[2] != (byte)'2' || array[3] != (byte)'\0')
            {
                throw new FileLoadException("该文件不是Survivalcraft2(2.2)的PAK文件！");
            }
            long num = binaryReader.ReadInt64();
            int num2 = binaryReader.ReadInt32();
            List<PakInfo> listFileStream = new List<PakInfo>(num2);
            for (int i = 0; i < num2; i++)
            {
                string fileName = binaryReader.ReadString();
                string typeName = binaryReader.ReadString();
                long num3 = binaryReader.ReadInt64() + num;
                long num4 = binaryReader.ReadInt64();
                //Console.WriteLine($"{i}: {typeName},{fileName},{num3},{num4}");
                long position = binaryReader.BaseStream.Position;
                listFileStream.Add(new PakInfo
                {
                    fileStream = ContentFile(padStream, num3, num4),
                    fileName = fileName,
                    typeName = typeName
                });
                binaryReader.BaseStream.Position = position;
            }
            Load(listFileStream, pakDirectory);
            binaryReader.Dispose();
            padStream.Dispose();
        }
        public Stream ContentFile(PadStream padStream, long position, long bytesCount)
        {
            padStream.keys = new byte[] { 63 };
            padStream.Position = position;
            byte[] buffer = new byte[bytesCount];
            for(long i = 0; i < bytesCount; i++)
            {
                buffer[i] = (byte)padStream.ReadByte();
            }
            MemoryStream memoryStream = new MemoryStream(buffer,false);
            padStream.keys = this.keys;
            return memoryStream;
        }
        public void Load(List<PakInfo> listFileStream, string pakDirectory)
        {
            if (!Directory.Exists(pakDirectory)) Directory.CreateDirectory(pakDirectory);
            foreach (PakInfo pakContentFile in listFileStream)
            {
                Stream fileStream;
                switch (pakContentFile.typeName)
                {
                    case "System.String":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.txt");
                        TextHandler.RecoverText(fileStream, pakContentFile.fileStream);
                        break;
                    case "System.Xml.Linq.XElement":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.xml");
                        TextHandler.RecoverText(fileStream, pakContentFile.fileStream);
                        break;
                    case "Engine.Media.StreamingSource":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.ogg");
                        pakContentFile.fileStream.CopyTo(fileStream);
                        break;
                    case "Engine.Graphics.Texture2D":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.png");
                        Texture2DHandler.RecoverTexture2D(fileStream, pakContentFile.fileStream);
                        break;
                    case "Engine.Audio.SoundBuffer":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.wav");
                        SoundHandler.RecoverSound(fileStream, pakContentFile.fileStream);
                        break;
                    case "Engine.Graphics.Model":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.dae");
                        ModelHandler.RecoverModel(fileStream, pakContentFile.fileStream);
                        //pakContentFile.fileStream.CopyTo(fileStream);
                        break;
                    case "Engine.Graphics.Shader":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.shader");
                        pakContentFile.fileStream.CopyTo(fileStream);
                        break;
                    case "Engine.Media.BitmapFont":
                        try
                        {
                            fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.lst");
                            Stream bitmapStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.png");
                            FontHandler.RecoverFont(fileStream, bitmapStream, pakContentFile.fileStream);
                            bitmapStream.Dispose();
                        }
                        catch
                        {
                            pakContentFile.fileStream.Position = 0;
                            fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.font");
                            pakContentFile.fileStream.CopyTo(fileStream);
                        }
                        break;
                    default:
                        throw new Exception("发现无法识别的文件类型:" + pakContentFile.typeName + "\t文件名称:" + pakContentFile.fileName);
                }
                fileStream.Dispose();
                pakContentFile.fileStream.Dispose();
            }
        }
        public FileStream CreateFile(string file)
        {
            FileStream fileStream;
            try
            {
                fileStream = new FileStream(file, FileMode.Create);
            }
            catch (DirectoryNotFoundException)
            {
                string[] directorys = file.Split('/');
                string path = "";
                for (int i = 0; i < directorys.Length - 1; i++)
                {
                    if (i == directorys.Length - 1)
                    {
                        path += directorys[i];
                        break;
                    }
                    path += directorys[i] + "/";
                }
                Directory.CreateDirectory(path);
                fileStream = new FileStream(file, FileMode.Create);
            }
            return fileStream;
        }
    }
}
