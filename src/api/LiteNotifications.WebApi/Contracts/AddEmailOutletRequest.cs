namespace LiteNotifications.WebApi.Contracts
{
    public sealed class AddEmailOutletRequest
    {
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string OutletGroup { get; set; } = "Default";

        public AddOutletRequest AsOutletRequest() => new AddOutletRequest
        {
            UserId = UserId,
            OutletType = "Email",
            Target = EmailAddress,
            OutletGroup = OutletGroup
        };
    }
}
