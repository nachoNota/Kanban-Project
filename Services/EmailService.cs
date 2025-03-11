using System.Net;
using System.Net.Mail;

namespace tl2_proyecto_2024_nachoNota.Services
{
    public interface IEmailService
    {
        Task SendEmail(string emailReceptor, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(string emailReceptor, string subject, string body)
        {
            var emailEmisor = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:EMAIL");
            var password = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:PASSWORD");
            var port = configuration.GetValue<int>("CONFIGURACIONES_EMAIL:PORT");
            var host = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:HOST");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailEmisor, password);

            var mensaje = new MailMessage(emailEmisor!, emailReceptor, subject, body);
        
            await smtpClient.SendMailAsync(mensaje);
        }
    }
}
