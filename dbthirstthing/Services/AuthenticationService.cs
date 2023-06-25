using dbthirstthing.DataContext;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;

namespace dbthirstthing.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool ValidateCredentials(string username, string passwordHash)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.SingleOrDefault(u => u.login == username);

                if (user == null)
                    return false;

                return Crypto.VerifyHashedPassword(user.pass, passwordHash);
            }
        }

        public class TwoFactorAuthResult
        {
            public string QrCodeUrl { get; set; }
            public string ManualEntryKey { get; set; }
        }

        public TwoFactorAuthResult GenerateTwoFactorAuthKey(string username)
        {
            string googleAuthKey = WebConfigurationManager.AppSettings["GoogleAuthKey"];
            string userUniqueKey = (username + googleAuthKey);

            var twoFacAuth = new TwoFactorAuthenticator();
            var setupInfo = twoFacAuth.GenerateSetupCode("UdayDodiyaAuthDemo.com", username, ConvertSecretToBytes(userUniqueKey, false), 300);

            var result = new TwoFactorAuthResult
            {
                QrCodeUrl = setupInfo.QrCodeSetupImageUrl,
                ManualEntryKey = setupInfo.ManualEntryKey
            };

            return result;
        }

        public bool ValidateTwoFactorAuthCode(string username, string code)
        {
            var userUniqueKey = (username + WebConfigurationManager.AppSettings["GoogleAuthKey"]);
            var twoFacAuth = new TwoFactorAuthenticator();

            return twoFacAuth.ValidateTwoFactorPIN(userUniqueKey, code, false);
        }

        private byte[] ConvertSecretToBytes(string secret, bool secretIsBase32) =>
            secretIsBase32 ? Base32Encoding.ToBytes(secret) : Encoding.UTF8.GetBytes(secret);
    }
}