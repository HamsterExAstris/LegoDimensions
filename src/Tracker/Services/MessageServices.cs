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

        public Task SendEmailAsync(string email, string subject, string message, bool isHtml, string plainTextMessage = null, params string[] categories)
        {
            // Plug in your email service here to send an email.
            var myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new MailAddress("atimson+legodimensionstracker@gmail.com", "LEGO® Dimensions Tracker");
            myMessage.Subject = subject;
            if (isHtml)
            {
                myMessage.Html = message;
                if (plainTextMessage != null)
                {
                    myMessage.Text = plainTextMessage;
                }
            }
            else
            {
                myMessage.Text = message;
            }

            // Disable batch-email tracking items that we do not want for our
            // person-specific emails.
            myMessage.DisableClickTracking();
            myMessage.DisableGoogleAnalytics();
            myMessage.DisableOpenTracking();
            myMessage.DisableUnsubscribe();

            // Skip the spam check that's not currently needed as we are not
            // sending any user-generated messages. This should be removed
            // if we ever support custom content (unlikely).
            myMessage.DisableSpamCheck();

            myMessage.SetCategories(categories);
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
