using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbthirstthing.DTO
{
    public class UserDTO
    {
        public string displayname { get; set; }
        public string login { get; set; }
        public string email { get; set; }

        public int? roleid { get; set; }
        public RoleModel RoleModel { get; set; }
    }
}