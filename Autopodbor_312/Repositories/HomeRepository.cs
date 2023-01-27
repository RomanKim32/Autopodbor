using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Autopodbor_312.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly AutopodborContext _context;
        private readonly IWebHostEnvironment _appEnvironment;


        public HomeRepository(AutopodborContext autopodborContext, IWebHostEnvironment webHost)
        {
            _context = autopodborContext;
            _appEnvironment = webHost;
        }

        public void CreateTool(MainPage MainPage, IFormFile file)
        {
            MainPage.Banner = "third";
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/mainPageFiles/{file.FileName}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            MainPage.Path = $"/mainPageFiles/{file.FileName}";
            _context.Add(MainPage);
            _context.SaveChanges();
        }

        public MainPage Delete(int? id)
        {
            var mainPage = _context.MainPage.FirstOrDefault(p => p.Id == id);
            return mainPage;
        }

        public void DeleteConfirmed(int? id)
        {
            var mainPage = _context.MainPage.FirstOrDefault(m => m.Id == id);
            _context.Remove(mainPage);
            _context.SaveChanges();
        }

        public void EditBanner(MainPage FirstBanner, MainPage SecondBanner, MainPage item, IFormFile newPhoto)
        {
            if (FirstBanner.Id != 0)
            {
                EditBanner(FirstBanner, newPhoto);
            }
            else if (SecondBanner.Id != 0)
            {
                EditBanner(SecondBanner, newPhoto);
            }
            else
            {
                EditBanner(item, newPhoto);
            }
        }

        public MainPageViewModel GetMainPageViewModel()
        {
            MainPageViewModel mainPageViewModel = new MainPageViewModel
            {
                FirstBanner = _context.MainPage.FirstOrDefault(b => b.Banner == "first"),
                SecondBanner = _context.MainPage.FirstOrDefault(b => b.Banner == "second"),
                ThirdBanner = _context.MainPage.Where(b => b.Banner == "third").ToList()
            };
            return mainPageViewModel;
        }

        private void EditBanner(MainPage banner, IFormFile file)
        {
            if (file != null)
            {
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/mainPageFiles/{file.FileName}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                banner.Path = $"/mainPageFiles/{file.FileName}";
            }
            _context.Update(banner);
            _context.SaveChanges();
        }
    }
}
