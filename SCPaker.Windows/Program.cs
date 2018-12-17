using static System.Console;
using System.IO;
using System;
using SCPAK;

namespace SCPaker.Windows
{
    class Program
    {
        const String information = @"SCPAK.Windows 版本1.1.0
作者：守望地雷（已失踪）、lixue（也经常失踪）";

        static void Main(string[] args)
        {
            Console.WriteLine(information);
            if (args.Length < 1)
            {
                WriteLine("将pak文件拖动到程序图标上以解包");
                WriteLine("将文件夹拖动到程序图标上以打包");
                Exit();
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
                        WriteLine(e.Message);
                        if (e.InnerException != null)
                        {
                            if (e.InnerException is NotSupportedException)
                                WriteLine("不支持的dea文件版本");
                            else if (e.InnerException is IndexOutOfRangeException)
                                WriteLine("如果你手改了dae文件，请停止这种行为，并且使用blender导出的dae文件");
                            else
                                WriteLine("发生了未知错误，请联系开发者");
                            Exit(e.InnerException);
                        }
                        else
                        {
                            Exit(e);
                        }
                    }
                }
                else
                {
                    try
                    {
                        new UnPak(text);
                    }
                    catch (Exception e)
                    {
                        WriteLine(e.Message);
                        if (e.InnerException != null)
                        {
                            WriteLine("发生了未知错误，请联系开发者");
                            Exit(e.InnerException);
                        }
                        else
                        {
                            Exit(e);
                        }
                    }
                }
            }
        }

        static void Exit(Exception e)
        {
            WriteException(e);
            Exit();
        }

        static void Exit()
        {
            Write("按任意建退出");
            ReadKey();
        }

        static void WriteException(Exception e)
        {
            WriteLine(e.Message);
            WriteLine(e.StackTrace);
        }
    }
}
