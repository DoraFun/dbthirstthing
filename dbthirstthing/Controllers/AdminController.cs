using dbthirstthing.DataContext;
using dbthirstthing.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace dbthirstthing.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Waitlist()
        {
            return View(db.Preregistration.ToList());
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Confirm")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(int? id, [Bind(Include = "userid,displayname,login,email")] UserModel userModel)
        {
            
                try
                {
                    db.Users.Add(userModel);
                    PreregistrationModel preregistrationModel = db.Preregistration.Find(id);
                    db.Preregistration.Remove(preregistrationModel);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(ex);
                }

            

            
        }
        //БРАТАН ПОЧЕМУ У ТЕБЯ 2 МЕТОДА
        public ActionResult Confirm(int? id)
        {
            var randompassword = Crypto.GenerateSalt(8);
            using (db)
            {
                var usersToMove = db.Preregistration.Where(u => u.userid == id).ToList();

                foreach (var user in usersToMove)
                {
                    var newUser = new UserModel
                    {
                        //userid = user.userid,
                        displayname = user.displayname,
                        login = user.login,
                        email = user.email,
                        pass = HashPassword(randompassword) /*интернет мужики говорят что норм тема*/
                };

                    string filePath = Server.MapPath($"~/messages/{newUser.login}_confirmation.txt");

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine($"Ваш одноразовый пароль для первого входа: {randompassword} обязательно смените его после авторизации");
                    }
                    
                    db.Users.Add(newUser);
                    db.Preregistration.Remove(user);
                }
                
                db.SaveChanges();

                return RedirectToAction("Index");

            }

        }

        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher();
            return hasher.HashPassword(password);
        }

    }


   
}