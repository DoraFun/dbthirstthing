using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace dbthirstthing.DataContext
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext():base(nameOrConnectionString:"Myconnection") 
        { 
        
        }

        public virtual DbSet<UserModel> Users { get; set; }
    }
}