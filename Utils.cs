using Life;
using System;

namespace MyPresto
{
    abstract class Utils
    {
        public static int getIconId(int itemId)
        {
            int iconId = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(itemId).icon);
            return iconId >= 0 ? iconId : getIconId(1112);
        }
    }
}
