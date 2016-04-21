using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Sql.Ef.Repositories.Interfaces;
using Collision.Sql.Ef.Extensions;
using System.Linq.Expressions;
using AircraftCore = Collision.Core.Models.Aircraft;
using AircraftEntity = Collision.Sql.Ef.Aircraft;

namespace Collision.Sql.Ef.Repositories
{
    public class AircraftRepository : BaseRepository, IAircraftRepository
    {
        public IEnumerable<AircraftCore> Search(Expression<Func<AircraftEntity, bool>> predicate, int page, int pageSize)
        {
            IQueryable<AircraftEntity> records = _context.Aircraft;
            if (predicate != null)
            {
                records = records.Where(predicate);
            }
            return records.OrderBy(e => e.Id).Skip(page * pageSize).Take(pageSize).ToList().Select(x => x.ToCore());
        }
        public IQueryable<AircraftEntity> BreezeSearch()
        {
            return _context.Aircraft;
        }
        public AircraftCore Get(int id)
        {
            return _context.Aircraft.Find(id).ToCore();
        }
        public AircraftCore Create(AircraftCore item)
        {
            throw new NotImplementedException();
        }
        public AircraftCore Update(int id, AircraftCore item)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
