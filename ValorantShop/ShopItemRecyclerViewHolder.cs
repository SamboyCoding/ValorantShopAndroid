using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace ValorantShop
{
    internal class ShopItemRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public ImageView ShopItemImage;
        public TextView ShopItemName;
        public TextView ShopItemPrice;

        public ShopItemRecyclerViewHolder(View itemView) : base(itemView)
        {
            ShopItemImage = itemView.FindViewById<ImageView>(Resource.Id.shop_item_image);
            ShopItemName = itemView.FindViewById<TextView>(Resource.Id.shop_item_name);
            ShopItemPrice = itemView.FindViewById<TextView>(Resource.Id.shop_item_price);
        }

    }
}