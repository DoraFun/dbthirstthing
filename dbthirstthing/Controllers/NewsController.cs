using dbthirstthing.DataContext;
using dbthirstthing.Models;
using NLog;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static dbthirstthing.Models.NewsViewModel;

namespace dbthirstthing.Controllers
{

    public class NewsController : Controller
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly int pageSize = 10; // количество объектов на страницу

        public async Task<ActionResult> Index(int page = 1)
        {
            var query = db.News.OrderByDescending(n => n.newsdate);
            int totalNewsCount = await query.CountAsync();
            int totalPagesCount = (int)System.Math.Ceiling((double)totalNewsCount / pageSize);

            var newsOnCurrentPage = await query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalNewsCount };
            var ivm = new IndexViewModel { PageInfo = pageInfo, News = newsOnCurrentPage };
            logger.Info("News accessed. ");
            return View(ivm);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}