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
        public virtual DbSet<PreregistrationModel> Preregistration { get; set; }

        public virtual DbSet<RightModel> Rights { get; set; }
        public virtual DbSet<RoleModel> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleModel>().HasMany(c => c.rights)
                .WithMany(s => s.roles)
                .Map(t => t.MapLeftKey("roleid")
                .MapRightKey("rightid")
                .ToTable("rolerights"));

            modelBuilder.Entity<RoleModel>().HasMany(c => c.users)
            .WithMany(s => s.roles)
            .Map(t => t.MapLeftKey("roleid")
            .MapRightKey("userid")
            .ToTable("userroles"));
        }
    }
}