using MimeKit;
using RateACourse.Core.Services.Interfaces;
using RateACourse.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;

using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MimeKit.Encodings;

namespace RateACourse.Core.Services
{
    public class EmailService : IEmailService
    {
        public async Task<BaseResultModel> SendConfirmationMailAsync(string userId, string userEmail, string token,string confirmationLink)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("admin@rateACourse.com"));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "Confirm your emailaddress";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"<h5>Please confirm your emailadress</h5>" +
                       $"<p>Please confirm your emailadress by clicking " +
                       $"<a href='{confirmationLink}'>here</a>",
            };
            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync("gradProgPriTest@gmail.com", "owjxlealkiucmsbu");
            var result = await smtpClient.SendAsync(email);
            await smtpClient.DisconnectAsync(true);
            return new BaseResultModel { IsSuccess = true };
        }
    }
}
