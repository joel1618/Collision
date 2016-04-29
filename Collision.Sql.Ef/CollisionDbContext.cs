using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Collision.Core.Models;
using Collision.Sql;

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
            //modelBuilder.Configurations.Add(new ConflictDtoConfiguration());
        }

        public DbSet<Collision.Sql.Ef.Conflict> Conflicts { get; set; }
        public DbSet<Collision.Sql.Ef.Position> Positions { get; set; }
        public DbSet<Collision.Sql.Ef.UserSetting> UserSettings { get; set; }
        public DbSet<Collision.Sql.Ef.Models.RadarFlight> RadarFlights { get; set; }
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
