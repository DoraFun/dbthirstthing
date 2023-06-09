using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static dbthirstthing.Models.NewsViewModel;

namespace dbthirstthing.Models
{
    public class IndexViewModel
    {
        public IEnumerable<NewsModel> News { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}