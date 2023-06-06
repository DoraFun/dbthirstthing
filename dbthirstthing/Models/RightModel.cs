using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dbthirstthing.Models
{
    [Table("roles", Schema = "public")]
    public class RightModel
    {
        public int rightid { get; set; }

        [Required]
        public string rightname { get; set; }

        public virtual ICollection<RoleModel> roles { get; set; }

        public RightModel()
        {
            roles = new List<RoleModel>();
        }
    }
}