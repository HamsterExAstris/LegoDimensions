using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.OptionsModel;
using SendGrid;

namespace ShatteredTemple.LegoDimensions.Tracker.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private SendGridOptions sendGridOptions_;

        public AuthMessageSender(IOptions<SendGridOptions> sendGridOptions)
        {
            this.sendGridOptions_ = sendGridOptions.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            var myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new MailAddress("atimson+legodimensionstracker@gmail.com", "LEGO® Dimensions Tracker");
            myMessage.Subject = subject;
            myMessage.Html = message;
            // Create a Web transport for sending email.
            var transportWeb = new Web(this.sendGridOptions_.SendGridApiKey);
            // Send the email.
            if (transportWeb != null)
            {
                return transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                return Task.FromResult(0);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
