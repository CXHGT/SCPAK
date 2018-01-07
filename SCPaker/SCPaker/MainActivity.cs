using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.V4.View;
using Android.Content.PM;
using System;
using static Android.Widget.AdapterView;
using System.IO;
using System.Threading.Tasks;
using SCPAK;
using static System.Console;
using Android.Content;
using SCPaker.BlocksDataEdit;

namespace SCPaker
{
    [Activity(Label = "SCPaker", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        DrawerLayout drawerLayout;
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
            SetContentView (Resource.Layout.Main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.Main_Toolbar);
            SetSupportActionBar(toolbar);
            this.drawerLayout = FindViewById<DrawerLayout>(Resource.Id.Main_DrawerLayout);
            Android.Support.V7.App.ActionBar actionbar = SupportActionBar;
            if (actionbar != null)
            {
                actionbar.SetDisplayHomeAsUpEnabled(true);
                actionbar.SetHomeAsUpIndicator(Resource.Drawable.menu);
            }
            listView = FindViewById<ListView>(Resource.Id.Main_ListView);
            mainListView = new MainListView(this);
            mainListView.UpDirectories(sdcard);
            listView.ItemClick += new EventHandler<ItemClickEventArgs>(ListView_Item_Click);
            listView.ItemLongClick += new EventHandler<ItemLongClickEventArgs>(ListView_Item_Long_Click);
        }
        private void ListView_Item_Long_Click(object sender, ItemLongClickEventArgs e)
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

        private void ListView_Item_Click(object sender, ItemClickEventArgs e)
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
        }
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back && e.Action == KeyEventActions.Down)
            {
                mainListView.UpThisDirectories(listDirectory);
            }
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(GravityCompat.Start);
                    break;
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
                        .SetMessage($"作者：攻守兼备 \n\n贴吧id：修改是种乐趣 \n\nQQ：3052400179\n\n版本号：{packageInfo}")
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
            }
            return true;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }
    }
}

