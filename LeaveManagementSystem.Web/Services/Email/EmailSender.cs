using System.Net;
using System.Net.Mail;

namespace LeaveManagementSystem.Web.Services.Email
{
    public class EmailSender(IConfiguration _configuration) : IEmailSender
    {
        /// <summary>
        /// Service responsible for sending emails using SMTP settings from the application configuration. Builds and sends HTML email messages asynchronously through the configured SMTP server.
        /// </summary>

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromAddress = _configuration["EmailSettings:DefaultEmailAddress"];
            var smtpServer = _configuration["EmailSettings:Server"];
            var smtpPort = Convert.ToInt32(_configuration["EmailSettings:Port"]);
            var smtpUser = _configuration["EmailSettings:UserName"];
            var smtpPass = _configuration["EmailSettings:Password"];
            var enableSsl = Convert.ToBoolean(_configuration["EmailSettings:EnableSsl"]);

            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(email);

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = enableSsl,
                UseDefaultCredentials = false
            };

            await client.SendMailAsync(message);
        }
    }
}