using System.Collections.Generic;
using System.Linq;
using Autopodbor_312.Models;

namespace Autopodbor_312.DataSeeder
{
    public class DbInitializer
	{
        public static void SeedDatabase(AutopodborContext context)
        {

            if (!context.ContactInformation.Any())
            {
                var contactInformation = new ContactInformation()
                {
                    Email = "mgaldobin@mail.ru", 
                    LinkToInstagram = "https://instagram.com/autopodbor.312?igshid=YmMyMTA2M2Y=", 
                    LinkToTelegram = "", 
                    LinkToTiktok = "tiktok.com/@autopodbor.312", 
                    LinkToWhatsapp = "", 
                    LinkToYoutube = "",
                    PhoneNumber = "0558303707"
                };
                context.AddRange(contactInformation);
                context.SaveChanges();
            }

            if (!context.Services.Any())
            {
                var services = new List<Services>
                {
                    new Services{
                        Id = 1,
                        NameRu = "Подбор под ключ", DescriptionRu = "Пользуюясь подбором под ключ, вы заполняете анкету, указав в ней нужные данные по авто: марку, модель, бюджет и т.д. Мы, по вашим данным, ведём поиск автомобиля самостоятельно, выезжаем на все объявления и проверяем авто, до тех пор, пока не найдём стоящий вариант. Как нашли живое авто, вы встречаетесь с хозяином отдельно от нас и смотрите машину. Если вам всё нравится, мы проверяем авто на нескольких СТО и после этого торгуемся за машину и вы её покупаете. Оплата: Вначале оплачиваете 3000 сом - это предоплата, которая гарантирует нам, что за время поиска, вы не купите другое авто. Остальная сумма оплачивается после покупки авто.",
                        NameKy = "Название на кырг", DescriptionKy = "Описание на кырг",
                        IsAdditional = false, Photo = "/serviceImg/Id=1&tree-736885__480.jpg",
                    },
                    new Services {
                        Id = 2,
                        NameRu = "Выездной осмотр", DescriptionRu = "Если вам понравился та или иная машина в объявлении, который вы хотите посмотреть, но у вас нет времени, либо нет навыков в авто, то вы обращаетесь к нам. Это можно сделать по звонку, в инстаграме, Вотсапе и телеграмме. Отправляете ссылку на объявление и все, ждете пока мы посмотрим машину. Мы сами звоним продавцу, встречаемся с ним, смотрим машину и отправляем вам все фото, видео, а также отчет о диагностике в том же чате. А вы после этого отправляете деньги на баланс телефона, либо на банковский счёт. Но если же вы хотите сами поприсутствовать на осмотре, то можете приехать и наблюдать за всем вживую. ",
						NameKy = "Название на кырг", DescriptionKy = "Описание на кырг",
						IsAdditional = false, Photo = "/serviceImg/Id=2&tree-736885__480.jpg",
                    },
                    new Services {
                        Id = 3,
                        NameRu = "Дополнительные услуги", DescriptionRu = "Описание дополнительных услуг",
						NameKy = "Название на кырг", DescriptionKy = "Описание на кырг",
						IsAdditional = false, Photo = "/serviceImg/Id=3&Add.jpg",
                    },
                    new Services {Id = 4, NameRu = "Обратный звонок", DescriptionRu = "" ,NameKy = "", DescriptionKy = "", IsAdditional = false},
                    new Services{ 
                        Id = 5,
                        NameRu = "Эксперт на день", DescriptionRu = "Вы нанимаете эксперта на определённое время и осматриваете авто, выбранные вами. Все перемещения производятся на машине эксперта. Услуга работает как по городу, так и на авторынке. Цена: от 6000 сом в день",
						NameKy = "Название на кырг", DescriptionKy = "Описание на кырг",
						IsAdditional = true, Photo = "/serviceImg/Id=5&Add.jpg",
					},
                    new Services {
                        Id = 6,
                        NameRu = "Обслуживание авто", DescriptionRu = "Хотите обслужить/отремонтировать свою машину, но боитесь, что попадёте на недобросовестный сервис? Для этого, обращаетесь к нам, объясняете задачу и отдаёте авто. Мы производим весь ремонт/обслуживание только в тех сервисах, в которых уверены. Все чеки работ, старые запчасти, фото/видео отчёты прилагаются. Цена: от 1000 сом и выше. (Зависит от масштаба работ)",
						NameKy = "Название на кырг", DescriptionKy = "Описание на кырг",
						IsAdditional = true, Photo = "/serviceImg/Id=6&Add.jpg",
					},
                    new Services {
                        Id = 7,
                        NameRu = "Оценка стоимости авто перед продажей", DescriptionRu = "Если вы продаёте авто, но не знаете средних цен на вашу модель/марку и боитесь ошибиться в цене, то данная услуга для вас. Мы приезжаем и проверяем автомобиль, указываем на все недочёты и объясняем, за какую цену можно продать ваше авто в таком состоянии",
						NameKy = "Название на кырг", DescriptionKy = "Описание на кырг",
						IsAdditional = true, Photo = "/serviceImg/Id=7&Add.jpg",
					},
                    new Services { 
                        Id = 8,
                        NameRu = "Сопровождение сделки", DescriptionRu = "Вы решили купить авто, но боитесь, что неправильно посчитаете деньги или вас обманут при оформлении документов, то можете воспользоваться этой услугой. Мы сопровождаем любую сделку (переоформление, оформление доверенности, подписание договора с компанией по привозу авто и т.д.) Цена: от 1000 сом и выше за сделку",
						NameKy = "Название на кырг", DescriptionKy = "Описание на кырг",
						IsAdditional = true, Photo = "/serviceImg/Id=8&Add.jpg",
					}
                };
                context.AddRange(services);
                context.SaveChanges();
            }
            if (!context.CarsBrands.Any())
            {
                var brands = new List<CarsBrands>
                {
                    new CarsBrands{ Brand = "Toyota", Price = "300"},
                    new CarsBrands{ Brand = "Mercedes-Benz", Price = "500"},
                    new CarsBrands{ Brand = "Audi", Price = "400"}
                };
                context.AddRange(brands);
                context.SaveChanges();
            }
            if (!context.CarsYears.Any())
            {
                var years = new List<CarsYears>
                {
                    new CarsYears{ ManufacturesYear = "До 2000", Price = "200"},
                    new CarsYears{ ManufacturesYear = "До 2010", Price = "300"},
                    new CarsYears{ ManufacturesYear = "До 2020", Price = "400"},
                    new CarsYears{ ManufacturesYear = "Выше 2020", Price = "500"}
                };
                context.AddRange(years);
                context.SaveChanges();
            }
            if (!context.CarsBodyTypes.Any())
            {
                var bodies = new List<CarsBodyTypes>
                {
                    new CarsBodyTypes{ BodyType = "Седан", Price = "300"},
                    new CarsBodyTypes{ BodyType = "Хэтчбек", Price = "200"},
                    new CarsBodyTypes{ BodyType = "Минивэн", Price = "500"},
                    new CarsBodyTypes{ BodyType = "Внедорожник", Price = "500"},
                    new CarsBodyTypes{ BodyType = "Купе", Price = "300"}
                };
                context.AddRange(bodies);
                context.SaveChanges();
            }
            if (!context.CarsFuels.Any())
            {
                var fuels = new List<CarsFuels>
                {
                    new CarsFuels{ FuelsType = "Гибрид", Price = "300"},
                    new CarsFuels{ FuelsType = "Не гибрид", Price = "100"}
                };
                context.AddRange(fuels);
                context.SaveChanges();
            }
        }
    }
}
