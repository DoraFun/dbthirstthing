using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dbthirstthing.Models
{
    [Table("rights", Schema = "public")]
    public class RightModel
    {
        [Key]
        public int rightid { get; set; }

        [Required]
        public string rightname { get; set; }

        public ICollection<RoleModel> roles { get; set; }


    }
}