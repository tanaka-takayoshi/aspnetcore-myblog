using System.Threading.Tasks;

namespace My_Blog.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
