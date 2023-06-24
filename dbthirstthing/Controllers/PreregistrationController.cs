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
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using dbthirstthing.Services;
using hbehr.recaptcha;
using Microsoft.Ajax.Utilities;
using NLog;

namespace dbthirstthing.Controllers
{
    public class PreregistrationController : Controller
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IPreregistrationService _preregistrationService;

        public PreregistrationController(IPreregistrationService preregistrationService)
        {
            _preregistrationService = preregistrationService;
        }

        // GET: PreregistrationModels1
        public ActionResult Index()
        {
            logger.Info("preregistrations list accessed. ");
            var preregistrations = _preregistrationService.GetPreregistrationList();
            return View(preregistrations);
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

                if (!string.IsNullOrEmpty(userResponse) && ReCaptcha.ValidateCaptcha(userResponse)) //каптча
                {
                    _preregistrationService.AddPreregistration(preregistrationModel);
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
            base.Dispose(disposing);
        }

        public ActionResult IsEmailValid(string email)
        {
            bool isValid = _preregistrationService.IsEmailValid(email);
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsLoginValid(string login)
        {
            bool isValid = _preregistrationService.IsLoginValid(login);
            return Json(isValid, JsonRequestBehavior.AllowGet);
        }


    }




}
