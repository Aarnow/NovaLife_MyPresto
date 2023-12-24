namespace MyPresto
{
    public class PlayerInfo
    {
        public int ConnectionId { get; set; }
        public uint PlayerId { get; set; }
        public int LastItemReceivedId { get; set; }
        public int LastItemReceivedCount { get; set; }

        public PlayerInfo(uint playerId, int connectionId)
        {
            ConnectionId = connectionId;
            PlayerId = playerId;
            LastItemReceivedId = 0;
            LastItemReceivedCount = 0;
        }
    }
}
