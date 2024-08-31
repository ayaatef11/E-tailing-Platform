using Models;

namespace UserService.services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendEmail(Messager message);
    }
}
