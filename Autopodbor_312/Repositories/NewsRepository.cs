using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using Autopodbor_312.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Autopodbor_312.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly AutopodborContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public NewsRepository(AutopodborContext autopodborContext, IWebHostEnvironment webHost, IServiceScopeFactory serviceScopeFactory)
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
                PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile { Type = "picture", NewsId = id };
                dbContext.PortfolioNewsFiles.Add(portfolioNewsFile);
                dbContext.SaveChanges();
                portfolioNewsFile.Path = $"/newsPortfolioFiles/newsFiles/Id={portfolioNewsFile.Id}&{newPhoto.FileName}";
                dbContext.PortfolioNewsFiles.Update(portfolioNewsFile);
                dbContext.SaveChanges();
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/Id={portfolioNewsFile.Id}&{newPhoto.FileName}");
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
                PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + videoId, Type = "video", NewsId = id };
                dbContext.PortfolioNewsFiles.Add(portfolioNewsFile);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(id);
        }

        public void CreateNews(News news, IFormFile mainPic, IFormFileCollection uploadFiles, string video)
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
                string newsFolderPath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/newsPortfolioFiles/newsFiles");
                DirectoryInfo newsInfo = new DirectoryInfo(newsFolderPath);
                if (!newsInfo.Exists)
                {
                    newsInfo.Create();
                }

                news.CreatedDate = DateTime.Now;
                news.Publicate = false;
                dbContext.News.Add(news);
                dbContext.SaveChanges();
                news.MainImagePath = $"/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{mainPic.FileName}";
                if (mainPic != null)
                {
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{mainPic.FileName}");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        mainPic.CopyTo(fileStream);
                    }
                    PortfolioNewsFile update = new PortfolioNewsFile { Path = $"/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{mainPic.FileName}", Type = "mainPic", NewsId = news.Id };
                    dbContext.PortfolioNewsFiles.Add(update);
                    dbContext.SaveChanges();
                }
                if (uploadFiles != null)
                {
                    foreach (var upload in uploadFiles)
                    {
                        PortfolioNewsFile portfolioNewsFile = new PortfolioNewsFile { Type = "picture", NewsId = news.Id };
                        dbContext.PortfolioNewsFiles.Add(portfolioNewsFile);
                        dbContext.SaveChanges();
                        string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/Id={portfolioNewsFile.Id}&{upload.FileName}");
                        portfolioNewsFile.Path = $"/newsPortfolioFiles/newsFiles/Id={portfolioNewsFile.Id}&{upload.FileName}";
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            upload.CopyTo(fileStream);
                        }
                        dbContext.SaveChanges();
                    }
                    if (video != null)
                    {
                        string[] paths = video.Split(' ');
                        foreach (var path in paths)
                        {
                            PortfolioNewsFile vid = new PortfolioNewsFile { Path = "https://www.youtube.com/embed/" + path, Type = "video", NewsId = news.Id };
                            dbContext.Add(vid);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
        }

        public News DeleteNews(int? id)
        {
            News news = _context.News.FirstOrDefault(p => p.Id == id);
            return news;
        }

        public void DeleteConfirmedNews(int? id)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                News news = dbContext.News.FirstOrDefault(p => p.Id == id);
                List<PortfolioNewsFile> portfolioNewsFiles = dbContext.PortfolioNewsFiles.Where(p => p.NewsId == id).ToList();
                foreach (var n in portfolioNewsFiles)
                {
                    string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{n.Path}");
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    dbContext.PortfolioNewsFiles.Remove(n);
                }
                dbContext.SaveChanges();
                dbContext.Remove(news);
                dbContext.SaveChanges();
            }
        }

        public PortfolioNewsFile DeletePhotoOrVideo(int? id)
        {
            PortfolioNewsFile portfolioNewsFile = _context.PortfolioNewsFiles.FirstOrDefault(p => p.Id == id);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                string FilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{portfolioNewsFile.Path}");
                if (System.IO.File.Exists(FilePath))
                {
                    System.IO.File.Delete(FilePath);
                }
                dbContext.PortfolioNewsFiles.Remove(portfolioNewsFile);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(portfolioNewsFile.NewsId);
            return portfolioNewsFile;
        }

        public NewsDetailsViewModel DetailsNews(int? id)
        {
            News news = _context.News.FirstOrDefault(n => n.Id == id);
            List<PortfolioNewsFile> minorImg = _context.PortfolioNewsFiles.Where(i => i.NewsId == id && i.Type == "picture").ToList();
            List<PortfolioNewsFile> videos = _context.PortfolioNewsFiles.Where(v => v.NewsId == id && v.Type == "video").ToList();
            NewsDetailsViewModel newsDetailsViewModel = new NewsDetailsViewModel { News = news, MinorPictures = minorImg, Videos = videos };
            return newsDetailsViewModel;
        }

        public PortfolioNewsFile EditMainPhoto(int? id, IFormFile newPhoto)
        {
            PortfolioNewsFile portfolioNewsFile = _context.PortfolioNewsFiles.FirstOrDefault(p => p.Id == id);
            News news = _context.News.FirstOrDefault(n => n.Id == portfolioNewsFile.NewsId);
            string oldFilePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot{news.MainImagePath}");
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{newPhoto.FileName}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                newPhoto.CopyTo(fileStream);
            }
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                portfolioNewsFile.Path = $"/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{newPhoto.FileName}";
                news.MainImagePath = $"/newsPortfolioFiles/newsFiles/mainPicId={news.Id}&{newPhoto.FileName}";
                dbContext.News.Update(news);
                dbContext.SaveChanges();
                dbContext.PortfolioNewsFiles.Update(portfolioNewsFile);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(portfolioNewsFile.NewsId);
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
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, $"wwwroot/newsPortfolioFiles/newsFiles/Id={id}&{newPhoto.FileName}");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                newPhoto.CopyTo(fileStream);
            }
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                portfolioNewsFile.Path = $"/newsPortfolioFiles/newsFiles/Id={id}&{newPhoto.FileName}";
                dbContext.PortfolioNewsFiles.Update(portfolioNewsFile);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(portfolioNewsFile.NewsId);
            return portfolioNewsFile;
        }

        public NewsDetailsViewModel EditNews(int? id)
        {
            News news = _context.News.FirstOrDefault(n => n.Id == id);
            List<PortfolioNewsFile> pics = _context.PortfolioNewsFiles.Where(i => i.NewsId == id && i.Type == "picture").ToList();
            List<PortfolioNewsFile> vids = _context.PortfolioNewsFiles.Where(v => v.NewsId == id && v.Type == "video").ToList();
            PortfolioNewsFile mainPic = _context.PortfolioNewsFiles.Where(m => m.NewsId == id && m.Type == "mainPic").FirstOrDefault();
            NewsDetailsViewModel newsDetailsViewModel = new NewsDetailsViewModel { MinorPictures = pics, Videos = vids, News = news, MainPic = mainPic };
            return newsDetailsViewModel;
        }

        public void EditNews(int? id, News news)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                news.CreatedDate = DateTime.Now;
                dbContext.Update(news);
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
            UpdateCreationDate(portfolioNewsFile.NewsId);
            return portfolioNewsFile;
        }

        public IEnumerable<News> GetAllNews()
        {
            List<News> news = _context.News.OrderByDescending(n => n.CreatedDate).ToList();
            return news;
        }

        public IEnumerable<News> GetPublicatedNews()
        {
            List<News> newsPublished = _context.News.Where(n => n.Publicate == true).OrderByDescending(n => n.CreatedDate).ToList();
            return newsPublished;
        }

        public void PublicNews(int? id)
        {
            News news = _context.News.FirstOrDefault(n => n.Id == id);
            if (news.Publicate == false)
                news.Publicate = true;
            else
                news.Publicate = false;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
                dbContext.Update(news);
                dbContext.SaveChanges();
            }
            UpdateCreationDate(id);
        }

        private void UpdateCreationDate(int? id)
        {
            var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AutopodborContext>();
            News news = dbContext.News.FirstOrDefault(p => p.Id == id);
            news.CreatedDate = DateTime.Now;
            dbContext.News.Update(news);
            dbContext.SaveChangesAsync();
        }
    }
}
