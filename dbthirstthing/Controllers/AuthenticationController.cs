using dbthirstthing.DataContext;
using dbthirstthing.Models;
using DocumentFormat.OpenXml.EMMA;
using Google.Authenticator;
using hbehr.recaptcha;
using hbehr.recaptcha.Exceptions;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;


namespace dbthirstthing.Controllers
{
    public class AuthenticationController : Controller
    {

        public ActionResult Login()
        {
            Session["Name"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (Session["Name"] != null && Session["IsValidTwoFactorAuthentication"] != null && (bool)Session["IsValidTwoFactorAuthentication"])
                return RedirectToAction("Index");

            string googleAuthKey = WebConfigurationManager.AppSettings["GoogleAuthKey"];
            string userUniqueKey = (model.Name + googleAuthKey);

            string userResponse = HttpContext.Request.Params["g-recaptcha-response"];
            if (string.IsNullOrEmpty(userResponse) || !ReCaptcha.ValidateCaptcha(userResponse))
            {
                ModelState.AddModelError("", "Подтвердите, что вы не робот для продолжения");
                return View(model);
            }

            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.SingleOrDefault(u => u.login == model.Name);

                if (user == null || !Crypto.VerifyHashedPassword(user.pass, model.Password))
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                    return View(model);
                }

                Session["Name"] = model.Name;

                var twoFacAuth = new TwoFactorAuthenticator();
                var setupInfo = twoFacAuth.GenerateSetupCode("UdayDodiyaAuthDemo.com", model.Name, ConvertSecretToBytes(userUniqueKey, false), 300);
                Session["UserUniqueKey"] = userUniqueKey;
                ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                ViewBag.SetupCode = setupInfo.ManualEntryKey;
            }

            ViewBag.Status = true;
            return View(model);
        }

        public ActionResult TwoFactorAuthenticate(LoginModel model)
        {
            var token = Request["CodeDigit"];
            var twoFacAuth = new TwoFactorAuthenticator();
            var userUniqueKey = Session["UserUniqueKey"].ToString();
            bool isValid = twoFacAuth.ValidateTwoFactorPIN(userUniqueKey, token, false);

            if (!isValid)
            {
                ViewBag.Message = "Google Two Factor PIN is expired or wrong";
                return RedirectToAction("Login");
            }

            var userCode = Convert.ToBase64String(MachineKey.Protect(Encoding.UTF8.GetBytes(userUniqueKey)));

            Session["IsValidTwoFactorAuthentication"] = true;
            using (var db = new ApplicationDbContext())
            {
                string uname = (string)Session["Name"];
                var user = db.Users.SingleOrDefault(u => u.login == uname);
                FormsAuthentication.SetAuthCookie(user.login, true);

                if (user.neverlogged != true)
                    return RedirectToAction("Index", "Home");

                return RedirectToAction("ChangePassword", "Password"); // Add explanation page for why the password should be changed
            }
        }

        private byte[] ConvertSecretToBytes(string secret, bool secretIsBase32) =>
            secretIsBase32 ? Base32Encoding.ToBytes(secret) : Encoding.UTF8.GetBytes(secret);

        public ActionResult Logoff()
        {
            Session["Name"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}