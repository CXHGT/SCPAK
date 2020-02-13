using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPAK
{
    public static class Texture2DHandler
    {
        public static void WriteTexture2D(Stream mainStream, Stream BitmapStream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(mainStream, Encoding.UTF8, true);
            Bitmap bitmap = new Bitmap(BitmapStream);
            binaryWriter.Write(false);
            binaryWriter.Write(bitmap.Width);
            binaryWriter.Write(bitmap.Height);
            binaryWriter.Write(1);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for(int x = 0; x < bitmap.Width; x++)
                {
                    System.Drawing.Color color = bitmap.GetPixel(x,y);
                    binaryWriter.Write((uint)(color.A << 24 | color.B << 16 | color.G << 8 | color.R));
                }
            }
            bitmap.Dispose();
        }
        public static void RecoverTexture2D(Stream targetFileStream, Stream texture2DStream)
        {
            BinaryReader binaryReader = new BinaryReader(texture2DStream, Encoding.UTF8, true);
            binaryReader.ReadByte();
            int width = binaryReader.ReadInt32();
            int height = binaryReader.ReadInt32();
            binaryReader.ReadInt32();
            Bitmap bitmap = new Bitmap(width,height);
            for(int y = 0; y < height; y++)
            {
                for(int x = 0;x < width; x++)
                {
                    uint colorData = binaryReader.ReadUInt32();
                    bitmap.SetPixel(x,y,System.Drawing.Color.FromArgb((byte)(colorData>>24),(byte)(colorData),(byte)(colorData>>8),(byte)(colorData>>16)));
                }
            }
            bitmap.Save(targetFileStream,ImageFormat.Png);
            bitmap.Dispose();
        }
        

    }
}
