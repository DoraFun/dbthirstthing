using dbthirstthing.DataContext;
using dbthirstthing.Models;
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
                    UserModel user = null;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        user = db.Users.FirstOrDefault(u => u.email == model.Email);
                        //ААААААААААААААААААААААААААААААА

                        if (user != null && Crypto.VerifyHashedPassword(user.pass, model.OldPassword) == true) /*пиздец костыль*/
                        {

                            if (user.neverlogged == true)
                            {
                                user.neverlogged = false;

                            }

                            user.pass = Crypto.HashPassword(model.NewPassword);
                            db.SaveChanges();
                            logger.Info("User changed password. ");
                            return RedirectToAction("Index", "Home");
                            //тут определенно надо бы отправлять еще код подтверждения

                        }
                        else
                        {
                            logger.Info("Someone tried to change user password and failed. ");
                            ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                        }
                    }
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