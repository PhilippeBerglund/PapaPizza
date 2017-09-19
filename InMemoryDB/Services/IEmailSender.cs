using System.Threading.Tasks;

namespace PapaPizza.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
