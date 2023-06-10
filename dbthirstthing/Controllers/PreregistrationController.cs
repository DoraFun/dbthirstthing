using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using dbthirstthing.DataContext;
using dbthirstthing.Models;
using hbehr.recaptcha;
using Microsoft.Ajax.Utilities;
using NLog;

namespace dbthirstthing.Controllers
{
    public class PreregistrationController : Controller
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PreregistrationModels1
        public ActionResult Index()
        {
            logger.Info("preregistrations list accessed. ");
            return View(db.Preregistration.ToList());
        }


        // GET: PreregistrationModels1/Create
        public ActionResult Create()
        {
            logger.Info("preregistration page accessed. ");
            return View();
        }

        // POST: PreregistrationModels1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,displayname,login,email,aboutuser")] PreregistrationModel preregistrationModel)
        {
            if (ModelState.IsValid)
            {
                string userResponse = HttpContext.Request.Params["g-recaptcha-response"];
                if (!string.IsNullOrEmpty(userResponse) && ReCaptcha.ValidateCaptcha(userResponse)) /*каптча*/
                {
                    db.Preregistration.Add(preregistrationModel);
                    db.SaveChanges();
                    logger.Info("New preregistration added. ");
                    return View("./PreRegistrationConfirmed");
                }
                else
                {
                    logger.Info("Someone failed captcha. ");
                    ModelState.AddModelError("", "Подтвердите, что вы не робот для продолжения");
                    // Bot Attack, non validated !


                }
            }

            return View(preregistrationModel);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult IsEmailValid(string email)
        {
            bool isExistPreregistration = false;
            bool isExistUser = false;

            using (var db = new ApplicationDbContext())
            {
                isExistPreregistration = db.Preregistration.Any(u => u.email == email);
                isExistUser = db.Users.Any(u => u.email == email);


            }

            if (!isExistUser && !isExistPreregistration)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ActionName("IsLoginValid")]
        public ActionResult IsLoginValid(string login)
        {
            bool isExistPreregistration = false;
            bool isExistUser = false;

            using (var db = new ApplicationDbContext())
            {
                //мб сделать их ассинхронными ? Надо почитать про это
                isExistPreregistration = db.Preregistration.Any(u => u.login == login);
                isExistUser = db.Users.Any(u => u.login == login);

            }

            if (!isExistUser && !isExistPreregistration)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
    }

    
}
