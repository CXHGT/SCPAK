using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public class Pak
    {
        public Pak(string PakDirectory)
        {
            if (!Directory.Exists(PakDirectory))
            {
                throw new DirectoryNotFoundException("将要封包的文件夹不存在");
            }
            List<ContentFileInfo> list = new List<ContentFileInfo>();
            list = ContentFiles(list, PakDirectory);
            List<long> list2 = new List<long>();
            new DirectoryInfo(PakDirectory);
            if (PakDirectory.EndsWith("/") || PakDirectory.EndsWith("\\"))
            {
                PakDirectory = PakDirectory.Substring(0, PakDirectory.Length - 1);
            }
            FileStream fileStream;
            if (File.Exists(PakDirectory + ".pak"))
            {
                if (File.Exists(PakDirectory + ".pak.bak")) File.Delete(PakDirectory + ".pak.bak");
                File.Move(PakDirectory + ".pak", PakDirectory + ".pak.bak");
            }
            fileStream = new FileStream(PakDirectory + ".pak", FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8, true);
            binaryWriter.Write((byte)'P');
            binaryWriter.Write((byte)'A');
            binaryWriter.Write((byte)'K');
            binaryWriter.Write((byte)'\0');
            binaryWriter.Write(0);
            binaryWriter.Write(list.Count);
            foreach (ContentFileInfo current in list)
            {
                if(current.typeName == "Engine.Graphics.Texture2D")
                {
                    string[] fileN = current.fileName.Substring(PakDirectory.Length + 1, current.fileName.Length - PakDirectory.Length - 1).Split('/');
                    string fileN1 = fileN[fileN.Length - 1];
                    if(fileN1[0] == '!')
                    {
                        string fileName = "";
                        for (int i = 0; i < fileN.Length; i++)
                        {
                            if (i + 1 == fileN.Length)
                                fileName += "/" + fileN1.Substring(1);
                            else
                                fileName += "/" + fileN[i];
                        }
                        binaryWriter.Write(fileName.Substring(1));
                    }
                    else
                    {
                        binaryWriter.Write(current.fileName.Substring(PakDirectory.Length + 1, current.fileName.Length - PakDirectory.Length - 1));
                    }
                    goto IL_00;
                }
                binaryWriter.Write(current.fileName.Substring(PakDirectory.Length + 1, current.fileName.Length - PakDirectory.Length - 1));
                IL_00:
                binaryWriter.Write(current.typeName);
                list2.Add(binaryWriter.BaseStream.Position);
                binaryWriter.Write(0);
                binaryWriter.Write(0);
            }
            long position = binaryWriter.BaseStream.Position;
            binaryWriter.BaseStream.Position = 4L;
            binaryWriter.Write((int)position);
            long num = position;
            int num2 = 0;
            long num3;
            foreach (ContentFileInfo info in list)
            {
                binaryWriter.BaseStream.Position = (int)num;
                binaryWriter.Write((byte)222);
                binaryWriter.Write((byte)173);
                binaryWriter.Write((byte)190);
                binaryWriter.Write((byte)239);
                num3 = binaryWriter.BaseStream.Position;
                Stream stream = PakData._PakData(info.fileName, info.typeName);
                stream.CopyTo(fileStream);
                num = binaryWriter.BaseStream.Position;
                binaryWriter.BaseStream.Position = list2[num2++];
                binaryWriter.Write((int)(num3 - position));
                binaryWriter.Write((int)stream.Length);
            }
            binaryWriter.Dispose();
            fileStream.Dispose();
        }

        private List<ContentFileInfo> ContentFiles(List<ContentFileInfo> list, string PakDirectory)
        {
            foreach (string dire in Directory.GetDirectories(PakDirectory)) list = ContentFiles(list, dire);
            foreach (string file in Directory.GetFiles(PakDirectory))
            {
                string extenName = Path.GetExtension(file);
                string typeName;
                switch (extenName)
                {
                    case ".txt":
                        typeName = "System.String";
                        break;
                    case ".xml":
                        typeName = "System.Xml.Linq.XElement";
                        break;
                    case ".png":
                        if (File.Exists(file.Substring(0, file.Length - 4) + ".lst")) continue;
                        typeName = "Engine.Graphics.Texture2D";
                        break;
                    case ".dae":
                        typeName = "Engine.Graphics.Model";
                        break;
                    case ".fsh":
                        typeName = "Engine.Graphics.Shader";
                        break;
                    case ".lst":
                        typeName = "Engine.Media.BitmapFont";
                        break;
                    case ".font":
                        typeName = "Engine.Media.BitmapFont";
                        break;
                    case ".wav":
                        typeName = "Engine.Audio.SoundBuffer";
                        break;
                    case ".ogg":
                        typeName = "Engine.Media.StreamingSource";
                        break;
                    default:
                        throw new Exception("发现不能识别的文件 :"+file);
                }
                ContentFileInfo contentFileInfo;
                if (typeName == "") continue;
                string Dire = Path.GetDirectoryName(file);
                string[] Arratitem = Dire.Split((char)'\\');
                if (Arratitem.Length > 1)
                {
                    Dire = Arratitem[0];
                    for (int i = 1; i < Arratitem.Length; i++) Dire += "/" + Arratitem[i];
                }
                contentFileInfo.fileName = Dire + "/" + Path.GetFileNameWithoutExtension(file);
                contentFileInfo.typeName = typeName;
                list.Add(contentFileInfo);
            }
            return list;
        }
    }
}
