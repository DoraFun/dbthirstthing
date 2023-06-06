using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbthirstthing.Models
{
    [Table("preregistration", Schema = "public")]
    public class PreregistrationModel
    {
        [Key]
        public int userid { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите имя для отображения")]
        public string displayname { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите логин")]
        [Remote("IsLoginValid", "Preregistration", HttpMethod = "POST", ErrorMessage = "Этот логин уже занят.")]
        public string login { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите почту.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Please enter a valid email address.")]
        [Remote("IsEmailValid", "Preregistration", HttpMethod = "POST", ErrorMessage = "Эта почта уже занята.")]
        public string email { get; set; }
        public string aboutuser { get; set; }
    }
}