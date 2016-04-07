using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PositionEntity = Collision.Sql.Ef.Position;
using CorePosition = Collision.Core.Models.Position;
using System.Linq.Expressions;

namespace Collision.Sql.Ef.Repositories.Interfaces
{
    public interface IPositionRepository : IRepository<CorePosition>
    {
        IEnumerable<CorePosition> Search(Expression<Func<PositionEntity, bool>> predicate, int page, int pageSize);
        IQueryable<PositionEntity> BreezeSearch();
        IEnumerable<CorePosition> GetByQuadrant(CorePosition item);
    }
}
