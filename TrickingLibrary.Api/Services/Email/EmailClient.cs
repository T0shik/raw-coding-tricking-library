using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TrickingLibrary.Api.Services.Email
{
    public class EmailClient
    {
        private readonly IOptionsMonitor<SendGridOptions> _optionsMonitor;
        private readonly SendGridClient _client;

        public EmailClient(IOptionsMonitor<SendGridOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
            _client = new SendGridClient(_optionsMonitor.CurrentValue.ApiKey);
        }

        public Task<Response> SendModeratorInviteAsync(string email, string link)
        {
            var htmlContent = $@"You are invited to be a moderator on Tricking Library

follow the <a href=""{link}"">link</a> to register.";
            var msg = MailHelper.CreateSingleEmail(
                new EmailAddress(_optionsMonitor.CurrentValue.From),
                new EmailAddress(email),
                "Tricking Library Moderator Invite",
                "",
                htmlContent
            );
            return _client.SendEmailAsync(msg);
        }
    }
}