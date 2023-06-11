using dbthirstthing.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace dbthirstthing.Models
{
    [Table("news", Schema = "public")]
    public class NewsModel
    {
        [Key]
        public int newsid { get; set; }

        public string header { get; set; }

        public string content { get; set; }
        public DateTime newsdate { get; set; }
    }
}