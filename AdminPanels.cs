using Life;
using Life.InventorySystem;
using Life.Network;
using Life.UI;
using UIPanelManager;

namespace MyPresto
{
    abstract class AdminPanels
    {
        public static void Open(Player player)
        {
            UIPanel panel = new UIPanel("MyPresto", UIPanel.PanelType.TabPrice).SetTitle($"MyPresto");

            foreach (int i in Main.BlockedItems)
            {
                Item item = Nova.man.item.GetItem(i);
                panel.AddTabLine($"{item.itemName}", $"<color={PanelManager.Colors[NotificationManager.Type.Error]}>Bloqué</color>", Utils.getIconId(i), ui =>
                {
                    Main.BlockedItems.Remove(i);
                });
            }

            panel.AddButton("Ajouter", ui => PanelManager.NextPanel(player, ui, () => AddBlockedItem(player)));
            panel.AddButton("Supprimer", ui =>
            {
                ui.SelectTab();
                Open(player);
                PanelManager.NextPanel(player, ui, () => Open(player));
            });
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void AddBlockedItem(Player player)
        {
            UIPanel panel = new UIPanel("MyPresto", UIPanel.PanelType.Input).SetTitle($"MyPresto");

            panel.inputPlaceholder = "ID de l'objet à bloquer";

            panel.AddButton("Confirmer", ui =>
            {
                if (int.TryParse(ui.inputText, out int id))
                {
                    Item item = Nova.man.item.GetItem(id);
                    if (item != null)
                    {
                        Main.BlockedItems.Add(id);
                        PanelManager.NextPanel(player, ui, () => Open(player));
                    }
                    else PanelManager.Notification(player, "Erreur", "Aucun objet ne correspond à cette identifiant", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Veuillez renseigner l'indentifiant de l'objet que vous souhaitez bloquer.", NotificationManager.Type.Error);
            });
            panel.AddButton("Annuler", ui => PanelManager.NextPanel(player, ui, () => Open(player)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
