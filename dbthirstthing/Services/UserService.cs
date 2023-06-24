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



    }
}