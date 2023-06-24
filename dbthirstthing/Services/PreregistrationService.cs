using dbthirstthing.DataContext;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;

using hbehr.recaptcha;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace dbthirstthing.Services
{
    public class PreregistrationService : IPreregistrationService
    {
        private readonly ApplicationDbContext _db;

        public PreregistrationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<PreregistrationModel> GetPreregistrationList()
        {
            return _db.Preregistration.ToList();
        }

        public void AddPreregistration(PreregistrationModel preregistrationModel)
        {
            _db.Preregistration.Add(preregistrationModel);
            _db.SaveChanges();
        }

        public PreregistrationModel GetPreregistration(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _db.Preregistration.Find(id);
        }

        public void DeletePreregistration(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var preregistration = _db.Preregistration.Find(id);
            if (preregistration == null)
            {
                throw new ArgumentException("Invalid preregistration ID");
            }

            _db.Preregistration.Remove(preregistration);
            _db.SaveChanges();
        }

        public void AcceptPreregistration(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            // Get the user from Preregistration table by ID
            var prepUser = GetPreregistration(id);

            // Check if the user was found
            if (prepUser == null)
            {
                throw new Exception("User not found in PrepRegistration.");
            }

            // Generate a random password
            var randomPassword = Crypto.GenerateSalt(8);

            // Create a new User object with the properties from the PrepRegistration user
            var newUser = new UserModel
            {
                displayname = prepUser.displayname,
                login = prepUser.login,
                email = prepUser.email,
                pass = HashPassword(randomPassword),
                neverlogged = true
            };

            // Write the confirmation message to a file
            string filePath = HttpContext.Current.Server.MapPath($"~/messages/{newUser.login}_confirmation.txt");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Ваш одноразовый пароль для первого входа: {randomPassword}. Обязательно смените его после авторизации");
            }

            // Add the new user to the Users table in the database
            _db.Users.Add(newUser);

            // Remove the PreRegistration user from the table
            _db.Preregistration.Remove(prepUser);

            // Save changes to the database
            _db.SaveChanges();
        }

        public bool IsEmailValid(string email)
        {
            bool isExistPreregistration = _db.Preregistration.Any(u => u.email == email);
            bool isExistUser = _db.Users.Any(u => u.email == email);
            return !isExistUser && !isExistPreregistration;
        }

        public bool IsLoginValid(string login)
        {
            bool isExistPreregistration = _db.Preregistration.Any(u => u.login == login);
            bool isExistUser = _db.Users.Any(u => u.login == login);
            return !isExistUser && !isExistPreregistration;
        }

        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher();
            return hasher.HashPassword(password);
        }


    }
}