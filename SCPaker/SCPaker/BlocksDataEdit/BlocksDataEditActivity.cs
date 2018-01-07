using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;
using System.IO;
using System;

namespace SCPaker.BlocksDataEdit
{
    [Activity(Label = "BlocksData编辑器", Theme = "@style/AppTheme")]
    public class BlocksDataEditActivity : AppCompatActivity
    {
        ListView listView;
        Spinner spinner;
        private string fileDirectory;
        private List<Data> BlocksData = new List<Data>();




        List<string> type = new List<string>();
        List<List<string>> dataBase = new List<List<string>>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.BlocksDataEditActivity);
            listView = FindViewById<ListView>(Resource.Id.BlocksData_ListView);
            spinner = FindViewById<Spinner>(Resource.Id.BlocksData_Spinner);
            spinner.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(SpinnerClick);
            FileStream fileStream = File.OpenRead("app:/BlocksData.txt");
            StreamReader sr = new StreamReader(fileStream);
            while (true)
            {
                string content = sr.ReadLine();
                if (content == null || content.Length == 0) break;
                string[] str = content.Split(';');
                Data data = new Data
                {
                    ClassName = str[0],
                    ChineseName = str[1],
                    EditTextPower = bool.Parse(str[2]),
                    ButtonClickPower = bool.Parse(str[3]),
                };
                string[] shortcut = str[4].Split(',');
                List<string> _shortcut = new List<string>();
                foreach(string shortcutItem in shortcut)
                {
                    _shortcut.Add(shortcutItem);
                }
                data.Shortcut = _shortcut;

                this.BlocksData.Add(data);
            }
            sr.Dispose();
            fileStream.Dispose();



            fileDirectory = Intent.Extras.GetString("File");
            Updata();
        }
        /*private string getChineseName(string ClassName)
        {

        }*/


        private void SpinnerClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
        }
        private void Updata()
        {
            FileStream fileStream = File.OpenRead(fileDirectory);
            StreamReader sr = new StreamReader(fileStream);
            foreach (string typeText in sr.ReadLine().Split(';'))
            {
                this.type.Add(typeText);
            }
            while (true)
            {
                string content = sr.ReadLine();
                if (content == null) break;
                if (content.Length == 0) continue;
                List<string> list = new List<string>();
                foreach (string str in content.Split(';'))
                {
                    list.Add(str);
                }
                this.dataBase.Add(list);
            }
            sr.Dispose();
            fileStream.Dispose();
            UpDataListView(1);
        }
        private void UpDataListView(int position)
        {
            List<Item> items = new List<Item>();
            foreach(string item_string in dataBase[position])
            {
                int i = dataBase[position].IndexOf(item_string);
                Item item = new Item();
                


            }

        }
    }
}