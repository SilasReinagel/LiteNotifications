//using System.Threading.Tasks;
//using Notifications.Sms.Twilio;
//using Notifications.WebAPI.Domain;
//
//namespace Notifications.WebAPI.Sms
//{
//    public sealed class TwilioSmsChannel : IChannel
//    {
//        private readonly SmsClient _smsClient;
//        public string Type => "SMS";
//
//        public TwilioSmsChannel(SmsClient smsClient) => _smsClient = smsClient;
//        
//        public async Task Send(RoutedNotification n)
//        {
//            await _smsClient.SendMessageAsync(n.Target, $"{n.Title} - {n.Text}");
//        }
//    }
//}
