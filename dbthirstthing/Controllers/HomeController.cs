using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbthirstthing.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {

            
                logger.Info("Main page accessed. ");
            
            
            return View();
        }

        public ActionResult About()
        {

                logger.Info("About page accessed. ");
            
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            logger.Info("Contact page accessed. ");
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}