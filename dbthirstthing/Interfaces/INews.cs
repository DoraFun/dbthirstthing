using dbthirstthing.DataContext;
using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbthirstthing.Interfaces
{
    internal interface INews
    {
        Task<IEnumerable<NewsModel>> GetNewsAsync(int page = 1, int pageSize = 10);
    }
}
