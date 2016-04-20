using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFConflict = Collision.Data.Conflict;
using CoreConflict = Collision.Core.Models.Conflict;

namespace Collision.Data.Extensions
{
    public static class ConflictExtensions
    {
        public static CoreConflict ToCore(this EFConflict item)
        {
            if (item == null)
            {
                return null;
            }

            return new CoreConflict()
            {
                Id = item.Id,
                PositionId1 = item.PositionId1,
                PositionId2 = item.PositionId2,
                Position1 = item.Position.ToCore(),
                Position2 = item.Position1.ToCore(),
                IsActive = item.IsActive
            };
        }
    }
}
