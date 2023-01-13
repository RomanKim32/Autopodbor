using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Autopodbor_312.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AutopodborContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public PortfolioRepository(AutopodborContext autopodborContext, IWebHostEnvironment webHost, IServiceScopeFactory serviceScopeFactory)
        {
            _context = autopodborContext;
            _appEnvironment = webHost;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void AddMinorPhoto(int? id, IFormFile newPhoto)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile { Type = "picture", PortfolioId = id };
                dbContext.PortfolioNewsFiles.Add(portfolioNewsFile);
                dbContext.SaveChanges();
                portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/Id={portfolioNewsFile.Id}&{newPhoto.FileName}";
                dbContext.PortfolioNewsFiles.Update(portfolioNewsFile);
                dbContext.SaveChanges();
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/Id={portfolioNewsFile.Id}&{newPhoto.FileName}");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    newPhoto.CopyTo(fileStream);
                }
            }
            UpdateCreationDate(id);
        }

        public void AddVideo(int? id, string videoId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + videoId, Type = "video", PortfolioId = id };
                dbContext.PortfolioNewsFiles.Add(portfolioNewsFile);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(id);
        }

        public CreatePortfolioViewModel CreatePortfolio()
        {
            Portfolio portfolio = new Portfolio();
            List<CarsBodyTypes> carsBodyTypes = _context.CarsBodyTypes.ToList();
            List<CarsBrands> carsBrands = _context.CarsBrands.ToList();
            List<CarsBrandsModel> carsBrandsModels = _context.CarsBrandsModels.ToList();
            var createPortfolioViewModel = new CreatePortfolioViewModel
            {
                Portfolio = portfolio,
                CarsBodyTypes = carsBodyTypes,
                CarsBrandsModel = carsBrandsModels,
                CarsBrands = carsBrands,
            };
            return createPortfolioViewModel;
        }

        public void CreatePortfolio(Portfolio portfolio, IFormFile mainPic, IFormFileCollection uploadFiles, string video)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                string newsPortfolioFolderPath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/newsPortfolioFiles");
                DirectoryInfo newsPortfolioInfo = new DirectoryInfo(newsPortfolioFolderPath);
                if (!newsPortfolioInfo.Exists)
                {
                    newsPortfolioInfo.Create();
                }
                string newsFolderPath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/newsPortfolioFiles/portfolioFiles");
                DirectoryInfo newsInfo = new DirectoryInfo(newsFolderPath);
                if (!newsInfo.Exists)
                {
                    newsInfo.Create();
                }

                portfolio.CreatedDate = DateTime.Now;
                portfolio.Publicate = false;
                dbContext.Portfolio.Add(portfolio);
                dbContext.SaveChanges();
                portfolio.MainImagePath = $"/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{mainPic.FileName}";
                dbContext.SaveChanges();
                if (mainPic != null)
                {
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{mainPic.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        mainPic.CopyTo(fileStream);
                    }
                    PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{mainPic.FileName}", Type = "mainPic", PortfolioId = portfolio.Id };
                    dbContext.PortfolioNewsFiles.Add(update);
                    dbContext.SaveChanges();
                }

                if (uploadFiles != null)
                {
                    foreach (var upload in uploadFiles)
                    {
                        PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile { Type = "picture", PortfolioId = portfolio.Id };
                        dbContext.PortfolioNewsFiles.Add(portfolioNewsFile);
                        dbContext.SaveChanges();
                        string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/Id={portfolioNewsFile.Id}&{upload.FileName}");
                        portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/Id={portfolioNewsFile.Id}&{upload.FileName}";
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            upload.CopyTo(fileStream);
                        }
                    }
                    if (video != null)
                    {
                        string[] paths = video.Split(' ');
                        foreach (var path in paths)
                        {
                            PortfolioNewsFile vid = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + path, Type = "video", PortfolioId = portfolio.Id };
                            dbContext.Add(vid);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }

        public void DeleteConfirmedPortfolio(int? id)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                Portfolio portfolio = dbContext.Portfolio.FirstOrDefault(p => p.Id == id);
                List<PortfolioNewsFile> portfolioNewsFiles = dbContext.PortfolioNewsFiles.Where(p => p.PortfolioId == id).ToList();
                foreach (var p in portfolioNewsFiles)
                {
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{p.Path}");
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    dbContext.PortfolioNewsFiles.Remove(p);
                }
                dbContext.SaveChanges();
                dbContext.Remove(portfolio);
                dbContext.SaveChanges();
            }
        }

        public PortfolioNewsFile DeletePhotoOrVideo(int? id)
        {
            PortfolioNewsFile portfolioNewsFile = _context.PortfolioNewsFiles.FirstOrDefault(p => p.Id == id);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{portfolioNewsFile.Path}");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                dbContext.PortfolioNewsFiles.Remove(portfolioNewsFile);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(portfolioNewsFile.PortfolioId);
            return portfolioNewsFile;
        }

        public Portfolio DeletePortfolio(int? id)
        {
            return (_context.Portfolio.FirstOrDefault(p => p.Id == id));
        }

        public PortfolioDetailsViewModel DetailsPortfolio(int? id)
        {
            Portfolio portfolio = _context.Portfolio.FirstOrDefault(p => p.Id == id);
            List<PortfolioNewsFile> minorImg = _context.PortfolioNewsFiles.Where(i => i.PortfolioId == id && i.Type == "picture").ToList();
            List<PortfolioNewsFile> videos = _context.PortfolioNewsFiles.Where(v => v.PortfolioId == id && v.Type == "video").ToList();
            PortfolioDetailsViewModel portfolioDetailsViewModel = new PortfolioDetailsViewModel { Portfolio = portfolio, MinorPictures = minorImg, Videos = videos };
            return portfolioDetailsViewModel;
        }

        public PortfolioNewsFile EditMainPhoto(int? id, IFormFile newPhoto)
        {
            PortfolioNewsFile portfolioNewsFile = _context.PortfolioNewsFiles.FirstOrDefault(p => p.Id == id);
            Portfolio portfolio = _context.Portfolio.FirstOrDefault(p => p.Id == portfolioNewsFile.PortfolioId);
            string oldFilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{portfolio.MainImagePath}");
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{newPhoto.FileName}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                newPhoto.CopyTo(fileStream);
            }
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{newPhoto.FileName}";
                portfolio.MainImagePath = $"/newsPortfolioFiles/portfolioFiles/mainPicId={portfolio.Id}&{newPhoto.FileName}";
                dbContext.Portfolio.Update(portfolio);
                dbContext.SaveChanges();
                dbContext.PortfolioNewsFiles.Update(portfolioNewsFile);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(portfolioNewsFile.PortfolioId);
            return portfolioNewsFile;
        }

        public PortfolioNewsFile EditMinorPhoto(int? id, IFormFile newPhoto)
        {
            PortfolioNewsFile portfolioNewsFile = _context.PortfolioNewsFiles.FirstOrDefault(p => p.Id == id);
            string oldFilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{portfolioNewsFile.Path}");
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/portfolioFiles/Id={id}&{newPhoto.FileName}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                newPhoto.CopyTo(fileStream);
            }
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                portfolioNewsFile.Path = $"/newsPortfolioFiles/portfolioFiles/Id={id}&{newPhoto.FileName}";
                dbContext.PortfolioNewsFiles.Update(portfolioNewsFile);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(portfolioNewsFile.PortfolioId);
            return portfolioNewsFile;

        }

        public PortfolioDetailsViewModel EditPortfolio(int? id)
        {
            var port = _context.Portfolio.Include(p => p.CarsBodyTypes).Include(p => p.CarsBrands).Include(p => p.CarsBrandsModel).FirstOrDefault(p => p.Id == id);
            List<PortfolioNewsFile> pics = _context.PortfolioNewsFiles.Where(i => i.PortfolioId == id && i.Type == "picture").ToList();
            List<PortfolioNewsFile> vids = _context.PortfolioNewsFiles.Where(v => v.PortfolioId == id && v.Type == "video").ToList();
            PortfolioNewsFile mainPic = _context.PortfolioNewsFiles.Where(m => m.PortfolioId == id && m.Type == "mainPic").FirstOrDefault();
            PortfolioDetailsViewModel portfolioDetailsViewModel = new PortfolioDetailsViewModel
            {
                MinorPictures = pics,
                Videos = vids,
                Portfolio = port,
                MainPic = mainPic,
            };
            return portfolioDetailsViewModel;
        }

        public void EditPortfolio(int? id, Portfolio portfolio)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                portfolio.CreatedDate = DateTime.Now;
                dbContext.Update(portfolio);
                dbContext.SaveChanges();
            }
        }

        public PortfolioNewsFile EditVideo(int? id, string newVideoId)
        {
            PortfolioNewsFile portfolioNewsFile = _context.PortfolioNewsFiles.FirstOrDefault(p => p.Id == id);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                portfolioNewsFile.Path = "https://www.youtube.com/embed/" + newVideoId;
                dbContext.PortfolioNewsFiles.Update(portfolioNewsFile);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(portfolioNewsFile.PortfolioId);
            return portfolioNewsFile;
        }

        public IEnumerable<Portfolio> GetAllPortfolioForAdmin()
        {
            return _context.Portfolio.OrderByDescending(p => p.CreatedDate);
        }

        public FilterPortfolioViewModel GetFieldInspectionPortfolio(int? bodyType, int? brand, int? model)
        {
            IQueryable<Portfolio> portfolios = _context.Portfolio.Where(p => p.Publicate == true && p.IsFieldInspection == true).OrderByDescending(p => p.CreatedDate)
                .Include(p => p.CarsBodyTypes)
                .Include(p => p.CarsBrands)
                .Include(p => p.CarsBrandsModel);
            return GetFilterPortfolioViewModel(portfolios, bodyType, brand, model); 
        }

        public FilterPortfolioViewModel GetTurnkeySelectionPortfolio(int? bodyType, int? brand, int? model)
        {
            IQueryable<Portfolio> portfolios = _context.Portfolio.Where(p => p.Publicate == true && p.IsFieldInspection == false).OrderByDescending(p => p.CreatedDate)
                .Include(p => p.CarsBodyTypes)
                .Include(p => p.CarsBrands)
                .Include(p => p.CarsBrandsModel);
            return GetFilterPortfolioViewModel(portfolios, bodyType, brand, model);
        }

        public void PublicPortfolio(int? id)
        {
            Portfolio portfolio = _context.Portfolio.FirstOrDefault(p => p.Id == id);
            if (portfolio.Publicate == false)
                portfolio.Publicate = true;
            else
                portfolio.Publicate = false;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                dbContext.Update(portfolio);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(portfolio.Id);
        }

        private void UpdateCreationDate(int? id)
        {
            var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
            Portfolio portfolio = _context.Portfolio.FirstOrDefault(p => p.Id == id);
            portfolio.CreatedDate = DateTime.Now;
            dbContext.Portfolio.Update(portfolio);
            dbContext.SaveChangesAsync();
        }

        private FilterPortfolioViewModel GetFilterPortfolioViewModel(IQueryable<Portfolio> portfolios, int? bodyType, int? brand, int? model, int pageNumber = 1)
        {
            if (bodyType != null && bodyType != 0)
            {
                portfolios = portfolios.Where(p => p.CarsBodyTypes.Id == bodyType);
            }
            if (brand != null && brand != 0)
            {
                portfolios = portfolios.Where(p => p.CarsBrands.Id == brand);
            }
            if (model != null && model != 0)
            {
                portfolios = portfolios.Where(p => p.CarsBrandsModel.Id == model);
            }

            List<CarsBodyTypes> carsBodyTypes = _context.CarsBodyTypes.ToList();
            List<CarsBrands> carsBrands = _context.CarsBrands.ToList();
            List<CarsBrandsModel> carsBrandsModels = _context.CarsBrandsModels.ToList();
            carsBodyTypes.Insert(0, new CarsBodyTypes { BodyType = "Все", Id = 0 });
            carsBrands.Insert(0, new CarsBrands { Brand = "Все", Id = 0 });
            carsBrandsModels.Insert(0, new CarsBrandsModel { Model = "Все", Id = 0 });

            var fpvm = new FilterPortfolioViewModel
            {
                Portfolios = PaginationList<Portfolio>.Create(portfolios.ToList(), pageNumber, 5),
                CarsBodyTypes = new SelectList(carsBodyTypes, "Id", "BodyType"),
                CarsBrands = new SelectList(carsBrands, "Id", "Brand"),
                CarsModels = new SelectList(carsBrandsModels, "Id", "Model"),
            };
            return fpvm;
        }

        public IEnumerable<CarsBrands> GetAllCarsBrands()
        {
            return _context.CarsBrands;
        }

        public IEnumerable<CarsBrandsModel> GetAllCarsBrandsModel()
        {
            return _context.CarsBrandsModels;
        }

        public IEnumerable<CarsBodyTypes> GetAllCarsBodyTypes()
        {
            return _context.CarsBodyTypes;
        }
    }
}
