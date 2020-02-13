using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public static class FontHandler
    {
        public static void WriteFont(Stream mainStream, Stream lstStream, Stream bitmapStream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(mainStream);
            StreamReader streamReader = new StreamReader(lstStream, Encoding.UTF8);
            int num = int.Parse(streamReader.ReadLine());
            binaryWriter.Write(num);
            for (int i = 0; i < num; i++)
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
            Texture2DHandler.WriteTexture2D(mainStream,bitmapStream);
        }
        public static void RecoverFont(Stream lstFileStream, Stream bitmapFileStream,Stream fontStream)
        {
            BinaryReader binaryReader = new BinaryReader(fontStream);
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
            data += binaryReader.ReadSingle() + "\t" + binaryReader.ReadSingle() + "\n";
            data += binaryReader.ReadSingle() + "\n";
            data += binaryReader.ReadChar();
            lstFileStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
            Texture2DHandler.RecoverTexture2D(bitmapFileStream,fontStream);
        }
    }
}
