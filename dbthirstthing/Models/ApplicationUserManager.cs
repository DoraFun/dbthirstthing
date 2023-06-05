using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using dbthirstthing.DataContext;

namespace dbthirstthing.Models
{
    public class ApplicationUserManager : UserManager<UserModel>
    {
        public ApplicationUserManager(IUserStore<UserModel> store)
        : base(store)
        {
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
                                                IOwinContext context)
        {
            ApplicationDbContext db = context.Get<ApplicationDbContext>();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<UserModel>(db));
            return manager;
        }
    }
}