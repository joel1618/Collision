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
            return _context.Positions.Select(e => new RadarFlightEntity()
            {
                Id = e.Id, AircraftId = e.AircraftId, Carrier = e.Aircraft.Carrier,
                CarrierName = e.Aircraft.CarrierName,
                Latitude2 = (double)e.Latitude2.Value,
                Longitude2 = (double)e.Longitude2.Value,
                Altitude2 = (double)e.Altitude2.Value,
                Speed2 = (double)e.Speed2.Value,
                Heading2 = (double)e.Heading2.Value,
                CreatedAtUtcTimeStamp = e.CreatedAtUtcTimeStamp,
                ModifiedAtUtcTimeStamp = e.ModifiedAtUtcTimeStamp,
                IsConflict = e.Conflicts.Count > 0 ? true : false
            });
            //TODO: Figure out how to execute with the sql resolving only after the where clause is appended via breezejs
            //IEnumerable<RadarFlightEntity> response = null;
            //using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["IdentityConnection"].ToString()))
            //{
            //    const string query =
            //        "select " +
            //        "Position.Id, " +
            //        "AircraftId, " +
            //        "Aircraft.Carrier, " +
            //        "Aircraft.CarrierName, " +
            //        "Aircraft.FlightNumber, " +
            //        "Latitude2, " +
            //        "Longitude2, " +
            //        "Altitude2, " +
            //        "Speed2, " +
            //        "Heading2, " +
            //        "Position.CreatedAtUtcTimeStamp, " +
            //        "Position.ModifiedAtUtcTimeStamp, " +
            //        "case when Conflict.CreatedAtUtcTimeStamp is null then 0 else 1 end as IsConflict " +
            //        "from Position " +
            //        "left join Aircraft " +
            //        "on Position.AircraftId = Aircraft.Id " +
            //        "left " +
            //        "join Conflict " +
            //        "on Position.Id = Conflict.PositionId1 or Position.Id = Conflict.PositionId2";
            //    response = connection.Query<RadarFlightEntity>(query);
            //}
            //return response.AsQueryable();
        }
    }
}
