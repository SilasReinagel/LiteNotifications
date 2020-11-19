using System.Threading.Tasks;
using DSharpPlus;
using LiteNotifications.WebApi.Domain;

public class DiscordTextChannelDestination : Destination
{
    public string Type => "DiscordTextChannel";
    
    private readonly DiscordClient _client;

    public DiscordTextChannelDestination(string botToken) 
        : this(new DiscordClient(new DiscordConfiguration{Token = botToken, TokenType = TokenType.Bot})) {}
    
    public DiscordTextChannelDestination(DiscordClient client) => _client = client;

    public async Task Send(RoutedNotification n)
    {
        var channel = await _client.GetChannelAsync(ulong.Parse(n.Target));
        await channel.SendMessageAsync(n.Text);
    }
}
