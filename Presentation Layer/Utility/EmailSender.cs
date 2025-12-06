using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("dodoxdmekheimer@gmail.com", "iytt ugkf erjo xjdu")
            };

            return client.SendMailAsync(
                new MailMessage(
                    from: "dodoxdmekheimer@gmail.com",
                    to: email,
                    subject,
                    htmlMessage
                    )
                {
                    IsBodyHtml = true
                }
                );
        }
    }
}
