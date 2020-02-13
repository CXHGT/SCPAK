using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public static class TextHandler
    {
        public static void RecoverText(Stream targetFileStream, Stream textStream)
        {
            BinaryReader binaryReader = new BinaryReader(textStream, Encoding.UTF8, true);
            binaryReader.BaseStream.Position = 0;
            string text = binaryReader.ReadString();
            targetFileStream.Write(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetBytes(text).Length);
        }
        public static void WriteText(Stream mainStream, Stream textStream)
        {
            byte[] array = new byte[textStream.Length];
            textStream.Read(array, 0, (int)textStream.Length);
            BinaryWriter binaryWriter = new BinaryWriter(mainStream, Encoding.UTF8,true);
            binaryWriter.Write(Encoding.UTF8.GetString(array));
        }
    }
}
