using dbthirstthing.DataContext;
using dbthirstthing.Models;
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
        public ActionResult Confirm(int? id, [Bind(Include = "userid,displayname,login,email,onetimepassword")] UserModel userModel)
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

        public ActionResult Confirm(int? id)
        {
            using (db)
            {
                var usersToMove = db.Preregistration.Where(u => u.userid == id).ToList();

                foreach (var user in usersToMove)
                {
                    var newUser = new UserModel
                    {
                        userid = user.userid,
                        displayname = user.displayname,
                        login = user.login,
                        email = user.email,
                        onetimepassword = Crypto.GenerateSalt(8)
                    };

                    string filePath = Server.MapPath($"~/messages/{newUser.login}_confirmation.txt");

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine($"Ваш одноразовый пароль для первого входа: {newUser.onetimepassword}");
                    }

                    db.Users.Add(newUser);
                    db.Preregistration.Remove(user);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }


   
}