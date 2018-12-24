using static System.Console;
using System.IO;
using System;
using SCPAK;

namespace SCPaker.Windows
{
    class Program
    {
        const String version = "1.1.4";
        static readonly String information = $@"SCPAK.Windows 版本{version}
作者：守望地雷（已失踪）、lixue_jiu（也经常失踪）
我是lixue_jiu，这个程序原本是守望地雷写的，但是代码质量不敢恭维；
后来地雷失踪了，然后现在我又来修改一番，解决模型方面的问题
程序有问题就找我？我大概不会经常看消息";

        static void Main(string[] args)
        {
            WriteLine(information);
            if (args.Length < 1)
            {
                WriteLine("将pak文件拖动到程序图标上以解包");
                WriteLine("将文件夹拖动到程序图标上以打包");
                Exit();
            }
            else
            {
                WriteLine();
                string text = args[0];
                if (Directory.Exists(text))
                {
#if !DEBUG
                    try
                    {
#endif
                        new Pak(text);
#if !DEBUG
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
#endif
                }
                else
                {
#if !DEBUG
                    try
                    {
#endif
                        new UnPak(text);
#if !DEBUG
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
#endif
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
