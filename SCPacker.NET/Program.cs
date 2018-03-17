using System;
using System.IO;
using SCPAK;

namespace SCPacker.NET
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("将pak文件拖动到程序图标上以解包");
                Console.WriteLine("将文件夹拖动到程序图标上以打包");
                Console.WriteLine("按Enter键退出......");
                Console.ReadKey();
            }
            else
            {
                string text = args[0];
                if (Directory.Exists(text))
                {
                    try
                    {
                        new Pak(text);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"发生了一个错误：{e.Message}");
                        Console.WriteLine("按Enter键退出......");
                        Console.ReadKey();
                    }
                }
                else
                {
                    //new UnPak(text);
                    try
                    {
                        new UnPak(text);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"发生了一个错误：{e.Message}");
                        Console.WriteLine("按Enter键退出......");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
