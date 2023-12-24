using Life;
using Newtonsoft.Json;
using System;
using System.IO;

namespace MyPresto
{
    abstract class Utils
    {
        public static int GetIconId(int itemId)
        {
            int iconId = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(itemId).icon);
            return iconId >= 0 ? iconId : GetIconId(1112);
        }

        public static void SaveBlockedItem()
        {
            string json = JsonConvert.SerializeObject(Main.BlockedItems, Formatting.Indented);
            string filePath = Path.Combine(Main.DirectoryPath, Main.Filename);
            File.WriteAllText(filePath, json);
        }
    }
}
