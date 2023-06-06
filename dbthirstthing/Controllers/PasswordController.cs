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
    public class PasswordController : Controller
    {
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
                        return RedirectToAction("Index", "Home");


                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                    }
                }
            }

            return View(model);
        }
    }
}