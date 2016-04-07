using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Sql.Ef.Extensions;
using Collision.Sql.Ef.Repositories.Interfaces;
using ConflictEntity = Collision.Sql.Ef.Conflict;
using CoreConflict = Collision.Core.Models.Conflict;
using CorePosition = Collision.Core.Models.Position;
using System.Linq.Expressions;

namespace Collision.Sql.Ef.Repositories
{
    public class ConflictRepository : IConflictRepository
    {
        private CollisionEntities _context;

        public ConflictRepository(CollisionEntities context)
        {
            _context = context;
        }
        public IEnumerable<CoreConflict> Search(Expression<Func<ConflictEntity, bool>> predicate, int page, int pageSize)
        {
            IQueryable<ConflictEntity> records = _context.Conflicts;
            if (predicate != null)
            {
                records = records.Where(predicate);
            }
            return records.OrderBy(e => e.Id).Skip(page * pageSize).Take(pageSize).ToList().Select(x => x.ToCore());
        }
        public IQueryable<ConflictEntity> BreezeSearch()
        {
            return _context.Conflicts;
        }
        public CoreConflict Get(int id)
        {
            return _context.Conflicts.Find(id).ToCore();
        }

        public CoreConflict Create(CoreConflict item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Core.Models.Position");
            }
            var now = DateTime.UtcNow;
            var _item = new ConflictEntity()
            {
                PositionId1 = item.PositionId1,
                PositionId2 = item.PositionId2,
                CreatedAtUtcTimeStamp = DateTime.UtcNow,
                IsActive = true
            };

            _context.Conflicts.Add(_item);
            _context.SaveChanges();

            _context.Entry(_item).Reference(x => x.Position).Load();
            _context.Entry(_item).Reference(x => x.Position1).Load();
            return _item.ToCore();
        }

        public CoreConflict Update(int id, CoreConflict item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Core.Models.Position");
            }

            var record = _context.Conflicts.FirstOrDefault(x => x.Id == id);
            if (record == null)
            {
                throw new ArgumentNullException("Cannot find position.");
            }

            record.IsActive = item.IsActive;

            record.ModifiedAtUtcTimeStamp = DateTime.UtcNow;

            _context.SaveChanges();

            _context.Entry(record).Reference(x => x.Position).Load();
            _context.Entry(record).Reference(x => x.Position1).Load();
            return record.ToCore();
        }

        public void Delete(int id)
        {
            var conflict = _context.Conflicts.Find(id);
            _context.Conflicts.Attach(conflict);
            _context.Conflicts.Remove(conflict);
            _context.SaveChanges();
        }
    }
}
