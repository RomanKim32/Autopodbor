using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Autopodbor_312.Repositories
{
	
    public class ServiceRepository : IServiceRepository
    {
        private readonly  AutopodborContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public ServiceRepository(AutopodborContext autopodborContext, IWebHostEnvironment webHost, IServiceScopeFactory serviceScopeFactory)
        {
            _context = autopodborContext;
            _appEnvironment = webHost;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<Services> GetAllServices()
        {
            List<Services> services =  _context.Services.Where(s => s.NameRu != "Обратный звонок").Where(s => s.IsAdditional == false).OrderBy(s => s.Id).ToList();
            return services;
        }

        public IEnumerable<Services> ForAdminServices()
        {
            var services = _context.Services.Where(s => s.NameRu != "Дополнительные услуги").OrderBy(s => s.Id).ToList();
            return services;
        }

        public virtual void CreateServices(Services service, IFormFile servicePhotoFile)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                dbContext.Services.Add(service);
                dbContext.SaveChanges();

                service.IsAdditional = true;
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/Id={service.Id}&{servicePhotoFile.FileName}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    servicePhotoFile.CopyTo(fileStream);
                }
                service.Photo = $"/serviceImg/Id={service.Id}&{servicePhotoFile.FileName}";
                dbContext.SaveChanges();
            }
        }

        public Services GetService(int? id)
        {
            var services = _context.Services.FirstOrDefault(m => m.Id == id);
            return services;
        }

        public void EditServices(IFormFile servicePhotoFile, int id, [Bind(new[] { "Id,NameRu,DescriptionRu,NameKy,DescriptionKy,IsAdditional,Photo" })] Services service)
        {
            if (servicePhotoFile != null)
            {
                string oldFilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{service.Photo}");
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/serviceImg/Id={service.Id}&{servicePhotoFile.FileName}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    servicePhotoFile.CopyTo(fileStream);
                }
                service.Photo = $"/serviceImg/Id={service.Id}&{servicePhotoFile.FileName}";
            }
            _context.Update(service);
            _context.SaveChanges();
        }

        public Services DeleteServices(int? id)
        {
            var services = _context.Services.FirstOrDefault(m => m.Id == id);
            return services;
        }

        public void DeleteConfirmedServices(int id)
        {
            var services = _context.Services.Find(id);
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{services.Photo}");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            _context.Services.Remove(services);
            _context.SaveChanges();
        }

        public IEnumerable<Services> AdditionalServicesDetails()
        {
            var additionalServicesList = _context.Services.Where(s => s.IsAdditional == true).OrderBy(s => s.Id).ToList();
            return additionalServicesList;
        }
    }
}
