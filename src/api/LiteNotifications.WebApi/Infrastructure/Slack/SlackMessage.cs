namespace LiteNotifications.WebApi.Infrastructure.Slack
{
    public class SlackMessage
    {
        public string Text { get; set; }
        public string Channel { get; internal set; }
        public string Title { get; internal set; }
    }
}
