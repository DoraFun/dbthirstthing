using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dbthirstthing.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsModel>> GetAllNews();
    }
}
