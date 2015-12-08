using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Core.Models;
using Collision.Sql.Ef.Interfaces;
using Collision.Sql.Ef.Extensions;
using EfPosition = Collision.Sql.Ef.Position;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.Sql.Ef.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private CollisionEntities _context;

        public PositionRepository(CollisionEntities context)
        {
            _context = context;
        }

        public IEnumerable<CorePosition> Search()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<CorePosition> GetAll()
        {
            throw new NotImplementedException();
        }

        public CorePosition Get(int id)
        {
            var item = _context.Positions.Find(id);
            return item.ToCore();
        }

        public CorePosition Create(CorePosition item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("Core.Models.Position");
            }
            var now = DateTime.UtcNow;
            var _item = new EfPosition()
            {
                Name = item.Name,
                Temp1Latitude = (decimal)item.Temp1Latitude,
                Temp1Longitude = (decimal)item.Temp1Longitude,
                Temp1Altitude = item.Temp1Altitude,
                Temp1Speed = item.Temp1Speed,
                Temp1Heading = item.Temp1Heading,
                Temp1EpochTimeStamp = (long)(now - new DateTime(1970, 1, 1)).TotalMilliseconds,
                Temp1TimeStamp = now,
                IsInFlight = item.IsInFlight
            };

            _context.Positions.Add(_item);
            _context.SaveChanges();
            return _item.ToCore();
        }

        public CorePosition Update(int id, CorePosition item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var position = new EfPosition { Id = id };
            _context.Positions.Attach(position);
            _context.Positions.Remove(position);
            _context.SaveChanges();
        }

    }
}
