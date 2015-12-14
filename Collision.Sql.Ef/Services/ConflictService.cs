﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Sql.Ef.Extensions;
using Collision.Sql.Ef.Services.Interfaces;
using EfConflict = Collision.Sql.Ef.Conflict;
using CoreConflict = Collision.Core.Models.Conflict;

namespace Collision.Sql.Ef.Services
{
    public class ConflictService : IConflictService
    {
        private CollisionEntities _context;

        public ConflictService(CollisionEntities context)
        {
            _context = context;
        }

        public IEnumerable<CoreConflict> Search()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<CoreConflict> GetAll()
        {
            return _context.Conflicts.ToList().Select(x => x.ToCore());
        }

        public IEnumerable<CoreConflict> GetByPositionId1(int positionId1)
        {
            return _context.Conflicts.ToList().Where(e => e.PositionId1 == positionId1).Select(x => x.ToCore());
        }

        public CoreConflict GetByPositionId1AndPositionId2(int positionId1, int positionId2)
        {
            return _context.Conflicts.ToList().Where(e => e.PositionId1 == positionId1 && e.PositionId2 == positionId2).FirstOrDefault().ToCore();
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
            var _item = new EfConflict()
            {
                PositionId1 = item.PositionId1,
                PositionId2 = item.PositionId2,
                CreatedAtUtcTimeStamp = DateTime.UtcNow,
                IsActive = true
            };

            _context.Conflicts.Add(_item);
            _context.SaveChanges();
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

            return record.ToCore();
        }

        public void Delete(int id)
        {
            var conflict = new EfConflict { Id = id };
            _context.Conflicts.Attach(conflict);
            _context.Conflicts.Remove(conflict);
            _context.SaveChanges();
        }
    }
}