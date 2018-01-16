using Android.App;
using System.Collections.Generic;
using System.IO;

namespace SCPaker
{
    public class MainListView
    {
        private Activity activity;

        public MainListView(Activity activity)
        {
            this.activity = activity;
        }

        public void UpThisDirectories(string d)
        {
            string[] dire = d.Split('/');
            string diress = "/";
            for (int i = 1; i < dire.Length - 1; i++)
                diress += $"{dire[i]}/";
            string[] dir = diress.Split('/');
            if (dir.Length >= 3 && dir[dir.Length - 1] == "")
            {
                diress = diress.Substring(0, diress.Length - 1);
            }
            UpDirectories(diress);
        }

        public void UpDirectories(string directory)
        {
            MainActivity.listDirectory = directory;
            MainActivity.listFile = Directory.GetDirectories(directory);
            string[] file = new string[MainActivity.listFile.Length + Directory.GetFiles(directory).Length];
            MainActivity.listFile.CopyTo(file, 0);
            Directory.GetFiles(directory).CopyTo(file, MainActivity.listFile.Length);
            MainActivity.listFile = file;

            var list = new List<string>(file);
            list.Sort((a, b) =>
            {
                bool ad = Directory.Exists(a);
                bool bd = Directory.Exists(b);
                if (ad == bd)
                {
                    return string.Compare(a, b, System.StringComparison.Ordinal);
                }
                if (ad)
                {
                    return -1;
                }
                return 1;
            });
            MainActivity.listFile = list.ToArray();

            List<Animal> files = new List<Animal>(MainActivity.listFile.Length);
            for (int i = 0; i < MainActivity.listFile.Length; i++)
            {
                FileInfo info = new FileInfo(MainActivity.listFile[i]);
                if (File.Exists(MainActivity.listFile[i]))
                {
                    string name = Path.GetExtension(MainActivity.listFile[i]);
                    int r = fileImage(name);
                    files.Add(new Animal
                    {
                        Name = Path.GetFileName(MainActivity.listFile[i]),
                        Description = $"文件大小：{fileSize(info.Length)}",
                        Image = r
                    });
                }
                else if (Directory.Exists(MainActivity.listFile[i]))
                {
                    files.Add(new Animal
                    {
                        Name = Path.GetFileName(MainActivity.listFile[i]),
                        Description = info.DirectoryName,
                        Image = Resource.Drawable.ic_folder
                    });
                }
            }
            AnimalListAdapter adapter = new AnimalListAdapter(activity, files);
            MainActivity.listView.Adapter = adapter;
            MainActivity.listFile.Clone();
        }

        int fileImage(string suffixName)
        {
            switch (suffixName)
            {
                case ".pak":
                    return Resource.Drawable.ic_database;
                case ".zip":
                case ".tga":
                case ".7z":
                case ".rar":
                    return Resource.Drawable.ic_zip;
                case ".txt":
                case ".xml":
                    return Resource.Drawable.ic_text;
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                    return Resource.Drawable.ic_image;
                case ".apk":
                    return Resource.Drawable.ic_android;
                default:
                    return Resource.Drawable.ic_unkown;
            }
        }

        string fileSize(long i)
        {
            if (i < 1024)
            {
                return i + "B";
            }
            if (i < 1024 * 1024)
            {
                return (i / 1024f).ToString("0.00") + "KB";
            }
            if (i < 1024f * 1024f * 1024f)
            {
                return (i / (1024f * 1024f)).ToString("0.00") + "MB";
            }
            if (i < (long)1024 * 1024 * 1024 * 1024)
            {
                return (i / (1024f * 1024f * 1024f)).ToString("0.00") + "GB";
            }
            return "发生未知错误！";
        }
    }
}