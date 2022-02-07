using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using ValorantShop.Models;

namespace ValorantShop
{
    internal class ShopItemRecyclerAdapter : ListAdapter
    {
        public ShopItemRecyclerAdapter() : base(new DiffCallback())
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var ourVh = holder as ShopItemRecyclerViewHolder;
            if (ourVh == null)
                return;

            var item = GetItem(position) as ShopItem;

            ourVh.ShopItemName.Text = item.Name;
            ourVh.ShopItemPrice.Text = $"{item.Price} VP";
            
            //Queue download
            if(item.ImageUrl != null)
                new DownloadImageTask(ourVh.ShopItemImage).Execute(item.ImageUrl);
            else
            {
                Log.Error("ShopItemRecyclerAdapter", $"No image for {item.Name}?");
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.recycler_item_store_item, parent, false);

            ShopItemRecyclerViewHolder holder = new(itemView);

            return holder;
        }

        class DiffCallback : DiffUtil.ItemCallback
        {
            public override bool AreContentsTheSame(Java.Lang.Object p0, Java.Lang.Object p1)
            {
                if(p0 is not ShopItem a)
                    return false;
                if (p1 is not ShopItem b)
                    return false;

                return a == b;
            }

            public override bool AreItemsTheSame(Java.Lang.Object p0, Java.Lang.Object p1)
            {
                if (p0 is not ShopItem a)
                    return false;
                if (p1 is not ShopItem b)
                    return false;

                return a == b;
            }
        }
    }
}