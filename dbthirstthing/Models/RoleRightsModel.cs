using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dbthirstthing.Models
{
    [Table("rolerights", Schema = "public")]
    public class RoleRightsModel
    {
        [Key]
        [Column(Order = 1)]
        public int roleid { get; set; }
        [Key]
        [Column(Order = 2)]
        public int rightid { get; set; }

        public virtual RoleModel role { get; set; }
        public virtual RightModel right { get; set; }
    }
}