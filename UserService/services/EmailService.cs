
using MailKit.Net.Smtp;
using MimeKit;
using Models;
using UserService.DTOs.Configuration;
namespace UserService.services
{
    public class EmailService( EmailConfiguration _emailConfig) :IEmailService
    {
      

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromMail = "ayabadrin667@gmail.com";
            var fromPassword = "472273483@g10";

            var message = new MailMessage
            {
                From = new MailAddress(fromMail),
                Subject = subject
            };
            message.To.Add(email);
            message.Body = $"<html><body>{htmlMessage}</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
          

            public async Task SendEmail(Messager message)
            {
                var emailMessage =  CreateEmailMessage(message);
                Send(emailMessage);
            }

            private MimeMessage CreateEmailMessage(Messager message)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));

                // Convert string email addresses to MailboxAddress
                foreach (var recipient in message.To)
                {
                    emailMessage.To.Add(new MailboxAddress(recipient.ToString(),"fdskl"));
                }

                emailMessage.Subject = message.Subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

                return emailMessage;
            }

            private void Send(MimeMessage mailMessage)
            {
                using var client = new MailKit.Net.Smtp.SmtpClient();
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    // Log an error message or throw an exception or both
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
    
   
    

