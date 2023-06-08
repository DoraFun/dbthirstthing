using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Web;

namespace dbthirstthing.Models
{
    [Table("roles", Schema = "public")]
    public class RoleModel
    {
        [Key]
        public int roleid { get; set; }

        [Required]
        public string rolename { get; set; }

        public ICollection<RightModel> rights { get; set; }

        //public ICollection<UserModel> Users { get; set; }
        //public RoleModel()
        //{
        //    Users = new List<UserModel>();
        //}



    }
}