using AutoMapper;
using dbthirstthing.DataContext;
using dbthirstthing.DTO;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace dbthirstthing.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<RoleModel> GetAllRoles()
        {
            return _db.Roles.ToList();
        }

        public List<UserDTO> GetUsers()
        {
            var users = _db.Users.Include("RoleModel").ToList();
            return Mapper.Map<List<UserDTO>>(users);
        }

        public void CreateUser(UserModel userModel)
        {
            var randompassword = Crypto.GenerateSalt(8);
            var newUser = new UserModel
            {
                displayname = userModel.displayname,
                login = userModel.login,
                email = userModel.email,
                pass = HashPassword(randompassword),
                /*интернет мужики говорят что норм тема*/
                neverlogged = true,
            };
            string filePath = HttpContext.Current.Server.MapPath($"~/messages/{newUser.login}_confirmation.txt");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Ваш одноразовый пароль для первого входа: {randompassword} обязательно смените его после авторизации");
            }
            _db.Users.Add(newUser);
            _db.SaveChanges();
        }

        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher();
            return hasher.HashPassword(password);
        }
    }
}