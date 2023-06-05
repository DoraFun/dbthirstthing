using dbthirstthing.DataContext;
using dbthirstthing.Models;
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
                // поиск пользователя в бд
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
                            ModelState.AddModelError("", "Чел не входил");
                        }


                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                    }
                }
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