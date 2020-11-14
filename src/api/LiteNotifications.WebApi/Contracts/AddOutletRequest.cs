namespace LiteNotifications.WebApi.Contracts
{
    public sealed class AddOutletRequest
    {
        public string UserId { get; set; }
        public string OutletType { get; set; }
        public string Target { get; set; }
        public string OutletGroup { get; set; }
    }
}
