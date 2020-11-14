namespace LiteNotifications.WebApi.Email
{
    public struct SmtpSettings
    {
        private readonly string _rawAddress;

        public int Port => _rawAddress.Contains(":") ? int.Parse(_rawAddress.Split(":")[1]) : 25;
        public string Address => _rawAddress.Split(":")[0];

        public SmtpSettings(string rawAddress) => _rawAddress = rawAddress;
    }
}
