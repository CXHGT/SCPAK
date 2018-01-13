using Engine;
using Hjg.Pngcs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    internal class UnPakData
    {
        public static void _UnPakData(List<PAKInfo> listFileStream, string pakDirectory)
        {
            if (!Directory.Exists(pakDirectory)) Directory.CreateDirectory(pakDirectory);
            foreach (PAKInfo pakContentFile in listFileStream)
            {
                FileStream fileStream;
                switch (pakContentFile.typeName)
                {
                    case "System.String":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.txt");
                        TextSave(pakContentFile.fileStream, fileStream);
                        break;
                    case "System.Xml.Linq.XElement":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.xml");
                        TextSave(pakContentFile.fileStream, fileStream);
                        break;
                    case "Engine.Graphics.Texture2D":
                        {
                            string[] fileNames = pakContentFile.fileName.Split('/');
                            string fileName = "";
                            for (int i = 0; i < fileNames.Length; i++)
                            {
                                if (i + 1 == fileNames.Length)
                                {
                                    fileName += "/!" + fileNames[i];
                                }
                                else
                                {
                                    fileName += "/" + fileNames[i];
                                }
                            }
                            if (File.Exists($"{pakDirectory}/{pakContentFile.fileName}.lst") || File.Exists($"{pakDirectory}{fileName}.lst")) continue;
                            fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.png");
                            bool b = PngSave(pakContentFile.fileStream, fileStream);
                            if (b)
                            {
                                fileStream.Dispose();
                                if (File.Exists($"{pakDirectory}{fileName}.png")) File.Delete($"{pakDirectory}{fileName}.png");
                                File.Move($"{pakDirectory}/{pakContentFile.fileName}.png", $"{pakDirectory}{fileName}.png");
                            }
                        }
                        break;
                    case "Engine.Audio.SoundBuffer":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.wav");
                        SoundSave(pakContentFile.fileStream,fileStream);
                        break;
                    case "Engine.Graphics.Model":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.dae");
                        SCModelSave(pakContentFile.fileStream, fileStream);
                        break;
                    case "Engine.Graphics.Shader":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.fsh");
                        pakContentFile.fileStream.CopyTo(fileStream);
                        break;
                    case "Engine.Media.BitmapFont":
                        {
                            try
                            {
                                string[] fileNames = pakContentFile.fileName.Split('/');
                                string fileName = "";
                                for (int i = 0; i < fileNames.Length; i++)
                                {
                                    if (i + 1 == fileNames.Length)
                                    {
                                        fileName += "/!" + fileNames[i];
                                    }
                                    else
                                    {
                                        fileName += "/" + fileNames[i];
                                    }
                                }
                                MemoryStream lstStream = new MemoryStream();
                                FontSave(pakContentFile.fileStream, lstStream);
                                Stream pngStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.png");
                                bool b = PngSave(pakContentFile.fileStream, pngStream);
                                pngStream.Dispose();
                                if (b)
                                {
                                    if (File.Exists($"{pakDirectory}{fileName}.png")) File.Delete($"{pakDirectory}{fileName}.png");
                                    File.Move($"{pakDirectory}/{pakContentFile.fileName}.png", $"{pakDirectory}{fileName}.png");
                                }
                                fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.lst");
                                lstStream.Position = 0;
                                lstStream.CopyTo(fileStream);
                                lstStream.Dispose();
                            }
                            catch
                            {
                                pakContentFile.fileStream.Position = 0;
                                fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.font");
                                pakContentFile.fileStream.CopyTo(fileStream);
                            }
                        }
                        break;
                    case "Engine.Media.StreamingSource":
                        fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}.ogg");
                        pakContentFile.fileStream.CopyTo(fileStream);
                        break;
                    default:
                        throw new Exception("发现无法识别的文件类型:"+pakContentFile.typeName+"\t文件名称:"+pakContentFile.fileName);
                        //fileStream = CreateFile($"{pakDirectory}/{pakContentFile.fileName}");
                        //pakContentFile.fileStream.CopyTo(fileStream);
                        //break;
                }
                fileStream.Dispose();
                pakContentFile.fileStream.Dispose();
            }
        }
        private static FileStream CreateFile(string file)
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
        private static bool PngSave(Stream stream, Stream pngStream)
        {
            BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8, true);
            binaryReader.ReadByte();
            int width = binaryReader.ReadInt32();
            int height = binaryReader.ReadInt32();
            binaryReader.ReadInt32();
            ImageInfo imageInfo = new ImageInfo(width, height, 8, true, false, false);
            PngWriter pngWriter = new PngWriter(pngStream, imageInfo);
            pngWriter.ShouldCloseStream = false;
            byte[] array = new byte[4 * width];
            for (int i = 0; i < height; i++)
            {
                int num2 = 0;
                for (int j = 0; j < width; j++)
                {
                    uint color = binaryReader.ReadUInt32();
                    array[num2++] = (byte)color;
                    array[num2++] = (byte)(color >> 8);
                    array[num2++] = (byte)(color >> 16);
                    array[num2++] = (byte)(color >> 24);
                }
                pngWriter.WriteRowByte(array, i);
            }
            bool b = false;
            try
            {
                binaryReader.ReadByte();
                b = true;
            }
            catch
            {
            }
            binaryReader.Dispose();
            pngWriter.End();
            return b;
        }
        private static void TextSave(Stream stream, Stream textStream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            string text = binaryReader.ReadString();
            binaryReader.Dispose();
            textStream.Write(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetBytes(text).Length);
        }
        private static void SoundSave(Stream stream, Stream wavStream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            bool flat = binaryReader.ReadBoolean();
            int channelsCount = binaryReader.ReadInt32();
            int samplingFrequency = binaryReader.ReadInt32();
            int bytesCount = binaryReader.ReadInt32();
            byte[] array = new byte[bytesCount];
            if (stream.Read(array, 0, array.Length) != array.Length)
            {
                throw new Exception("还原wav文件错误");
            }
            BinaryWriter binaryWriter = new BinaryWriter(wavStream);
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
            binaryWriter.Dispose();
            wavStream.Dispose();
        }
        private static void FontSave(Stream stream, Stream lstStream)
        {
            EngineBinaryReader binaryReader = new EngineBinaryReader(stream, false); 
            int num = binaryReader.ReadInt32();
            string data = "";
            data += num + "\n";
            for (int i = 0; i < num; i++)
            {
                data += binaryReader.ReadChar() + "\t";
                data += binaryReader.ReadSingle() + "\t";
                data += binaryReader.ReadSingle() + "\t";
                data += binaryReader.ReadSingle() + "\t";
                data += binaryReader.ReadSingle() + "\t";
                data += binaryReader.ReadSingle() + "\t";
                data += binaryReader.ReadSingle() + "\t";
                data += binaryReader.ReadSingle() + "\n";
            }
            data += binaryReader.ReadSingle() + "\n";
            data += binaryReader.ReadSingle() + "\t" + binaryReader.ReadSingle()+"\n";
            data += binaryReader.ReadSingle() + "\n";
            data += binaryReader.ReadChar();
            lstStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
        }
        private static void SCModelSave(Stream stream, Stream daeStream)
        {
            ModelData data = ModelContentReader.Read(stream, out bool b);
            stream.Dispose();
            ColladaEx ex = new ColladaEx();
            ex.AddModel(data);
            ex.Save(daeStream);
        }


    }
}
