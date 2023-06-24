using dbthirstthing.DataContext;
using dbthirstthing.Interfaces;
using dbthirstthing.Models;

using hbehr.recaptcha;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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


    }
}