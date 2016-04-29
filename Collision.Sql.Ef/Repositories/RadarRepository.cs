using Collision.Sql.Ef.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Dapper;
using RadarFlightEntity = Collision.Sql.Ef.Models.RadarFlight;
using System.Data;
using System.Configuration;

namespace Collision.Sql.Ef.Repositories
{
    public class RadarRepository : BaseRepository, IRadarRepository
    {
        public IQueryable<RadarFlightEntity> BreezeSearch()
        {
            // return _context.UserSettings;
            IEnumerable<RadarFlightEntity> response = null;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["IdentityConnection"].ToString()))
            {
                const string query =
                    "select " +
                    "Position.Id, " +
                    "AircraftId, " +
                    "Aircraft.Carrier, " +
                    "Aircraft.CarrierName, " +
                    "Aircraft.FlightNumber, " +
                    "Latitude2, " +
                    "Longitude2, " +
                    "Altitude2, " +
                    "Speed2, " +
                    "Heading2, " +
                    "Position.CreatedAtUtcTimeStamp, " +
                    "Position.ModifiedAtUtcTimeStamp, " +
                    "case Conflict.CreatedAtUtcTimeStamp when null then 0 else 1 end as IsConflict " +
                    "from Position " +
                    "left join Aircraft " +
                    "on Position.AircraftId = Aircraft.Id " +
                    "left " +
                    "join Conflict " +
                    "on Position.Id = Conflict.PositionId1 or Position.Id = Conflict.PositionId2";
                response = connection.Query<RadarFlightEntity>(query);
            }
            return response.AsQueryable();
        }
    }
}
