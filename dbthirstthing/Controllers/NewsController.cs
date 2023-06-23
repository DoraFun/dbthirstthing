using dbthirstthing.DataContext;
using dbthirstthing.Filters;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using dbthirstthing.Services;
using Ninject;
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
        //public NewsController() 
        //{
        //    IKernel ninjectKernel = new StandardKernel();
        //    ninjectKernel.Bind<INewsService>().To<NewsService>();
        //    newsService = ninjectKernel.Get<INewsService>();
        //}

        private readonly INewsService newsService;
        private readonly int pageSize = 10; // количество объектов на страницу

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        //[JwtAuthorizationFilter]
        public async Task<ActionResult> Index(int page = 1)
        {
            var query = await newsService.GetAllNews();
            int totalNewsCount = query.Count();
            int totalPagesCount = (int)System.Math.Ceiling((double)totalNewsCount / pageSize);

            var newsOnCurrentPage = query.Skip((page - 1) * pageSize)
                .Take(pageSize);

            var pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalNewsCount };
            var ivm = new IndexViewModel { PageInfo = pageInfo, News = newsOnCurrentPage };
            return View(ivm);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}