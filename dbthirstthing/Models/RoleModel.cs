using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dbthirstthing.Models
{
    [Table("roles", Schema = "public")]
    public class RoleModel
    {
        public int roleid { get; set; }

        [Required]
        public string rolename { get; set; }

        public virtual ICollection<RightModel> rights { get; set; }
        public virtual ICollection<UserModel> users { get; set; }

        public RoleModel()
        {
            rights = new List<RightModel>();
            users = new List<UserModel>();
        }

    }
}