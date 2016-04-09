using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Migrations;
using System.Configuration;
using Dapper;

namespace Collision.Sql.Ef.Migrations
{
    public class Configuration : IDatabaseInitializer<CollisionEntities>
    {
        public void InitializeDatabase(CollisionEntities context   )
        {
            using (context = new CollisionEntities())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                    context.Database.Connection.Open();
                    if (bool.Parse(ConfigurationManager.AppSettings["mockData"]))
                    {
                        AddMockAircraft(context);
                    }
                    else
                    {
                        AddRealAircraft(context);
                    }
                    context.SaveChanges();
                    context.Database.Connection.Close();
                }
            }
        }

        public void AddMockAircraft(CollisionEntities context)
        {            
            using(IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["IdentityConnection"].ToString()))
            {
                const string query =
                    "declare @id int " +
                    "select @id = 1 " +
                    "while @id >=1 and @id <= 30000 " +
                    "begin " +
                    "insert into Aircraft (Carrier, CarrierName, FlightNumber, IsActive) values ('AA', 'American Airlines', @id, 'true') " +
                    "select @id = @id + 1 " +
                    "end";
                connection.Query<Aircraft>(query);
            }
                       
        }        

        public void AddRealAircraft(CollisionEntities context)
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
