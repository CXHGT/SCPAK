using Engine;
using Hjg.Pngcs;
using Hjg.Pngcs.Chunks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCPAK
{
    internal class PakData
    {
        public static Stream _PakData(string fileName, string typeName)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                FileStream fileStream;
                switch (typeName)
                {
                    case "System.String":
                        fileStream = File.OpenRead(fileName + ".txt");
                        TextWriter(memoryStream, fileStream);
                        break;
                    case "System.Xml.Linq.XElement":
                        fileStream = File.OpenRead(fileName + ".xml");
                        TextWriter(memoryStream, fileStream);
                        break;
                    case "Engine.Media.StreamingSource":
                        fileStream = File.OpenRead(fileName + ".ogg");
                        fileStream.CopyTo(memoryStream);
                        break;
                    case "Engine.Graphics.Model":
                        fileStream = File.OpenRead(fileName + ".dae");
                        SCModelWriter(memoryStream, fileStream);
                        break;
                    case "Engine.Graphics.Shader":
                        fileStream = File.OpenRead(fileName + ".fsh");
                        fileStream.CopyTo(memoryStream);
                        break;
                    case "Engine.Audio.SoundBuffer":
                        fileStream = File.OpenRead(fileName + ".wav");
                        SoundWriter(memoryStream, fileStream);
                        break;
                    case "Engine.Graphics.Texture2D":
                        fileStream = File.OpenRead(fileName + ".png");
                        {
                            string fileN = Path.GetFileNameWithoutExtension(fileName + ".png");
                            if (fileN[0] == '!')
                                PngWriter(memoryStream, fileStream, true);
                            else
                                PngWriter(memoryStream, fileStream);
                        }
                        break;
                    case "Engine.Media.BitmapFont":
                        try
                        {
                            fileStream = File.OpenRead(fileName + ".lst");
                        }
                        catch
                        {
                            fileStream = File.OpenRead(fileName + ".font");
                            fileStream.CopyTo(memoryStream);
                            break;
                        }
                        FontWriter(memoryStream, fileStream);
                        {
                            int nameCount = Path.GetFileName(fileName + ".lst").Length;
                            string fileDirectoryName = (fileName + ".lst").Substring(0, (fileName + ".lst").Length - nameCount);

                            FileStream pngStream;
                            if (File.Exists(fileName + ".png"))
                            {
                                pngStream = File.OpenRead(fileName + ".png");
                                PngWriter(memoryStream, pngStream);
                            }
                            else if (File.Exists(fileDirectoryName + '!' + Path.GetFileNameWithoutExtension(fileName + ".lst") + ".png"))
                            {
                                pngStream = File.OpenRead(fileDirectoryName + '!' + Path.GetFileNameWithoutExtension(fileName + ".lst") + ".png");
                                PngWriter(memoryStream, fileStream, true);
                            }
                            else throw new Exception("字体库错误！！！");
                            pngStream.Dispose();
                        }
                        //fileStream.CopyTo(memoryStream);
                        break;
                    default:
                        throw new Exception("发现不能识别的文件 :" + fileName + "\n文件类型 :" + typeName);
                }
                fileStream.Dispose();
                memoryStream.Position = 0L;
                return memoryStream;
            }catch(Exception e)
            {
                throw new Exception("文件写入错误 :"+fileName+"\t类型 :"+typeName+"\n具体错误信息 :"+e.Message);
            }
        }
        private static void TextWriter(MemoryStream memoryStream, FileStream fileStream)
        {
            byte[] array = new byte[fileStream.Length];
            fileStream.Read(array, 0, (int)fileStream.Length);
            EngineBinaryWriter binaryWriter = new EngineBinaryWriter(memoryStream, false);
            binaryWriter.Write(Encoding.UTF8.GetString(array));
        }
        private static bool IsPowerOf2(long x)
        {
            return x > 0L && (x & x - 1L) == 0L;
        }
        private static void PngWriter(MemoryStream memoryStream, FileStream fileStream,bool modelpng = false)
        {
            PngReader pngReader = new PngReader(fileStream);
            EngineBinaryWriter binaryWriter = new EngineBinaryWriter(memoryStream, false);
            binaryWriter.Write(false);
            pngReader.ShouldCloseStream = false;
            pngReader.ChunkLoadBehaviour = ChunkLoadBehaviour.LOAD_CHUNK_NEVER;
            pngReader.MaxTotalBytesRead = 9223372036854775807L;
            ImageLines imageLines = pngReader.ReadRowsByte();
            pngReader.End();
            Image image = new Image(pngReader.ImgInfo.Cols, pngReader.ImgInfo.Rows);
            int n = 0;
            for (int i = 0; i < image.Height; i++)
            {
                byte[] array = imageLines.ScanlinesB[i];
                int num = 0;
                for (int j = 0; j < image.Width; j++)
                {
                    byte r = array[num++];
                    byte g = array[num++];
                    byte b = array[num++];
                    byte a = array[num++];
                    image.Pixels[n++] = new Color(r, g, b, a);
                }
            }
            List<Image> list = new List<Image>();
            if (IsPowerOf2(image.Width) && IsPowerOf2(image.Height))
            {
                list.AddRange(Image.GenerateMipmaps(image, 2147483647));
            }
            else
            {
                list.Add(image);
                modelpng = true;
            }
            if(modelpng) goto IL_00;
            binaryWriter.Write(image.Width);
            binaryWriter.Write(image.Height);
            binaryWriter.Write(1);
            for (int i = 0; i < image.Height; i++)
            {
                byte[] array = imageLines.ScanlinesB[i];
                int num = 0;
                for (int j = 0; j < image.Width; j++)
                {
                    byte r = array[num++];
                    byte g = array[num++];
                    byte b = array[num++];
                    byte a = array[num++];
                    binaryWriter.Write(new Color(r, g, b, a).PackedValue);
                }
            }
            return;
            IL_00:
            binaryWriter.Write(list[0].Width);
            binaryWriter.Write(list[0].Height);
            binaryWriter.Write(list.Count);
            foreach (Image current in list)
            {
                for (int j = 0; j < current.Pixels.Length; j++)
                {
                    binaryWriter.Write(current.Pixels[j].PackedValue);
                }
            }
        }
        private static void SoundWriter(MemoryStream memoryStream,FileStream fileStream)
        {
            BinaryReader binaryReader = new BinaryReader(fileStream);
            binaryReader.BaseStream.Position = 22L;
            int channelsCount = binaryReader.ReadInt16();
            int samplingFrequency = binaryReader.ReadInt32();
            binaryReader.BaseStream.Position += 12L;
            int bytesCount = binaryReader.ReadInt32();
            byte[] array = new byte[bytesCount];
            if (fileStream.Read(array, 0, array.Length) != array.Length)
            {
                throw new Exception("解析wav文件错误");
            }
            binaryReader.Dispose();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(false);
            binaryWriter.Write(channelsCount);
            binaryWriter.Write(samplingFrequency);
            binaryWriter.Write(bytesCount);
            binaryWriter.Write(array);
        }

        private static void FontWriter(MemoryStream memoryStream, FileStream fileStream)
        {
            EngineBinaryWriter binaryWriter = new EngineBinaryWriter(memoryStream,false);
            StreamReader streamReader = new StreamReader(fileStream,Encoding.UTF8);
            int num = int.Parse(streamReader.ReadLine());
            binaryWriter.Write(num);
            for(int i = 0; i < num; i++)
            {
                string[] data = streamReader.ReadLine().Split('\t');
                binaryWriter.Write(char.Parse(data[0]));
                binaryWriter.Write(float.Parse(data[1]));
                binaryWriter.Write(float.Parse(data[2]));
                binaryWriter.Write(float.Parse(data[3]));
                binaryWriter.Write(float.Parse(data[4]));
                binaryWriter.Write(float.Parse(data[5]));
                binaryWriter.Write(float.Parse(data[6]));
                binaryWriter.Write(float.Parse(data[7]));
            }
            binaryWriter.Write(float.Parse(streamReader.ReadLine()));
            string[] floatItem = streamReader.ReadLine().Split('\t');
            binaryWriter.Write(float.Parse(floatItem[0]));
            binaryWriter.Write(float.Parse(floatItem[1]));
            binaryWriter.Write(float.Parse(streamReader.ReadLine()));
            binaryWriter.Write(char.Parse(streamReader.ReadLine()));
        }

        private static void SCModelWriter(MemoryStream memoryStream, FileStream fileStream)
        {
            ModelData modelData = Collada.Load(fileStream);
            ModelContentWriter.Write(memoryStream, modelData, Vector3.One);
        }
    }
}
