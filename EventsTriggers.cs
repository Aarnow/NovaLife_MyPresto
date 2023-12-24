using Life.Network;
using MyEvents;
using System.Linq;
using UIPanelManager;
using System;
using Mirror;

namespace MyPresto
{
    public class EventsTriggers : Events
    {
        public override void OnPlayerSpawnCharacter(Player player)
        {
            base.OnPlayerSpawnCharacter(player);
            PlayerInfo currentPlayer = new PlayerInfo(player.netId, player.conn.connectionId);
            if (currentPlayer != null) Main.PlayerInfos.Add(currentPlayer);
        }

        public override void OnPlayerDisconnect(NetworkConnection conn)
        {
            base.OnPlayerDisconnect(conn);
            PlayerInfo currentPlayer = Main.PlayerInfos.FirstOrDefault(p => p.ConnectionId == conn.connectionId);
            if (currentPlayer != null) Main.PlayerInfos.Remove(currentPlayer);
        }
        public override void OnPlayerMoney(Player player, double amount, string reason)
        {
            base.OnPlayerMoney(player, amount, reason);
            if((reason == "BUY_ITEM_DEPOT_PRESTO"))
            {
                PlayerInfo playerInfo = Main.PlayerInfos.Where(p => p.ConnectionId == player.conn.connectionId).FirstOrDefault();
                if (playerInfo != null)
                {
                    if (Main.BlockedItems.Contains(playerInfo.LastItemReceivedId))
                    {
                        player.setup.inventory.RemoveItem(playerInfo.LastItemReceivedId, playerInfo.LastItemReceivedCount, true);
                        player.AddMoney(Math.Abs(amount), "Remboursement objet bloqué - (Plugin: MyPresto)");
                        PanelManager.Notification(player, "Information", "Cette objet est indisponible à l'achat", Life.NotificationManager.Type.Warning);
                    }
                }
            }
        }

        public override void OnPlayerReceiveItem(Player player, int itemId, int slotId, int number)
        {
            base.OnPlayerReceiveItem(player, itemId, slotId, number);

            PlayerInfo playerInfo = Main.PlayerInfos.Where(p => p.ConnectionId == player.conn.connectionId).FirstOrDefault();
            if (playerInfo != null)
            {
                playerInfo.LastItemReceivedId = itemId;
                playerInfo.LastItemReceivedCount = number;
            }

        }
    }
}