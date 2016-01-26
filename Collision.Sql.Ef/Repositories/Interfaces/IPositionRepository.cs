using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.Sql.Ef.Repositories.Interfaces
{
    public interface IPositionRepository : IRepository<CorePosition>
    {
        CorePosition GetByAircraftId(int id);
        IEnumerable<CorePosition> GetByQuadrant(CorePosition item);
    }
}
