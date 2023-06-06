using dbthirstthing.DataContext;
using dbthirstthing.Models;
using hbehr.recaptcha;
using hbehr.recaptcha.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;


namespace dbthirstthing.Controllers
{
    public class AuthenticationController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            { 

                string userResponse = HttpContext.Request.Params["g-recaptcha-response"];
                if (!string.IsNullOrEmpty(userResponse) && ReCaptcha.ValidateCaptcha(userResponse)) /*каптча*/
                {
                    UserModel user = null;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        user = db.Users.FirstOrDefault(u => u.login == model.Name);
                        //ААААААААААААААААААААААААААААААА

                        if (user != null && Crypto.VerifyHashedPassword(user.pass, model.Password) == true) /*пиздец костыль*/
                        {
                            FormsAuthentication.SetAuthCookie(model.Name, true);

                            if (user.neverlogged != true)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                //user.onetimepassword = null;
                                //db.SaveChanges(); /*Выглядит как такое себе решение*/
                                return RedirectToAction("ChangePassword", "Password"); /*надо бы добавить страничку ддля объяснения, зачем его менять*/
                            }



                        }
                        else
                        {
                            ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Подтвердите, что вы не робот для продолжения");
                    // Bot Attack, non validated !

                    
                }
                // поиск пользователя в бд
                
            }

            return View(model);
        }

        
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}