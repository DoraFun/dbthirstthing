
using dbthirstthing.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbthirstthing.Models
{
    [Table("users",Schema = "public")]

    public class UserModel
    {
        [Key]
        public int userid { get; set; }

        public string displayname { get; set; }

        public string login { get; set; }
        public string pass { get; set; }

        
        public string email { get; set; }

        //public string onetimepassword { get; set; }
        public bool neverlogged { get; set; }

        public int? roleid { get; set; }
        public RoleModel RoleModel { get; set; }
    }
}