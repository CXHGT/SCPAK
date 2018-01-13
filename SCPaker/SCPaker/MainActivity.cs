using Android;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Content.PM;
using System;
using static Android.Widget.AdapterView;
using System.IO;
using System.Threading.Tasks;
using SCPAK;

namespace SCPaker
{
    [Activity(Label = "SCPaker", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        public static ListView listView;
        public readonly string sdcard = @"/sdcard";
        public static string[] listFile;
        public static string listDirectory;
        private MainListView mainListView;
        private Handler hander = new Handler();
        private static ProgressDialog progressdialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            //Android.Support.V7.App.ActionBar actionbar = SupportActionBar;
            //if (actionbar != null)
            //{
            //    actionbar.SetDisplayHomeAsUpEnabled(true);
            //    actionbar.SetHomeAsUpIndicator(Resource.Drawable.menu);
            //}
            listView = FindViewById<ListView>(Resource.Id.Main_ListView);
            mainListView = new MainListView(this);
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
                {
                    mainListView.UpDirectories(sdcard);

                }
                else
                {
                    RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, 1);
                }
            }
            else
            {
                mainListView.UpDirectories(sdcard);
            }
            listView.ItemClick += ListViewItemClick;
            listView.ItemLongClick += ListViewItemLongClick;
        }

        private void ListViewItemLongClick(object sender, ItemLongClickEventArgs e)
        {
            if ((new FileInfo((listFile[e.Position])).Attributes & FileAttributes.Directory) != 0)
            {
                Android.Support.V7.App.AlertDialog alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle("打包PAK")
                .SetMessage("你确定生成PAK包在本目录 ?")
                .SetPositiveButton("确定", (s, ItemClickEventArgs) =>
                {
                    progressdialog = new ProgressDialog(this);
                    progressdialog.SetTitle("打包Pak中");
                    progressdialog.SetMessage("请稍等......");
                    progressdialog.SetCancelable(false);
                    progressdialog.Show();
                    Task.Run(() =>
                    {
                        try
                        {
                            new Pak(listFile[e.Position]);
                            hander.Post(() =>
                            {
                                progressdialog.Dismiss();
                                mainListView.UpThisDirectories(listFile[e.Position]);
                                (new Android.Support.V7.App.AlertDialog.Builder(this).SetTitle("打包PAK成功").SetMessage("PAK打包成功，欢迎再次使用！").SetNegativeButton("确定", delegate { }).Create()).Show();
                            });
                        }
                        catch (Exception exception)
                        {
                            hander.Post(() =>
                            {
                                progressdialog.Dismiss();
                                mainListView.UpThisDirectories(listFile[e.Position]);
                                (new Android.Support.V7.App.AlertDialog.Builder(this).SetTitle("打包PAK失败").SetMessage("PAK打包失败，请将错误信息交给作者！\n错误信息 ：" + exception.Message).SetNegativeButton("确定", delegate { }).Create()).Show();
                            });
                        }
                    });
                }).SetNegativeButton("取消", (s, ItemClickEventArgs) => { Toast.MakeText(this, "你已经取消了操作", ToastLength.Short).Show(); }).Create();
                alertDialog.Show();
                mainListView.UpThisDirectories(listFile[e.Position]);
            }
        }

        void ListViewItemClick(object sender, ItemClickEventArgs e)
        {
            {
                if ((new FileInfo((listFile[e.Position])).Attributes & FileAttributes.Directory) != 0)
                {
                    mainListView.UpDirectories(listFile[e.Position]);
                }
                else
                {
                    if (Path.GetExtension(listFile[e.Position]) == ".pak")
                    {
                        Android.Support.V7.App.AlertDialog alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                       .SetTitle("解包PAK")
                       .SetMessage("你确定把PAK包解在本目录 ?\n")
                       .SetPositiveButton("确定", (s, ItemClickEventArgs) =>
                       {
                           progressdialog = new ProgressDialog(this);
                           progressdialog.SetTitle("解包Pak中");
                           progressdialog.SetMessage("请稍等......");
                           progressdialog.SetCancelable(false);
                           progressdialog.Show();
                           Task.Run(() =>
                           {
                               try
                               {
                                   new UnPak(listFile[e.Position]);
                                   hander.Post(() =>
                                   {
                                       progressdialog.Dismiss();
                                       mainListView.UpThisDirectories(listFile[e.Position]);
                                       (new Android.Support.V7.App.AlertDialog.Builder(this).SetTitle("解包PAK成功").SetMessage("PAK解包成功，欢迎再次使用！").SetNegativeButton("确定", delegate { }).Create()).Show();
                                   });
                               }
                               catch (Exception exception)
                               {
                                   hander.Post(() =>
                                   {
                                       progressdialog.Dismiss();
                                       mainListView.UpThisDirectories(listFile[e.Position]);
                                       (new Android.Support.V7.App.AlertDialog.Builder(this).SetTitle("解包包PAK失败").SetMessage("PAK解包失败，请将错误信息交给作者！\n错误信息 ：" + exception.Message).SetNegativeButton("确定", delegate { }).Create()).Show();
                                   });
                               }
                           });
                       })
                       .SetNegativeButton("取消", (s, ItemClickEventArgs) => { Toast.MakeText(this, "你已经取消了操作", ToastLength.Short).Show(); }).Create();
                        alertDialog.Show();
                        mainListView.UpThisDirectories(listFile[e.Position]);
                    }
                }
            };
        }

        bool exiting;
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back && e.Action == KeyEventActions.Down)
            {
                if (listDirectory != sdcard)
                {
                    mainListView.UpThisDirectories(listDirectory);
                }
                else if (exiting)
                {
                    Finish();
                }
                else
                {
                    exiting = true;
                    Toast.MakeText(this, "再按一次退出", ToastLength.Short).Show();
                    var time_timer = new System.Timers.Timer();
                    time_timer.Interval = 2000;
                    time_timer.Enabled = true;
                    time_timer.Elapsed += delegate { exiting = false; };
                }
            }
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_item_1:
                    string packageInfo;
                    try
                    {
                        PackageManager pm = this.PackageManager;
                        PackageInfo info = pm.GetPackageInfo(this.PackageName, 0);
                        packageInfo = info.VersionName;
                    }
                    catch { packageInfo = "发生未知错误！"; }
                    (new Android.Support.V7.App.AlertDialog.Builder(this)
                        .SetTitle("关于")
                     .SetMessage($"作者：攻守兼备 & Lixue\n\n贴吧id：修改是种乐趣 & lixue_jiu\n\nQQ：3052400179 & 2919560136\n\n版本号：{packageInfo}")
                        .SetPositiveButton("确定", (s, ItemClickEventArgs) => { }).Create()).Show();
                    break;
                case Resource.Id.menu_item_2:
                    Toast.MakeText(this, "拜拜！", ToastLength.Short).Show();
                    Finish();
                    break;
                case Resource.Id.menu_item_3:
                    (new Android.Support.V7.App.AlertDialog.Builder(this)
                     .SetTitle("关于模型bug")
                     .SetMessage("模型解析方法由Lixue提供，由于计算机浮点运算多多少少会丢失一些数值，这是必不可免的，所以游戏内有时会发现贴图有问题！\n简单的解决方法 :在对应的模型贴图前面加个\"!\"，英文符号非中文，若已经存在，则请忽略！")
                     .SetPositiveButton("确定", (s, ItemClickEventArgs) => { }).Create()).Show();
                    break;
                case Resource.Id.menu_item_4:
                    (new Android.Support.V7.App.AlertDialog.Builder(this)
                     .SetTitle("历史版本及更新内容")
                     .SetMessage("测试版1.0.0:\t*解析png图片及文本文件\n\n1.0.0:\t*修复bug及模型伪解析\n\n1.1.0:\t*修复bug及模型文件解析\n\n1.2.0:\t*字体库文件解析\n\n1.3.1:\t*修复bug及音频文件解析\n\n1.3.2:\t*由lixue_jiu美化,并且修复字体库解析bug及拇指玩版本字体库无法解析问题")
                     .SetPositiveButton("确定", (s, ItemClickEventArgs) => { }).Create()).Show();
                    break;
                case Resource.Id.tutorial:
                    (new Android.Support.V7.App.AlertDialog.Builder(this)
                     .SetTitle("使用说明")
                     .SetMessage(Resources.GetString(Resource.String.Introduce))
                     .SetPositiveButton("确定", (s, ItemClickEventArgs) => { }).Create()).Show();
                    break;
            }
            return true;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            switch (requestCode)
            {
                case 1:
                    if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                    {
                        mainListView.UpDirectories(sdcard);
                    }
                    else
                    {
                        Finish();
                    }
                    break;
            }
        }
    }
}

