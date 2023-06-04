using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using dbthirstthing.DataContext;
using dbthirstthing.Models;

namespace dbthirstthing.Controllers
{
    public class PreregistrationModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PreregistrationModels
        public ActionResult Index()
        {
            return View(db.Preregistration.ToList());
        }

        // GET: PreregistrationModels/Details/5
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

        // GET: PreregistrationModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PreregistrationModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,displayname,login,pass,email")] PreregistrationModel preregistrationModel)
        {
            if (ModelState.IsValid)
            {
                db.Preregistration.Add(preregistrationModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(preregistrationModel);
        }

        // GET: PreregistrationModels/Edit/5
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

        // POST: PreregistrationModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,displayname,login,pass,email")] PreregistrationModel preregistrationModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(preregistrationModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(preregistrationModel);
        }

        // GET: PreregistrationModels/Delete/5
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

        // POST: PreregistrationModels/Delete/5
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
    }
}
