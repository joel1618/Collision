using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFPosition = Collision.Sql.Ef.Position;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.Sql.Ef.Extensions
{
    public static class PositionExtensions
    {
        public static CorePosition ToCore(this EFPosition item)
        {
            if (item == null)
            {
                return null;
            }

            return new CorePosition()
            {
                Id = item.Id,
                AircraftId = item.AircraftId,
                Name = item.Name,
                FlightId = item.FlightId,
                FlightIdentity = item.FlightIdentity,

                Latitude1 = item.Latitude1.HasValue ? item.Latitude1.Value : new Nullable<decimal>(),
                Longitude1 = item.Longitude1.HasValue ? item.Longitude1.Value : new Nullable<decimal>(),
                Altitude1 = item.Altitude1.HasValue ? item.Altitude1.Value : new Nullable<decimal>(),
                Speed1 = item.Speed1.HasValue ? item.Speed1.Value : new Nullable<decimal>(),
                Heading1 = item.Heading1.HasValue ? item.Heading1.Value : new Nullable<decimal>(),
                UtcTimeStamp1 = item.UtcTimeStamp1,

                Latitude2 = item.Latitude2.HasValue ? item.Latitude2.Value : new Nullable<decimal>(),
                Longitude2 = item.Longitude2.HasValue ? item.Longitude2.Value : new Nullable<decimal>(),
                Altitude2 = item.Altitude2.HasValue ? item.Altitude2.Value : new Nullable<decimal>(),
                Speed2 = item.Speed2.HasValue ? item.Speed2.Value : new Nullable<decimal>(),
                Heading2 = item.Heading2.HasValue ? item.Heading2.Value : new Nullable<decimal>(),
                UtcTimeStamp2 = item.UtcTimeStamp2,

                Latitude3 = item.Latitude3.HasValue ? item.Latitude3.Value : new Nullable<decimal>(),
                Longitude3 = item.Longitude3.HasValue ? item.Longitude3.Value : new Nullable<decimal>(),
                Altitude3 = item.Altitude3.HasValue ? item.Altitude3.Value : new Nullable<decimal>(),
                Speed3 = item.Speed3.HasValue ? item.Speed3.Value : new Nullable<decimal>(),
                Heading3 = item.Heading3.HasValue ? item.Heading3.Value : new Nullable<decimal>(),
                UtcTimeStamp3 = item.UtcTimeStamp3,

                X1 = item.X1.HasValue ? item.X1.Value : new Nullable<decimal>(),
                Y1 = item.Y1.HasValue ? item.Y1.Value : new Nullable<decimal>(),
                Z1 = item.Z1.HasValue ? item.Z1.Value : new Nullable<decimal>(),

                X2 = item.X2.HasValue ? item.X2.Value : new Nullable<decimal>(),
                Y2 = item.Y2.HasValue ? item.Y2.Value : new Nullable<decimal>(),
                Z2 = item.Z2.HasValue ? item.Z2.Value : new Nullable<decimal>(),

                X3 = item.X3.HasValue ? item.X3.Value : new Nullable<decimal>(),
                Y3 = item.Y3.HasValue ? item.Y3.Value : new Nullable<decimal>(),
                Z3 = item.Z3.HasValue ? item.Z3.Value : new Nullable<decimal>(),
                
                Radius = item.Radius.HasValue ? item.Radius.Value : new Nullable<decimal>(),              

                CreatedAtUtcTimeStamp = item.CreatedAtUtcTimeStamp,

                Aircraft = item.Aircraft.ToCore()
            };
        }
    }
}
