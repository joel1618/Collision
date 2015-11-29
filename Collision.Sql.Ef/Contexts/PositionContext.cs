using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Collision.Sql.Ef.Contexts
{
    public class PositionContext : DbContext
    {
        public PositionContext() : base("name=CollisionEntities")
        {
        }
        public DbSet<Position> Positions { get; set; }
    }
}
