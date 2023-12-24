using Life;
using Life.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using MyMenu.Entities;
using Life.Network;
using UIPanelManager;
using Life.InventorySystem;

namespace MyPresto
{
    public class Main : Plugin
    {
        private readonly EventsTriggers Events;
        public static List<int> BlockedItems = new List<int>();

        public Main(IGameAPI api): base(api)
        {
            Events = new EventsTriggers();

            BlockedItems.Add(1154);
            //MyMenu
            try
            {
                Section section = new Section(Section.GetSourceName(), Section.GetSourceName(), "v1.0.0", "Aarnow");
                Action<UIPanel> action = ui => Open(section.GetPlayer(ui));
                section.OnlyAdmin = true;
                section.Line = new UITabLine(section.Title, action);
                section.Insert(true);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            Debug.Log($"Plugin \"MyPresto\" initialisé avec succès.");
        }

        public void Open(Player player)
        {
            UIPanel panel = new UIPanel("MyPresto", UIPanel.PanelType.TabPrice).SetTitle($"MyPresto");

            foreach (int i in BlockedItems)
            {
                Item item = Nova.man.item.GetItem(i);
                panel.AddTabLine($"{item.itemName}", $"<color={PanelManager.Colors[NotificationManager.Type.Error]}>Bloqué</color>", getIconId(i), ui =>
                {
                    BlockedItems.Remove(i);
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

        public void AddBlockedItem(Player player)
        {
            UIPanel panel = new UIPanel("MyPresto", UIPanel.PanelType.Input).SetTitle($"MyPresto");

            panel.inputPlaceholder = "ID de l'objet à bloquer";

            panel.AddButton("Confirmer", ui =>
            {
                if(int.TryParse(ui.inputText, out int id))
                {
                    Item item = Nova.man.item.GetItem(id);
                    if (item != null)
                    {
                        BlockedItems.Add(id);
                        PanelManager.NextPanel(player, ui, () => Open(player));
                    }
                    else PanelManager.Notification(player, "Erreur", "Aucun objet ne correspond à cette identifiant", NotificationManager.Type.Error);
                } else PanelManager.Notification(player, "Erreur", "Veuillez renseigner l'indentifiant de l'objet que vous souhaitez bloquer.", NotificationManager.Type.Error);
            });
            panel.AddButton("Annuler", ui => PanelManager.NextPanel(player, ui, () => Open(player)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static int getIconId(int itemId)
        {
            int iconId = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(itemId).icon);
            return iconId >= 0 ? iconId : getIconId(1112);
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            Events.Init(Nova.server);
        }
    }
}