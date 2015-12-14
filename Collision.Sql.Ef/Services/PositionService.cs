using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Sql.Ef.Extensions;
using Collision.Sql.Ef.Services.Interfaces;
using EfPosition = Collision.Sql.Ef.Position;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.Sql.Ef.Services
{
    public class PositionService : IPositionService
    {
        private CollisionEntities _context;

        public PositionService(CollisionEntities context)
        {
            _context = context;
        }

        public IEnumerable<CorePosition> Search()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<CorePosition> GetAll()
        {
            return _context.Positions.ToList().Select(x => x.ToCore());          
        }

        public CorePosition Get(int id)
        {
            return _context.Positions.Find(id).ToCore();
        }

        public CorePosition GetByAircraftId(int id)
        {
            return _context.Positions.Where(x => x.AircraftId == id).FirstOrDefault().ToCore();
        }

        public CorePosition Create(CorePosition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Core.Models.Position");
            }
            var now = DateTime.UtcNow;
            var _item = new EfPosition()
            {
                Name = item.Name,
                AircraftId = item.AircraftId,

                Latitude1 = item.Latitude1,
                Longitude1 = item.Longitude1,
                Altitude1 = item.Altitude1,
                Speed1 = item.Speed1,
                Heading1 = item.Heading1,
                UtcTimeStamp1 = item.UtcTimeStamp1,

                Latitude2 = item.Latitude2,
                Longitude2 = item.Longitude2,
                Altitude2 = item.Altitude2,
                Speed2 = item.Speed2,
                Heading2 = item.Heading2,
                UtcTimeStamp2 = item.UtcTimeStamp2,

                Latitude3 = item.Latitude3,
                Longitude3 = item.Longitude3,
                Altitude3 = item.Altitude3,
                Speed3 = item.Speed3,
                Heading3 = item.Heading3,
                UtcTimeStamp3 = item.UtcTimeStamp3,

                Radius = item.Radius,

                CreatedAtUtcTimeStamp = now,
                IsInFlight = item.IsInFlight,
                IsActive = item.IsActive
            };

            _context.Positions.Add(_item);
            _context.SaveChanges();
            return _item.ToCore();
        }

        public CorePosition Update(int id, CorePosition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Core.Models.Position");
            }

            var record = _context.Positions.FirstOrDefault(x => x.Id == id);
            if (record == null)
            {
                throw new ArgumentNullException("Cannot find position.");
            }

            record.Name = item.Name;

            record.Latitude1 = item.Latitude1;
            record.Longitude1 = item.Longitude1;
            record.Altitude1 = item.Altitude1;
            record.Speed1 = item.Speed1;
            record.Heading1 = item.Heading1;
            record.UtcTimeStamp1 = item.UtcTimeStamp1;
            
            record.Latitude2 = item.Latitude2;
            record.Longitude2 = item.Longitude2;
            record.Altitude2 = item.Altitude2;
            record.Speed2 = item.Speed2;
            record.Heading2 = item.Heading2;
            record.UtcTimeStamp2 = item.UtcTimeStamp2;
            
            record.Latitude3 = item.Latitude3;
            record.Longitude3 = item.Longitude3;
            record.Altitude3 = item.Altitude3;
            record.Speed3 = item.Speed3;
            record.Heading3 = item.Heading3;
            record.UtcTimeStamp3 = item.UtcTimeStamp3;

            record.Radius = item.Radius;

            record.X1 = item.X1; record.Y1 = item.Y1; record.Z1 = item.Z1;
            record.X2 = item.X2; record.Y2 = item.Y2; record.Z2 = item.Z2;
            record.X3 = item.X3; record.Y3 = item.Y3; record.Z3 = item.Z3;

            record.IsInFlight = item.IsInFlight;
            record.IsActive = item.IsActive;

            record.ModifiedAtUtcTimeStamp = DateTime.UtcNow;

            _context.SaveChanges();

            return record.ToCore();
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
