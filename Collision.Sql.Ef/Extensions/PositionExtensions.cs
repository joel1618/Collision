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
                Id = item.Id
            };
        }
    }
}
