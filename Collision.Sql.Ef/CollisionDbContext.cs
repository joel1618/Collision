using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

//http://stackoverflow.com/questions/20693542/breezejs-datamodal-using-ef-as-a-design-tool/20694398
namespace Collision.Sql.Ef
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
            //modelBuilder.Configurations.Add(new ProviderDtoConfiguration());
        }

        //public DbSet<Provider> Providers { get; set; }
    }

    //internal class ProviderDtoConfiguration : EntityTypeConfiguration<Provider>
    //{
    //    public ProviderDtoConfiguration()
    //    {
    //        //Property(c => c.CompanyName).IsRequired().HasMaxLength(40);
    //    }
    //}
}
