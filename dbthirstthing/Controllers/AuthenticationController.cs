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
            if (ModelState.IsValid)
            {
                bool status = false;

                if (Session["Name"] == null || Session["IsValidTwoFactorAuthentication"] == null || !(bool)Session["IsValidTwoFactorAuthentication"])
                {
                    string googleAuthKey = WebConfigurationManager.AppSettings["GoogleAuthKey"];
                    string UserUniqueKey = (model.Name + googleAuthKey);

                    string userResponse = HttpContext.Request.Params["g-recaptcha-response"];
                    if (!string.IsNullOrEmpty(userResponse) && ReCaptcha.ValidateCaptcha(userResponse)) /*каптча*/
                    {
                        UserModel user = null;
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            user = db.Users.FirstOrDefault(u => u.login == model.Name);
                            //ААААААААААААААААААААААААААААААА

                            if (user != null && Crypto.VerifyHashedPassword(user.pass, model.Password) == true) /*пиздец костыль*/
                            {
                                Session["Name"] = model.Name;
                                

                                TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
                                var setupInfo = TwoFacAuth.GenerateSetupCode("UdayDodiyaAuthDemo.com", model.Name, ConvertSecretToBytes(UserUniqueKey, false), 300);
                                Session["UserUniqueKey"] = UserUniqueKey;
                                ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                                ViewBag.SetupCode = setupInfo.ManualEntryKey;
                                status = true;

                                //if (user.neverlogged != true)
                                //{
                                //    return RedirectToAction("Index", "Home");
                                //}
                                //else
                                //{
                                //    //user.onetimepassword = null;
                                //    //db.SaveChanges(); /*Выглядит как такое себе решение*/
                                //    return RedirectToAction("ChangePassword", "Password"); /*надо бы добавить страничку ддля объяснения, зачем его менять*/
                                //}



                            }
                            else
                            {
                                ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Подтвердите, что вы не робот для продолжения");
                        // Bot Attack, non validated !


                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Status = status;

            }

            return View(model);
        }

        
        public ActionResult Logoff()
        {
            Session["Name"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32) =>
           secretIsBase32 ? Base32Encoding.ToBytes(secret) : Encoding.UTF8.GetBytes(secret);

        public ActionResult TwoFactorAuthenticate(LoginModel model)
        {
            var token = Request["CodeDigit"];
            TwoFactorAuthenticator TwoFacAuth = new TwoFactorAuthenticator();
            string UserUniqueKey = Session["UserUniqueKey"].ToString();
            bool isValid = TwoFacAuth.ValidateTwoFactorPIN(UserUniqueKey, token, false);
            if (isValid)
            {
                HttpCookie TwoFCookie = new HttpCookie("TwoFCookie");
                string UserCode = Convert.ToBase64String(MachineKey.Protect(Encoding.UTF8.GetBytes(UserUniqueKey)));
;
                Session["IsValidTwoFactorAuthentication"] = true;
                FormsAuthentication.SetAuthCookie((string)Session["Name"], true);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Google Two Factor PIN is expired or wrong";
            return RedirectToAction("Login");
        }
    }
}