using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Autopodbor_312.OrderMailing
{
    public class EmailService
    {
		public EmailService(string email, string password)
		{
			Email = email;
			Password = password;
		}

		public string Email { get; set; }
        public string Password { get; set; }

		public async Task SendEmailAsync(string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", Email)); // Вставить почту от кого будет рассылка
            emailMessage.To.Add(new MailboxAddress("", Email)); // Вставить почту кому будет отправляться рассылка
            emailMessage.Subject = "Новый заказ";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (SmtpClient client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(Email, Password); // Вставить свой логин и пароль
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
