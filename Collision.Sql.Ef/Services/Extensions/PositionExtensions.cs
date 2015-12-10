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

                Temp1Latitude = item.Temp1Latitude,
                Temp1Longitude = item.Temp1Longitude,
                Temp1Altitude = item.Temp1Altitude,
                Temp1Speed = item.Temp1Speed,
                Temp1Heading = item.Temp1Heading,
                Temp1UtcTimeStamp = item.Temp1UtcTimeStamp,

                Temp2Latitude = item.Temp2Latitude.HasValue ? item.Temp2Latitude.Value : new Nullable<decimal>(),
                Temp2Longitude = item.Temp2Longitude.HasValue ? item.Temp2Longitude.Value : new Nullable<decimal>(),
                Temp2Altitude = item.Temp2Altitude.HasValue ? item.Temp2Altitude.Value : new Nullable<int>(),
                Temp2Speed = item.Temp2Speed.HasValue ? item.Temp2Speed.Value : new Nullable<int>(),
                Temp2Heading = item.Temp2Heading.HasValue ? item.Temp2Heading.Value : new Nullable<int>(),
                Temp2UtcTimeStamp = item.Temp2UtcTimeStamp,

                X1 = item.X1.HasValue ? item.X1.Value : new Nullable<decimal>(),
                Y1 = item.Y1.HasValue ? item.Y1.Value : new Nullable<decimal>(),
                Z1 = item.Z1.HasValue ? item.Z1.Value : new Nullable<int>(),

                X2 = item.X2.HasValue ? item.X2.Value : new Nullable<decimal>(),
                Y2 = item.Y2.HasValue ? item.Y2.Value : new Nullable<decimal>(),
                Z2 = item.Z2.HasValue ? item.Z2.Value : new Nullable<int>(),

                X3 = item.X3.HasValue ? item.X3.Value : new Nullable<decimal>(),
                Y3 = item.Y3.HasValue ? item.Y3.Value : new Nullable<decimal>(),
                Z3 = item.Z3.HasValue ? item.Z3.Value : new Nullable<int>(),

                X4 = item.X4.HasValue ? item.X4.Value : new Nullable<decimal>(),
                Y4 = item.Y4.HasValue ? item.Y4.Value : new Nullable<decimal>(),
                Z4 = item.Z4.HasValue ? item.Z4.Value : new Nullable<int>(),

                X5 = item.X5.HasValue ? item.X5.Value : new Nullable<decimal>(),
                Y5 = item.Y5.HasValue ? item.Y5.Value : new Nullable<decimal>(),
                Z5 = item.Z5.HasValue ? item.Z5.Value : new Nullable<int>(),

                X6 = item.X6.HasValue ? item.X6.Value : new Nullable<decimal>(),
                Y6 = item.Y6.HasValue ? item.Y6.Value : new Nullable<decimal>(),
                Z6 = item.Z6.HasValue ? item.Z6.Value : new Nullable<int>(),

                X7 = item.X7.HasValue ? item.X7.Value : new Nullable<decimal>(),
                Y7 = item.Y7.HasValue ? item.Y7.Value : new Nullable<decimal>(),
                Z7 = item.Z7.HasValue ? item.Z7.Value : new Nullable<int>(),

                X8 = item.X8.HasValue ? item.X8.Value : new Nullable<decimal>(),
                Y8 = item.Y8.HasValue ? item.Y8.Value : new Nullable<decimal>(),
                Z8 = item.Z8.HasValue ? item.Z8.Value : new Nullable<int>(),

                CreatedAtUtcTimeStamp = item.CreatedAtUtcTimeStamp
            };
        }
    }
}
