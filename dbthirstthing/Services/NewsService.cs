using dbthirstthing.DataContext;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static dbthirstthing.Models.NewsViewModel;

namespace dbthirstthing.Services
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<NewsModel>> GetAllNews()
        {
            return await _dbContext.News.ToListAsync();
        }
    }
}