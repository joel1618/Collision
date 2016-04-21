using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFAircraft = Collision.Data.Aircraft;
using CoreAircraft = Collision.Core.Models.Aircraft;

namespace Collision.Data.Extensions
{
    public static class AircraftExtensions
    {
        public static CoreAircraft ToCore(this EFAircraft item)
        {
            if (item == null)
            {
                return null;
            }

            return new CoreAircraft()
            {
                Id = item.Id,
                Carrier = item.Carrier,
                CarrierName = item.CarrierName,
                FlightNumber = item.FlightNumber,
                IsActive = item.IsActive
            };
        }
    }
}
