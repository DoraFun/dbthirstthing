using dbthirstthing.DataContext;
using dbthirstthing.Models;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace dbthirstthing.Controllers
{
    public class AdminController : Controller
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin

        [Authorize(Roles = "Admin")]
        public ActionResult Waitlist()
        {
            List<RoleModel> roles = db.Roles.ToList();
            ViewBag.Roles = roles;
            return View(db.Preregistration.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllUsers()
        {
            return View(db.Users.Include("RoleModel").ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreregistrationModel preregistrationModel = db.Preregistration.Find(id);
            
            if (preregistrationModel == null)
            {
                return HttpNotFound();
            }
            
            return View(preregistrationModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {

            try
            {
                PreregistrationModel preregistrationModel = db.Preregistration.Find(id);
                db.Preregistration.Remove(preregistrationModel);
                db.SaveChanges();
                logger.Info($"User with id {id} was denied. ");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error($"{ex} while tried to delete user with id {id} ");
                return View(ex);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Confirm")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(int? id, [Bind(Include = "userid,displayname,login,email, roleid")] UserModel userModel)
        {
          
                try
                {
                    db.Users.Add(userModel);
                   
                PreregistrationModel preregistrationModel = db.Preregistration.Find(id);
                    db.Preregistration.Remove(preregistrationModel);
                    db.SaveChanges();
                    logger.Info($" user with id {id} was accepted ");
                return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                logger.Error($"{ex} while tried to accept user with id {id} ");
                return View(ex);
                }

            

            
        }
        //БРАТАН ПОЧЕМУ У ТЕБЯ 2 МЕТОДА
        [Authorize(Roles = "Admin")]
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
                        pass = HashPassword(randompassword), /*интернет мужики говорят что норм тема*/
                        neverlogged = true,

                        


                };

                    string filePath = Server.MapPath($"~/messages/{newUser.login}_confirmation.txt");

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine($"Ваш одноразовый пароль для первого входа: {randompassword} обязательно смените его после авторизации");
                    }
                    logger.Info($" user with id {id} was accepted ");
                    db.Users.Add(newUser);
                    db.Preregistration.Remove(user);
                }
                
                db.SaveChanges();

                return RedirectToAction("Index");

            }

        }

        [Authorize(Roles = "Admin")]
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher();
            return hasher.HashPassword(password);
        }



    }


   
}