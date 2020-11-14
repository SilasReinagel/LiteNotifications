namespace LiteNotifications.WebApi.Contracts
{
    public sealed class SubscriptionRequest
    {
        public string UserId { get; set; }
        public string Topic { get; set; }
        public string OutletGroup { get; set; } = "Default";
    }
}
