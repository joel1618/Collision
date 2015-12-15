using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Collision.Sql.Ef.Migrations
{
    public class Configuration : IDatabaseInitializer<CollisionEntities>
    {
        public void InitializeDatabase(CollisionEntities context)
        {
            using (context = new CollisionEntities())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                    context.Database.Connection.Open();
                    //AddAll(context);
                    AddAircraft(context);
                    context.SaveChanges();
                    context.Database.Connection.Close();
                }
            }
        }

        public void AddAll(CollisionEntities context)
        {
            for (int i = 0; i < 10000; i++)
            {
                context.Aircraft.AddOrUpdate(x => x.Id,
                new Aircraft()
                {
                    Carrier = "AA",
                    CarrierName = "American Airlines",
                    FlightNumber = i.ToString(),
                    IsActive = true
                });
            }
        }
        

        public void AddAircraft(CollisionEntities context)
        {
            context.Aircraft.AddOrUpdate(x => x.Id,
                new Aircraft()
                {
                    Carrier = "AA",
                    CarrierName = "American Airlines",
                    FlightNumber = "100",
                    IsActive = true
                });
            context.Aircraft.AddOrUpdate(x => x.Id,
                new Aircraft()
                {
                    Carrier = "AA",
                    CarrierName = "American Airlines",
                    FlightNumber = "1002",
                    IsActive = true
                });
            context.Aircraft.AddOrUpdate(x => x.Id,
                new Aircraft()
                {
                    Carrier = "AA",
                    CarrierName = "American Airlines",
                    FlightNumber = "1003",
                    IsActive = true
                });
        }
    }
}
