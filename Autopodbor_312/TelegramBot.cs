using Autopodbor_312.Models;
using System.IO;
using System.Net;
using System.Text;
using Telegram.Bot;

namespace Autopodbor_312
{
    public class TelegramBot
    {
        private readonly TelegramBotClient _bot;
        private readonly string BotToken = "5946339457:AAEKvth7TxgWKo4CQMGkFkA9J7cLmea8TBk";
        private readonly string ChatId = "-1001622338471"; // указать айди чата в телеграме

        public TelegramBot()
        {
            _bot = new TelegramBotClient(BotToken);
        }

        public void SendInfo(Orders order)
        {
            StringBuilder info = new StringBuilder(
               $"Название заказа: {order.Services.Name}\n" +
               $"Номер телефона: {order.PhoneNumber}\n");
            if (order.UserName != null)
                info.Append($"Имя пользователя - {order.UserName}\n");
            if (order.Email != null)
                info.Append($"Почта: {order.Email}\n");
            if (order.CarsBrands != null)
                info.Append($"Марка: {order.CarsBrands.Brand}\n");
            if (order.CarsBodyTypes != null)
                info.Append($"Тип кузова: {order.CarsBodyTypes.BodyType}\n");
            if (order.CarsYears != null)
                info.Append($"Год выпуска: {order.CarsYears.ManufacturesYear} \n");
            if (order.CarsFuels != null)
                info.Append($"Вид топлива: {order.CarsFuels.FuelsType}\n");
            if (order.Comment != null)
                info.Append($"Дополнительная информация: {order.Comment}");

            string telegramApiUrl = $"https://api.telegram.org/bot{BotToken}/sendMessage?chat_id={ChatId}&text={info}";

            WebRequest request = WebRequest.Create(telegramApiUrl);
            Stream rs = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(rs);
            string line = "";
            StringBuilder sb = new StringBuilder();
            while (line != null)
            {
                line = reader.ReadLine();
                if (line != null)
                    sb.Append(line);
            }
            string response = sb.ToString();
             
        }
    }
}
