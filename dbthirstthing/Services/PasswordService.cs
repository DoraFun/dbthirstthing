using dbthirstthing.DataContext;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using hbehr.recaptcha;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace dbthirstthing.Services
{
    public class PasswordService : IPasswordService
    {

       public bool ChangePassword(ChangePasswordModel model)
        {
            UserModel user = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                user = db.Users.FirstOrDefault(u => u.email == model.Email);
                //ААААААААААААААААААААААААААААААА
                var randomPassword = Crypto.GenerateSalt(8);
                if (user != null && Crypto.VerifyHashedPassword(user.pass, model.OldPassword) == true) /*пиздец костыль*/
                {

                    if (user.neverlogged == true)
                    {
                        user.neverlogged = false;

                    }

                    user.pass = Crypto.HashPassword(model.NewPassword);
                    db.SaveChanges();
                    return true;
                    
                    //тут определенно надо бы отправлять еще код подтверждения

                }
                return false;
            }
        }
    }
}