using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SCPaker.BlocksDataEdit
{
    public class ItemListAdaoter : BaseAdapter<Item>
    {
        Activity context;
        public List<Item> Items;
        public ItemListAdaoter(Activity context, List<Item> items) : base()
        {
            this.Items = items;
            this.context = context;
        }
        public override Item this[int position]
        {
            get { return this.Items[position]; }
        }
        public override int Count
        {
            get { return this.Items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Item item = this.Items[position];
            View view = convertView;
            if (convertView == null || !(convertView is LinearLayout))
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.Item, parent, false);
            }
            TextView textView = view.FindViewById<TextView>(Resource.Id.BlocksDataItem_Name);
            EditText editText = view.FindViewById<EditText>(Resource.Id.BlocksDataItem_EditText);
            Button button = view.FindViewById<Button>(Resource.Id.BlocksDataItem_Button);
            textView.Text = item.textName;
            editText.Enabled = item.editTextPermissions;
            editText.Text = item.editText_Text;
            button.Click += new EventHandler(ButtonClick);
            textView.Click += new EventHandler(TextViewClick);
            return view;
        }
        private void TextViewClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}