using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Collision.Core.Models;

//http://stackoverflow.com/questions/20693542/breezejs-datamodal-using-ef-as-a-design-tool/20694398
namespace Collision.Data
{
    public class CollisionDbContext : DbContext
    {
        public CollisionDbContext()
            : base("BreezeMetadata")
        {
            // Prevent attempt to initialize a database for this context
            Database.SetInitializer<CollisionDbContext>(null);
        }
        static CollisionDbContext()
        {
            // Prevent attempt to initialize a database for this context
            Database.SetInitializer<CollisionDbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new ConflictDtoConfiguration());
        }

        public DbSet<Collision.Core.Models.Conflict> Conflicts { get; set; }
        //public DbSet<Provider> Providers { get; set; }
    }

    //internal class ConflictDtoConfiguration : EntityTypeConfiguration<Conflict>
    //{
    //    public ConflictDtoConfiguration()
    //    {
    //        //Property(c => c.CompanyName).IsRequired().HasMaxLength(40);
    //    }
    //}
}
