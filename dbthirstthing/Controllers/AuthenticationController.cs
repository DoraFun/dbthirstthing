using dbthirstthing.DataContext;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Google.Authenticator;
using hbehr.recaptcha;

using hbehr.recaptcha.Exceptions;
using Jose;
using Microsoft.Ajax.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace dbthirstthing.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly IAuthenticationService _authenticationService;
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly HttpClient httpClient;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            this.httpClient = new HttpClient();
        }

        public ActionResult Login()
        {
            Session["Name"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            logger.Info("Loggin page accessed. ");
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

            string userResponse = HttpContext.Request.Params["g-recaptcha-response"];
            if (string.IsNullOrEmpty(userResponse) || !ReCaptcha.ValidateCaptcha(userResponse))
            {
                ModelState.AddModelError("", "Подтвердите, что вы не робот для продолжения");
                return View(model);
            }

            if (!_authenticationService.ValidateCredentials(model.Name, model.Password))
            {
                ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                return View(model);
            }

            Session["Name"] = model.Name;


            var twoFacAuthKey = _authenticationService.GenerateTwoFactorAuthKey(model.Name);
            Session["UserUniqueKey"] = twoFacAuthKey;

            //var payload = new { userid = userId };
            //var token = JwtManager.CreateToken(payload, TimeSpan.FromMinutes(30));
            FormsAuthentication.SetAuthCookie(model.Name, true);

            ViewBag.BarcodeImageUrl = twoFacAuthKey.QrCodeUrl;
            ViewBag.SetupCode = twoFacAuthKey.ManualEntryKey;
            ViewBag.Status = true;







            return View(model);
        }

        public ActionResult TwoFactorAuthenticate(LoginModel model)
        {
            var token = Request["CodeDigit"];
            var username = Session["Name"].ToString();

            if (!_authenticationService.ValidateTwoFactorAuthCode(username, token))
            {
                ModelState.AddModelError("", "The token you entered is invalid. Please try again.");
                Session["IsValidTwoFactorAuthentication"] = false;

                return View("Login", model);
            }
            Session["IsValidTwoFactorAuthentication"] = true;

            string secretKey = "MySecretKey";
            var tokenPayload = new Dictionary<string, object>()
            {
                {"userLogin", username},

            };
            string JwtToken = Jose.JWT.Encode(tokenPayload, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);

            HttpCookie cookie = new HttpCookie("jwt");
            cookie.Value = JwtToken;
            cookie.Expires = DateTime.Now.AddMinutes(30);
            Response.Cookies.Add(cookie);
            //this.httpClient.DefaultRequestHeaders.Authorization =
            //   new AuthenticationHeaderValue("Bearer", JwtToken);

            if (_authenticationService.FirstAuth(username))
            { return View("../Password/ChangePassword"); }
            else
            {
                return RedirectToAction("Index", "Home");
            }
                

            
        }

        public ActionResult Logoff()
        {
            Session["Name"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}