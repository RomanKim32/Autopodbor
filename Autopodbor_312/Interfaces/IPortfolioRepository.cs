using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Autopodbor_312.Interfaces
{
    public interface IPortfolioRepository
    {
        FilterPortfolioViewModel GetTurnkeySelectionPortfolio(int? bodyType, int? brand, int? model, int pageIndex);
        FilterPortfolioViewModel GetFieldInspectionPortfolio(int? bodyType, int? brand, int? model, int pageIndex);
        IEnumerable<Portfolio> GetAllPortfolioForAdmin();
        CreatePortfolioViewModel CreatePortfolio();
        void CreatePortfolio(Portfolio portfolio, IFormFile mainPic, IFormFileCollection uploadFiles, string video);
        PortfolioDetailsViewModel DetailsPortfolio(int? id);
        PortfolioDetailsViewModel EditPortfolio(int? id);
        void EditPortfolio(int? id, Portfolio portfolio);
        PortfolioNewsFile EditMainPhoto(int? id, IFormFile newPhoto);
        PortfolioNewsFile EditMinorPhoto(int? id, IFormFile newPhoto);
        PortfolioNewsFile DeletePhotoOrVideo(int? id);
        void AddMinorPhoto(int? id, IFormFile newPhoto);
        PortfolioNewsFile EditVideo(int? id, string newVideoId);
        void AddVideo(int? id, string videoId);
        Portfolio DeletePortfolio(int? id);
        void DeleteConfirmedPortfolio(int? id);
        void PublicPortfolio(int? id);
        IEnumerable<CarsBrands> GetAllCarsBrands();
        IEnumerable<CarsBrandsModel> GetAllCarsBrandsModel();
        IEnumerable<CarsBodyTypes> GetAllCarsBodyTypes();
    }
}
