namespace LiteNotifications.WebApi.Contracts
{
    public sealed class AddOutletRequest
    {
        public int GroupId { get; set; }
        public string OutletType { get; set; }
        public string Target { get; set; }

        public string Hash => $"{GroupId}|{OutletType}|{Target}";
    }
}
