namespace LiteNotifications.WebApi.Contracts
{
    public sealed class AddEmailOutletRequest
    {
        public int GroupId { get; set; }
        public string EmailAddress { get; set; }
        public string OutletGroup { get; set; } = "Default";

        public AddOutletRequest AsOutletRequest() => new AddOutletRequest
        {
            GroupId = GroupId,
            OutletType = "Email",
            Target = EmailAddress,
        };
    }
}
