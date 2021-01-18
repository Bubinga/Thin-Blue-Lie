using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ThinBlueLie.Models;

namespace ThinBlueLie.Helper.Services
{
    public interface IEmailSender
    {
        Task SendConfirmationEmailAsync(string email, string subject, string callbackurl);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true                    
                };

                mail.To.Add(new MailAddress(email));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = _emailSettings.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = _emailSettings.MailServer,
                    EnableSsl = true,
                    Credentials = credentials
                };

                // Send it...         
                client.Send(mail);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }

            return Task.CompletedTask;
        }

        public async Task SendConfirmationEmailAsync(string email, string subject, string callbackurl)
        {      
            string message = @"<head><style> .im {color: #ff00 !important;} </style></head>" +
                               @"<div><div style=""text-align: center; margin-top: 50px;"">" +
                                    @"<img alt=""Thin Blue Lie Logo"" src=""https://thinbluelie.us/Assets/ThinBlueLie-Logo.png"" " +
                                    @"width=""100px"" height=""100px""> " +
                               @"</div>" +
                                @"<div style=""padding: 0 15px;""> " +
                                    @"<h3 style=""text-align:center;""> " +
                                      @"Thanks for registering for a Thin Blue Lie account!" +
                                    @"</h3>" +
                                        @"<div style=""text-align:center; max-width: 950px; margin-right: auto; margin-left: auto""> " +
                                            @"<p> " +
                                               @"To confirm your account, please click the link below. " +
                                            @"</p>" +
                                            @$"<a href=""{HtmlEncoder.Default.Encode(callbackurl)}"">Verify</a> " +
                                        @"</div> " +
                                @"</div></div>";
            await SendEmailAsync(email, subject, message);
        }
    }
}
