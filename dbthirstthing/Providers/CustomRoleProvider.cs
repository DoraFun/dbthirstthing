using dbthirstthing.DataContext;
using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace dbthirstthing.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            
            string[] roles = new string[] { };
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Получаем пользователя
                UserModel user = db.Users.Include("RoleModel").FirstOrDefault(u => u.login == username);
                if (user != null && user.RoleModel != null)
                {
                    // получаем роль
                    roles = new string[] { user.RoleModel.rolename };
                }
                return roles;
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Получаем пользователя
                UserModel user = db.Users.Include("RoleModel").FirstOrDefault(u => u.login == username);

                if (user != null && user.RoleModel != null && user.RoleModel.rolename == roleName)
                    return true;
                else
                    return false;
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}