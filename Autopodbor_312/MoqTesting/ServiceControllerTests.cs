﻿using Autopodbor_312.Controllers;
using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Autopodbor_312.MoqTesting
{

	public class ServiceControllerTests
	{
        
        [Fact]
        
        public void IndexReturnsAViewResultWithAListOfServices()
        {
            // Arrange
            var mock = new Mock<IServiceRepository>();
            mock.Setup(repo => repo.GetAllServices()).Returns(GetTestServices());
            var controller = new ServiceController(mock.Object);

            // Act
            var result = controller.Services();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Services>>(viewResult.Model);
            Assert.Equal(GetTestServices().Count, model.Count());
        }

        private List<Services> GetTestServices()
        {
            var services = new List<Services>
               {
                    new Services{
                        NameRu = "Подбор под ключ", DescriptionRu = "Пользуюясь подбором под ключ, вы заполняете анкету, указав в ней нужные данные по авто: марку, модель, бюджет и т.д. Мы, по вашим данным, ведём поиск автомобиля самостоятельно, выезжаем на все объявления и проверяем авто, до тех пор, пока не найдём стоящий вариант. Как нашли живое авто, вы встречаетесь с хозяином отдельно от нас и смотрите машину. Если вам всё нравится, мы проверяем авто на нескольких СТО и после этого торгуемся за машину и вы её покупаете. Оплата: Вначале оплачиваете 3000 сом - это предоплата, которая гарантирует нам, что за время поиска, вы не купите другое авто. Остальная сумма оплачивается после покупки авто.",
                        NameKy = "Ачкыч колго тийгенче тандоо", DescriptionKy = "Ачкыч колго тийгенче тандоону колдонуу менен, сиз анда унаа боюнча керектүү маалыматтарды: бренд, модель, бюджет ж.б. анкетаны толтурасыз.\r\nСиздин маалыматтарыңыз боюнча, биз өз алдынча машина издейбиз, бардык жарнамаларга кирип, татыктуу вариант табылганга чейин унааны текшеребиз. Тирүү машинаны тапкандан кийин бизден өзүнчө ээси менен жолугуп, машинаны көрөсүз. Эгер сизге баары жакса, биз машинаны бир нече техникалык тейлөө станцияларында текшеребиз, андан кийин биз машинаны соодалашабыз, сиз аны сатып аласыз.\r\nТөлөм: Алгач сиз 3000 сом төлөйсүз – бул алдын ала төлөм, бул бизге издөө учурунда сиз башка унаа сатып албайсыз деп кепилдик берет. Калган сумма унаа сатып алынгандан кийин төлөнөт.\r\n",
                        IsAdditional = false, Photo = "/serviceImg/tree-736885__480.jpg",
                    },
                    new Services {
                        NameRu = "Выездной осмотр", DescriptionRu = "Если вам понравился та или иная машина в объявлении, который вы хотите посмотреть, но у вас нет времени, либо нет навыков в авто, то вы обращаетесь к нам. Это можно сделать по звонку, в инстаграме, Вотсапе и телеграмме. Отправляете ссылку на объявление и все, ждете пока мы посмотрим машину. Мы сами звоним продавцу, встречаемся с ним, смотрим машину и отправляем вам все фото, видео, а также отчет о диагностике в том же чате. А вы после этого отправляете деньги на баланс телефона, либо на банковский счёт. Но если же вы хотите сами поприсутствовать на осмотре, то можете приехать и наблюдать за всем вживую. ",
                        NameKy = "Көчмө текшерүү", DescriptionKy = "Эгер сизге жарнамада тигил же бул унаа жакса, аны көргүңүз келсе,  бирок убактыңыз жок же унааны текшерүү жөндөмүңүз жок болсо, анда сиз бизге кайрыласыз. Муну чалуу, инстаграм, вотсап жана телеграмма аркылуу жасаса болот. Жарнама шилтемесин жөнөтөсүз жана биз машинаны көргөнгө чейин күтөсүз. Биз сатуучуга өзүбүз чалып, аны менен жолугуп, унааны көрүп, ошол эле чатта бардык сүрөттөрдү, видеолорду, ошондой эле диагностикалык отчетту жөнөтөбүз. Андан кийин сиз телефондун балансына же банк эсебине акча жөнөтөсүз. \r\nБирок, эгер сиз текшерүүгө өзүңүз катышкыңыз келсе, анда келип, бардыгын биз менен көрө аласыз.\r\n",
                        IsAdditional = false, Photo = "/serviceImg/tree-736885__480.jpg",
                    },
                    new Services {
                        NameRu = "Дополнительные услуги", DescriptionRu = "Описание дополнительных услуг",
                        NameKy = "Кошумча кызматтар", DescriptionKy = "Описание на кырг",
                        IsAdditional = false, Photo = "/serviceImg/Add.jpg",
                    },
                    new Services {NameRu = "Обратный звонок", DescriptionRu = "" ,NameKy = "", DescriptionKy = "", IsAdditional = false},
                    new Services{
                        NameRu = "Эксперт на день", DescriptionRu = "Вы нанимаете эксперта на определённое время и осматриваете авто, выбранные вами. Все перемещения производятся на машине эксперта. Услуга работает как по городу, так и на авторынке. Цена: от 6000 сом в день",
                        NameKy = "Бир күнгө эксперт", DescriptionKy = "Сиз белгилүү бир убакытка эксперти жалдап, тандап алган унааларды текшересиз. Бардык алып баруулар эксперттин машинасында жүргүзүлөт. Кызмат шаарда да, унаа базарында да иштейт. Баасы: 6000 сомдон жогору бир күнүнө",
                        IsAdditional = true, Photo = "/serviceImg/Add.jpg",
                    }
               };
            return services;
        }
    }
}
