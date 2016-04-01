using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AircraftEntity = Collision.Sql.Ef.Aircraft;
using CoreAircraft = Collision.Core.Models.Aircraft;

namespace Collision.Sql.Ef.Repositories.Interfaces
{
    public interface IAircraftRepository : IRepository<CoreAircraft>
    {
        IEnumerable<CoreAircraft> Search(Expression<Func<AircraftEntity, bool>> predicate, int page, int pageSize);
        IEnumerable<AircraftEntity> BreezeSearch();
    }
}
