using System;
using System.Collections.Generic;
using System.IO;

namespace Engine
{
    public class Image
    {
        public readonly int Width;

        public readonly int Height;

        public readonly Color[] Pixels;

        public Image(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Pixels = new Color[width * height];
        }
        public static int Max(int x1, int x2)
        {
            if (x1 <= x2)
            {
                return x2;
            }
            return x1;
        }
        public static IEnumerable<Image> GenerateMipmaps(Image image, int maxLevelsCount = 2147483647)
        {
            int num = image.Width;
            int num2 = image.Height;
            for (int i = 0; i < maxLevelsCount; i++)
            {
                yield return image;
                int num3 = num;
                int num4 = num2;
                num = Max(num3 / 2, 1);
                num2 = Max(num4 / 2, 1);
                Image image2 = new Image(num, num2);
                int num5 = num3 / num;
                int num6 = num4 / num2;
                if (num5 == 2 && num6 == 2)
                {
                    int j = 0;
                    int num7 = 0;
                    while (j < num2)
                    {
                        int num8 = j * 2 * num3;
                        int k = 0;
                        while (k < num)
                        {
                            Color color = image.Pixels[num8];
                            Color color2 = image.Pixels[num8 + 1];
                            Color color3 = image.Pixels[num8 + num3];
                            Color color4 = image.Pixels[num8 + num3 + 1];
                            byte r = (byte)((color.R + color2.R + color3.R + color4.R) / 4);
                            byte g = (byte)((color.G + color2.G + color3.G + color4.G) / 4);
                            byte b = (byte)((color.B + color2.B + color3.B + color4.B) / 4);
                            byte a = (byte)((color.A + color2.A + color3.A + color4.A) / 4);
                            image2.Pixels[num7] = new Color(r, g, b, a);
                            k++;
                            num8 += 2;
                            num7++;
                        }
                        j++;
                    }
                }
                else if (num5 == 2 && num6 == 1)
                {
                    int l = 0;
                    int num9 = 0;
                    while (l < num2)
                    {
                        int num10 = l * num3;
                        int m = 0;
                        while (m < num)
                        {
                            Color color5 = image.Pixels[num10];
                            Color color6 = image.Pixels[num10 + 1];
                            byte r2 = (byte)((color5.R + color6.R) / 2);
                            byte g2 = (byte)((color5.G + color6.G) / 2);
                            byte b2 = (byte)((color5.B + color6.B) / 2);
                            byte a2 = (byte)((color5.A + color6.A) / 2);
                            image2.Pixels[num9] = new Color(r2, g2, b2, a2);
                            m++;
                            num10 += 2;
                            num9++;
                        }
                        l++;
                    }
                }
                else
                {
                    if (num5 != 1 || num6 != 2)
                    {
                        break;
                    }
                    int n = 0;
                    int num11 = 0;
                    while (n < num2)
                    {
                        int num12 = n * 2 * num3;
                        int num13 = 0;
                        while (num13 < num)
                        {
                            Color color7 = image.Pixels[num12];
                            Color color8 = image.Pixels[num12 + num3];
                            byte r3 = (byte)((color7.R + color8.R) / 2);
                            byte g3 = (byte)((color7.G + color8.G) / 2);
                            byte b3 = (byte)((color7.B + color8.B) / 2);
                            byte a3 = (byte)((color7.A + color8.A) / 2);
                            image2.Pixels[num11] = new Color(r3, g3, b3, a3);
                            num13++;
                            num12++;
                            num11++;
                        }
                        n++;
                    }
                }
                image = image2;
            }
            yield break;
        }
    }
}
