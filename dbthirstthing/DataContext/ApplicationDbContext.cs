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

        public virtual DbSet<RoleRightsModel> RoleRights { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleModel>()
                .HasMany(r => r.rights)
                .WithMany(r => r.roles)
                .Map(m =>
                {
                    m.ToTable("rolerights");
                    m.MapLeftKey("roleid");
                    m.MapRightKey("rightid");
                });
        }
    }
}