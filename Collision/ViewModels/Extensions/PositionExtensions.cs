using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMPosition = Collision.ViewModels.PositionViewModel;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.ViewModels.Extensions
{
    public static class PositionExtensions
    {
        public static CorePosition ToCore(this VMPosition item)
        {
            if (item == null)
            {
                return null;
            }

            return new CorePosition()
            {
                Latitude2 = item.Latitude2.HasValue ? item.Latitude2.Value : new Nullable<decimal>(),
                Longitude2 = item.Longitude2.HasValue ? item.Longitude2.Value : new Nullable<decimal>(),
                Altitude2 = item.Altitude2.HasValue ? item.Altitude2.Value : new Nullable<decimal>(),
            };
        }
    }
}
