using AutoMapper;
using dbthirstthing.DataContext;
using dbthirstthing.DTO;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace dbthirstthing.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPreregistrationService _preregistrationService;
        private readonly IUserService _userService;

        public AdminController(IPreregistrationService preregistrationService, IUserService userService)
        {
            _preregistrationService = preregistrationService;
            _userService = userService;
        }

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Waitlist()
        {
            var roles = _userService.GetAllRoles();
            ViewBag.Roles = roles;
            var preregistrations = _preregistrationService.GetPreregistrationList();
            return View(preregistrations);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllUsers()
        {
            var userDTOs = _userService.GetUsers();
            return View(userDTOs);
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

            var preregistration = _preregistrationService.GetPreregistration(id);
            if (preregistration == null)
            {
                return HttpNotFound();
            }

            _preregistrationService.DeletePreregistration(id);

            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int? id)
        //{
        //    _preregistrationService.DeletePreregistration((int)id);
        //    return RedirectToAction("Waitlist");


        //}


        [Authorize(Roles = "Admin")]
        public ActionResult AcceptPreregistration(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _preregistrationService.AcceptPreregistration(id);
                return RedirectToAction("Waitlist");
            


        }


    }


   
}