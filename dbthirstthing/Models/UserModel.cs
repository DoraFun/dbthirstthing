﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
    }
}