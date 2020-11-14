namespace LiteNotifications.WebApi.Email
{
    public struct EmailSender
    {
        public string Address { get; }

        public EmailSender(string address) => Address = address;
    }
}
