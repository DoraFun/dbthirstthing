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
using Microsoft.Ajax.Utilities;

namespace dbthirstthing.Controllers
{
    public class PreregistrationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PreregistrationModels1
        public ActionResult Index()
        {
            return View(db.Preregistration.ToList());
        }

        // GET: PreregistrationModels1/Details/5
        public ActionResult Details(int? id)
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

        // GET: PreregistrationModels1/Create
        public ActionResult Create()
        {
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
                db.Preregistration.Add(preregistrationModel);
                db.SaveChanges();
                return View("./PreRegistrationConfirmed");
            }

            return View(preregistrationModel);
        }

        // GET: PreregistrationModels1/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: PreregistrationModels1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,displayname,login,email,aboutuser")] PreregistrationModel preregistrationModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(preregistrationModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(preregistrationModel);
        }

        // GET: PreregistrationModels1/Delete/5
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

        // POST: PreregistrationModels1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PreregistrationModel preregistrationModel = db.Preregistration.Find(id);
            db.Preregistration.Remove(preregistrationModel);
            db.SaveChanges();
            return RedirectToAction("Index");
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
