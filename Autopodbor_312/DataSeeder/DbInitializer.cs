﻿using System.Collections.Generic;
using System.Linq;
using Autopodbor_312.Models;

namespace Autopodbor_312.DataSeeder
{
    public class DbInitializer
	{
        public static void SeedDatabase(AutopodborContext context)
        {
            if (!context.Services.Any())
            {
                var services = new List<Services>
                {
                    new Services
                    {
                        Name = "Подбор под ключ", Description = "Описание услуги подбора под ключ",
                        IsAdditional = false, Photo = "/serviceImg/tree-736885__480.jpg",
                        KeyForDescriptionInResourcesFiles = "serviceDescription1", KeyForNameInResourcesFiles = "serviceName1"
                    },
                    new Services {
                        Name = "Выездной осмотр", Description = "Описание услуги выезной осмотр",
                        IsAdditional = false, Photo = "/serviceImg/tree-736885__480.jpg",
                        KeyForDescriptionInResourcesFiles = "serviceDescription2", KeyForNameInResourcesFiles = "serviceName2" 
                    },
                    new Services {
                        Name = "Дополнительные услуги", Description = "Описание дополнительных услуг",
                        IsAdditional = false, Photo = "/serviceImg/Add.jpg",
                        KeyForDescriptionInResourcesFiles = "serviceDescription3", KeyForNameInResourcesFiles = "serviceName3"
                    },
                    new Services { Name = "Обратный звонок", Description = "" , IsAdditional = false},
                    new Services
                    { 
                        Name = "Эксперт на день", Description = "Вы нанимаете эксперта на определённое время и осматриваете авто, выбранные вами. Все перемещения производятся на машине эксперта. Услуга работает как по городу, так и на авторынке. Цена: от 6000 сом в день",
                        IsAdditional = true, Photo = "/serviceImg/Add.jpg",
                        KeyForDescriptionInResourcesFiles = "serviceDescription5", KeyForNameInResourcesFiles = "serviceName5"
					},
                    new Services 
                    {
                        Name = "Обслуживание авто", Description = "Хотите обслужить/отремонтировать свою машину, но боитесь, что попадёте на недобросовестный сервис? Для этого, обращаетесь к нам, объясняете задачу и отдаёте авто. Мы производим весь ремонт/обслуживание только в тех сервисах, в которых уверены. Все чеки работ, старые запчасти, фото/видео отчёты прилагаются. Цена: от 1000 сом и выше. (Зависит от масштаба работ)",
                        IsAdditional = true, Photo = "/serviceImg/Add.jpg",
                        KeyForDescriptionInResourcesFiles = "serviceDescription6", KeyForNameInResourcesFiles = "serviceName6"
					},
                    new Services {
                        Name = "Оценка стоимости авто перед продажей", Description = "Если вы продаёте авто, но не знаете средних цен на вашу модель/марку и боитесь ошибиться в цене, то данная услуга для вас. Мы приезжаем и проверяем автомобиль, указываем на все недочёты и объясняем, за какую цену можно продать ваше авто в таком состоянии", 
                        IsAdditional = true, Photo = "/serviceImg/Add.jpg",
                        KeyForDescriptionInResourcesFiles = "serviceDescription7", KeyForNameInResourcesFiles = "serviceName7"
					},
                    new Services { 
                        Name = "Сопровождение сделки", Description = "Вы решили купить авто, но боитесь, что неправильно посчитаете деньги или вас обманут при оформлении документов, то можете воспользоваться этой услугой. Мы сопровождаем любую сделку (переоформление, оформление доверенности, подписание договора с компанией по привозу авто и т.д.) Цена: от 1000 сом и выше за сделку",
                        IsAdditional = true, Photo = "/serviceImg/Add.jpg",
                        KeyForDescriptionInResourcesFiles = "serviceDescription8", KeyForNameInResourcesFiles = "serviceName8"
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
