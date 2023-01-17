using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Http;

namespace Autopodbor_312.Interfaces
{
    public interface IHomeRepository
    {
        MainPageViewModel GetMainPageViewModel();
        void CreateTool(MainPage MainPage, IFormFile file);
        void EditBanner(MainPage FirstBanner, MainPage SecondBanner, MainPage item, IFormFile newPhoto);
        MainPage Delete(int? id);
        void DeleteConfirmed(int? id);
    }
}
