using dbthirstthing.DataContext;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace dbthirstthing.Services
{
    public class NewsService :INews
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<NewsModel>> GetNewsAsync(int page = 1, int pageSize = 10)
        {
            var query = _dbContext.News.OrderByDescending(n => n.newsdate);
            int totalNewsCount = await query.CountAsync();
            int totalPagesCount = (int)System.Math.Ceiling((double)totalNewsCount / pageSize);

            var newsOnCurrentPage = await query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return newsOnCurrentPage;
        }
    }
}
