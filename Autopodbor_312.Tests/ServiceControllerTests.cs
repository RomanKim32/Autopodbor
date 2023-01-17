using Autopodbor_312.Controllers;
using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Autopodbor_312.Tests
{
	public class ServiceControllerTests
	{
        [Fact]
        public void DeleteServicesTest()
        {
            // Arrange
            var mock = new Mock<IServiceRepository>();
            const int id = 2;
            var controller = new ServiceController(mock.Object);
            mock.Setup(repo => repo.DeleteServices(id));

            // Act
            var result = controller.DeleteServices(id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAdditionalServicesForDetailsPage()
        {
            // Arrange
            var mock = new Mock<IServiceRepository>();
            mock.Setup(repo => repo.AdditionalServicesDetails());
            var controller = new ServiceController(mock.Object);
            var expectedList = GetTestServices().Where(s => s.IsAdditional == true).OrderBy(s => s.Id);

            // Act
            var result = controller.AdditionalServicesDetails();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, expectedList.Count());
            Assert.Equal(5, expectedList.ToArray()[0].Id);
            Assert.Equal(6, expectedList.ToArray()[1].Id);
            Assert.True(expectedList.All(n => n.IsAdditional == true));
        }

        [Fact]
        public void EditMethodGetServiceTest()
        {
            // Arrange
            var mock = new Mock<IServiceRepository>();
            const int id = 2;
            mock.Setup(repo => repo.GetService(id));
            var controller = new ServiceController(mock.Object);
            var service = GetTestServices().Where(s=> s.Id == id);

            // Act
            var result = controller.EditServices(id);

            // Assert
            Assert.NotNull(result);
            Assert.Single(service);
            Assert.Equal(id, service.ToArray()[0].Id);
        }

        [Fact]
        public void GetListOfAllServicesForAdmin()
        {
            // Arrange
            var mock = new Mock<IServiceRepository>();
            mock.Setup(repo => repo.ForAdminServices()).Returns(GetTestServices());
            var controller = new ServiceController(mock.Object);
            var expectedList = GetTestServices().Where(s => s.NameRu != "Обратный звонок").OrderBy(s => s.Id);
            const int allServices = 5;

            // Act
            var result = controller.ForAdminServices();

            // Assert
            Assert.Equal(allServices, expectedList.Count());
            Assert.Equal(1, expectedList.ToArray()[0].Id);
            Assert.Equal(2, expectedList.ToArray()[1].Id);
            Assert.Equal(3, expectedList.ToArray()[2].Id);
            Assert.NotNull(result);
        }

        [Fact]
        public void IndexReturnsAViewResultWithAListOfMainServices()
        {
            // Arrange
            var mock = new Mock<IServiceRepository>();
            mock.Setup(repo => repo.GetMainServices()).Returns(GetTestServices());
            var controller = new ServiceController(mock.Object);
            const int mainServices = 3;
            var expectedList = GetTestServices().Where(s => s.IsAdditional == false && s.NameRu != "Обратный звонок").OrderBy(s => s.Id);

            // Act
            var result = controller.Services();

            // Assert
            Assert.Equal(mainServices, expectedList.Count());
            Assert.Equal(1, expectedList.ToArray()[0].Id);
            Assert.Equal(2, expectedList.ToArray()[1].Id);
            Assert.Equal(3, expectedList.ToArray()[2].Id);
            Assert.NotNull(result);
            Assert.True(expectedList.All(n => n.IsAdditional == false));
        }

        private List<Services> GetTestServices()
        {
            var services = new List<Services>
               {
                    new Services{
                        Id = 1,
                        NameRu = "Подбор под ключ", DescriptionRu = "Пользуюясь подбором под ключ, вы заполняете анкету, указав в ней нужные данные по авто: марку, модель, бюджет и т.д. Мы, по вашим данным, ведём поиск автомобиля самостоятельно, выезжаем на все объявления и проверяем авто, до тех пор, пока не найдём стоящий вариант. Как нашли живое авто, вы встречаетесь с хозяином отдельно от нас и смотрите машину. Если вам всё нравится, мы проверяем авто на нескольких СТО и после этого торгуемся за машину и вы её покупаете. Оплата: Вначале оплачиваете 3000 сом - это предоплата, которая гарантирует нам, что за время поиска, вы не купите другое авто. Остальная сумма оплачивается после покупки авто.",
                        NameKy = "Ачкыч колго тийгенче тандоо", DescriptionKy = "Ачкыч колго тийгенче тандоону колдонуу менен, сиз анда унаа боюнча керектүү маалыматтарды: бренд, модель, бюджет ж.б. анкетаны толтурасыз.\r\nСиздин маалыматтарыңыз боюнча, биз өз алдынча машина издейбиз, бардык жарнамаларга кирип, татыктуу вариант табылганга чейин унааны текшеребиз. Тирүү машинаны тапкандан кийин бизден өзүнчө ээси менен жолугуп, машинаны көрөсүз. Эгер сизге баары жакса, биз машинаны бир нече техникалык тейлөө станцияларында текшеребиз, андан кийин биз машинаны соодалашабыз, сиз аны сатып аласыз.\r\nТөлөм: Алгач сиз 3000 сом төлөйсүз – бул алдын ала төлөм, бул бизге издөө учурунда сиз башка унаа сатып албайсыз деп кепилдик берет. Калган сумма унаа сатып алынгандан кийин төлөнөт.\r\n",
                        IsAdditional = false, Photo = "/serviceImg/tree-736885__480.jpg",
                    },
                    new Services {
                        Id = 2,
                        NameRu = "Выездной осмотр", DescriptionRu = "Если вам понравился та или иная машина в объявлении, который вы хотите посмотреть, но у вас нет времени, либо нет навыков в авто, то вы обращаетесь к нам. Это можно сделать по звонку, в инстаграме, Вотсапе и телеграмме. Отправляете ссылку на объявление и все, ждете пока мы посмотрим машину. Мы сами звоним продавцу, встречаемся с ним, смотрим машину и отправляем вам все фото, видео, а также отчет о диагностике в том же чате. А вы после этого отправляете деньги на баланс телефона, либо на банковский счёт. Но если же вы хотите сами поприсутствовать на осмотре, то можете приехать и наблюдать за всем вживую. ",
                        NameKy = "Көчмө текшерүү", DescriptionKy = "Эгер сизге жарнамада тигил же бул унаа жакса, аны көргүңүз келсе,  бирок убактыңыз жок же унааны текшерүү жөндөмүңүз жок болсо, анда сиз бизге кайрыласыз. Муну чалуу, инстаграм, вотсап жана телеграмма аркылуу жасаса болот. Жарнама шилтемесин жөнөтөсүз жана биз машинаны көргөнгө чейин күтөсүз. Биз сатуучуга өзүбүз чалып, аны менен жолугуп, унааны көрүп, ошол эле чатта бардык сүрөттөрдү, видеолорду, ошондой эле диагностикалык отчетту жөнөтөбүз. Андан кийин сиз телефондун балансына же банк эсебине акча жөнөтөсүз. \r\nБирок, эгер сиз текшерүүгө өзүңүз катышкыңыз келсе, анда келип, бардыгын биз менен көрө аласыз.\r\n",
                        IsAdditional = false, Photo = "/serviceImg/tree-736885__480.jpg",
                    },
                    new Services {
                        Id = 3,
                        NameRu = "Дополнительные услуги", DescriptionRu = "Описание дополнительных услуг",
                        NameKy = "Кошумча кызматтар", DescriptionKy = "Описание на кырг",
                        IsAdditional = false, Photo = "/serviceImg/Add.jpg",
                    },
                    new Services { Id = 4, NameRu = "Обратный звонок", DescriptionRu = "" ,NameKy = "", DescriptionKy = "", IsAdditional = false},
                    new Services{
                        Id = 5,
                        NameRu = "Эксперт на день", DescriptionRu = "Вы нанимаете эксперта на определённое время и осматриваете авто, выбранные вами. Все перемещения производятся на машине эксперта. Услуга работает как по городу, так и на авторынке. Цена: от 6000 сом в день",
                        NameKy = "Бир күнгө эксперт", DescriptionKy = "Сиз белгилүү бир убакытка эксперти жалдап, тандап алган унааларды текшересиз. Бардык алып баруулар эксперттин машинасында жүргүзүлөт. Кызмат шаарда да, унаа базарында да иштейт. Баасы: 6000 сомдон жогору бир күнүнө",
                        IsAdditional = true, Photo = "/serviceImg/Add.jpg",
                    },
                    new Services {
                        Id = 6,
                        NameRu = "Обслуживание авто", DescriptionRu = "Хотите обслужить/отремонтировать свою машину, но боитесь, что попадёте на недобросовестный сервис? Для этого, обращаетесь к нам, объясняете задачу и отдаёте авто. Мы производим весь ремонт/обслуживание только в тех сервисах, в которых уверены. Все чеки работ, старые запчасти, фото/видео отчёты прилагаются. Цена: от 1000 сом и выше. (Зависит от масштаба работ)",
                        NameKy = "Автону тейлөө", DescriptionKy = "Унааңызды тейлегиңиз же оңдогуңуз келеби, бирок начар тейлөөдөн коркосузбу? Биз менен байланышып, тапшырманы түшүндүрүп, чечимди чогуу кабыл алабыз. Бардык оңдоолорду / техникалык тейлөөлөрдү биз ишенген жерлерде гана жасайбыз. бардык бөлүктөрдүн/иштин баасын текшерүү тиркелет. Тетиктерге/иштерге баалар менен бардык квитанциялар тиркелет. Баасы: 1000 сомдон жогору",
                        IsAdditional = true, Photo = "/serviceImg/Id=6&Add.jpg",
                    },
               };
            return services;
        }
    }
}
