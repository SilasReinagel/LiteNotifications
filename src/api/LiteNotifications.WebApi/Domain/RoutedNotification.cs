namespace LiteNotifications.WebApi.Domain
{
    public sealed class RoutedNotification
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Target { get; set; }
    }
}
