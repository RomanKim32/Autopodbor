using Autopodbor_312.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autopodbor_312.Interfaces
{
	public interface IServiceRepository
    {
        IEnumerable<Services> Services();

        IEnumerable<Services> ForAdminServices();

        void CreateServices(Services service, IFormFile servicePhotoFile);

        [HttpGet]
        Services EditServices(int? id);

        void EditServices(IFormFile servicePhotoFile, int id, [Bind("Id,NameRu,DescriptionRu,NameKy,DescriptionKy,IsAdditional,Photo")] Services service);

        Services DeleteServices(int? id);

        void DeleteConfirmedServices(int id);

        IEnumerable<Services> AdditionalServicesDetails();
    }
}
