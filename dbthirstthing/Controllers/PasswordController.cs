using dbthirstthing.DataContext;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using dbthirstthing.Services;
using hbehr.recaptcha;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace dbthirstthing.Controllers
{
    public class PasswordController : Controller
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        // GET: Password
        private readonly IPasswordService _passwordService;

        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost] /*пут ломает код ?*/
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                string userResponse = HttpContext.Request.Params["g-recaptcha-response"];
                if (!string.IsNullOrEmpty(userResponse) && ReCaptcha.ValidateCaptcha(userResponse)) /*каптча*/
                {
                    // поиск пользователя в бд
                    if (_passwordService.ChangePassword(model) == true)
                        return RedirectToAction("../Home/Index");
                    ModelState.AddModelError("", "Такого пользователя нет");
                }
                else
                {
                    logger.Info("Someone failed captcha. ");
                    ModelState.AddModelError("", "Подтвердите, что вы не робот для продолжения");
                    // Bot Attack, non validated !


                }
            }

            return View(model);
        }
    }
}