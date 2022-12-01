using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Autopodbor_312
{
	public class EmailService
	{
		public async Task SendEmailAsync(string message)
		{
			var emailMessage = new MimeMessage();
			emailMessage.From.Add(new MailboxAddress("Администрация сайта", "romakim32@gmail.com")); // Вставить почту от кого будет рассылка
			emailMessage.To.Add(new MailboxAddress("", "romakim32@gmail.com")); // Вставить почту кому будет отправляться рассылка
			emailMessage.Subject = "Новый заказ";
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = message
			};

			using (SmtpClient client = new SmtpClient())
			{
				await client.ConnectAsync("smtp.gmail.com", 587, false);
				await client.AuthenticateAsync("romakim32@gmail.com", "mbtrobcdyowtapom"); // Вставить свой логин и пароль
				await client.SendAsync(emailMessage);
				await client.DisconnectAsync(true);
			}
		}
	}
}
