using dbthirstthing.DataContext;
using dbthirstthing.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static dbthirstthing.Models.NewsViewModel;

namespace dbthirstthing.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly int pageSize = 3; // количество объектов на страницу

        public ActionResult Index(int page = 1)
        {
            int totalNewsCount = db.News.Count();
            int totalPagesCount = (int)System.Math.Ceiling((double)totalNewsCount / pageSize);

            List<NewsModel> newsOnCurrentPage = db.News
                                                .OrderByDescending(n => n.newsdate)
                                                .Skip((page - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToList();
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalNewsCount };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, News = newsOnCurrentPage };
            return View(ivm);
        }
    }
}