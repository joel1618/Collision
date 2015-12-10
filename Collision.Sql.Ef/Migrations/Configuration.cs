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
                    context.Aircraft.AddOrUpdate(x => x.Id,
                        new Aircraft()
                        {
                            Carrier = "AA",
                            CarrierName = "Amercan Airlines",
                            FlightNumber = "100"
                        });
                    context.SaveChanges();
                    context.Database.Connection.Close();
                }
            }
        }
    }
}
