using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static dbthirstthing.Services.AuthenticationService;

namespace dbthirstthing.Interfaces
{
    public interface IAuthenticationService
    {
        bool ValidateCredentials(string username, string passwordHash);
        TwoFactorAuthResult GenerateTwoFactorAuthKey(string username);
        bool ValidateTwoFactorAuthCode(string username, string code);
    }
}
