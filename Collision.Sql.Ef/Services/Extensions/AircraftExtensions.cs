using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFAircraft = Collision.Sql.Ef.Aircraft;
using CoreAircraft = Collision.Core.Models.Aircraft;

namespace Collision.Sql.Ef.Extensions
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
                Id = item.Id
            };
        }
    }
}
