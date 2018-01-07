using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android;
using Android.App;

namespace SCPaker
{
    class AnimalListAdapter : BaseAdapter<Animal>
    {
        Activity context;
        public List<Animal> Animals;
        public AnimalListAdapter(Activity context, List<Animal> animals) : base()
        {
            this.Animals = animals;
            this.context = context;
        }
        public override Animal this[int position]
        {
            get { return this.Animals[position]; }
        }
        public override int Count
        {
            get { return this.Animals.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this.Animals[position];
            var view = convertView;
            if (convertView == null || !(convertView is LinearLayout))
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.Item, parent, false);
            }
            var imageItem = view.FindViewById(Resource.Id.Item_Image) as ImageView;
            var texttop = view.FindViewById(Resource.Id.Item_TextView_1) as TextView;
            var textbottom = view.FindViewById(Resource.Id.Item_TextView_2) as TextView;
            imageItem.SetImageResource(item.Image);
            texttop.SetText(item.Name, TextView.BufferType.Normal);
            textbottom.SetText(item.Description, TextView.BufferType.Normal);
            return view;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
    class Animal
    {
        public string Name
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public int Image
        {
            get;
            set;
        }
    }
}