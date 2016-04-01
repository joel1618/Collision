using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ConflictEntity = Collision.Sql.Ef.Conflict;
using CoreConflict = Collision.Core.Models.Conflict;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.Sql.Ef.Repositories.Interfaces
{
    public interface IConflictRepository : IRepository<CoreConflict>
    {
        IEnumerable<CoreConflict> Search(Expression<Func<ConflictEntity, bool>> predicate, int page, int pageSize);
        IEnumerable<ConflictEntity> BreezeSearch();
    }
}
