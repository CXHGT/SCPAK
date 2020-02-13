using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public static class ShaderHandler
    {
        public static void WriteShader(Stream mainStream, Stream vertStream,Stream fragStream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(mainStream, Encoding.UTF8, true);
            byte[] array = new byte[vertStream.Length];
            vertStream.Read(array, 0, (int)vertStream.Length);
            binaryWriter.Write(Encoding.UTF8.GetString(array));
            array = new byte[fragStream.Length];
            fragStream.Read(array, 0, (int)fragStream.Length);
            binaryWriter.Write(Encoding.UTF8.GetString(array));
        }
        public static void RecoverShader(Stream vertFileStream, Stream fragFileStream, Stream shaderStream)
        {
            BinaryReader binaryReader = new BinaryReader(shaderStream, Encoding.UTF8, true);
            string vertexShaderCode = binaryReader.ReadString();
            string pixelShaderCode = binaryReader.ReadString();
            vertFileStream.Write(Encoding.UTF8.GetBytes(vertexShaderCode), 0, Encoding.UTF8.GetBytes(vertexShaderCode).Length);
            fragFileStream.Write(Encoding.UTF8.GetBytes(pixelShaderCode),0,Encoding.UTF8.GetBytes(pixelShaderCode).Length);
            //Console.WriteLine(binaryReader.ReadInt32());
        }
    }
}
