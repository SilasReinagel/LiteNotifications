
using System.Threading.Tasks;
using DSharpPlus;
using LiteNotifications.WebApi;
using LiteNotifications.WebApi.Domain;
using NUnit.Framework;

public class DiscordChannelIntegrationTests
{
    [Test, Ignore("One-Time Integration Test")]
    public async Task DiscordTexthannel_SendNotification()
    {
        var destination = new DiscordTextChannelDestination(new DiscordClient(new DiscordConfiguration
        {
            Token = new EnvironmentVariable("NotifyAppDiscordBotToken"),
            TokenType = TokenType.Bot
        }));

        await destination.Send(new RoutedNotification
        {
            Target = "778511955227836446",
            Text = "Hello, World"
        });
    }
}
