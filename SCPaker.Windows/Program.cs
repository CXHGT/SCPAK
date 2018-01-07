using static System.Console;
using System.IO;
using System;
using SCPAK;

namespace SCPaker.Windows
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                WriteLine("将pak文件拖动到程序图标上以解包");
                WriteLine("将文件夹拖动到程序图标上以打包");
                WriteLine("按Enter键退出......");
                ReadKey();
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
                        WriteLine($"发生了一个错误：{e.Message}");
                        WriteLine("按Enter键退出......");
                        ReadKey();
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
                        WriteLine($"发生了一个错误：{e.Message}");
                        WriteLine("按Enter键退出......");
                        ReadKey();
                    }
                }
            }
        }
    }
}
