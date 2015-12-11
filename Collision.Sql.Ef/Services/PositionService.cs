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
            return _context.Position.ToList().Select(x => x.ToCore());          
        }

        public CorePosition Get(int id)
        {
            return _context.Position.Find(id).ToCore();
        }

        public CorePosition GetByAircraftId(int id)
        {
            return _context.Position.Where(x => x.AircraftId == id).FirstOrDefault().ToCore();
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

                Temp1Latitude = item.Temp1Latitude,
                Temp1Longitude = item.Temp1Longitude,
                Temp1Altitude = item.Temp1Altitude,
                Temp1Speed = item.Temp1Speed,
                Temp1Heading = item.Temp1Heading,
                Temp1UtcTimeStamp = item.Temp1UtcTimeStamp,

                Temp2Latitude = item.Temp2Latitude,
                Temp2Longitude = item.Temp2Longitude,
                Temp2Altitude = item.Temp2Altitude,
                Temp2Speed = item.Temp2Speed,
                Temp2Heading = item.Temp2Heading,
                Temp2UtcTimeStamp = item.Temp2UtcTimeStamp,

                CreatedAtUtcTimeStamp = now,
                IsInFlight = item.IsInFlight,
                IsActive = item.IsActive
            };

            _context.Position.Add(_item);
            _context.SaveChanges();
            return _item.ToCore();
        }

        public CorePosition Update(int id, CorePosition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Core.Models.Position");
            }

            var record = _context.Position.FirstOrDefault(x => x.Id == id);
            if (record == null)
            {
                throw new ArgumentNullException("Cannot find position.");
            }

            var now = DateTime.UtcNow;

            record.Name = item.Name;
            record.Temp1Latitude = item.Temp1Latitude;
            record.Temp1Longitude = item.Temp1Longitude;
            record.Temp1Altitude = item.Temp1Altitude;
            record.Temp1Speed = item.Temp1Speed;
            record.Temp1Heading = item.Temp1Heading;
            record.Temp1UtcTimeStamp = item.Temp1UtcTimeStamp;

            record.Temp2Latitude = item.Temp2Latitude;
            record.Temp2Longitude = item.Temp2Longitude;
            record.Temp2Altitude = item.Temp2Altitude;
            record.Temp2Speed = item.Temp2Speed;
            record.Temp2Heading = item.Temp2Heading;
            record.Temp2UtcTimeStamp = item.Temp2UtcTimeStamp;

            record.X1 = item.X1; record.Y1 = item.Y1; record.Z1 = item.Z1;
            record.X2 = item.X2; record.Y2 = item.Y2; record.Z2 = item.Z2;
            record.X3 = item.X3; record.Y3 = item.Y3; record.Z3 = item.Z3;
            record.X4 = item.X4; record.Y4 = item.Y4; record.Z4 = item.Z4;
            record.X5 = item.X5; record.Y5 = item.Y5; record.Z5 = item.Z5;
            record.X6 = item.X6; record.Y6 = item.Y6; record.Z6 = item.Z6;
            record.X7 = item.X7; record.Y7 = item.Y7; record.Z7 = item.Z7;
            record.X8 = item.X8; record.Y8 = item.Y8; record.Z8 = item.Z8;

            record.IsInFlight = item.IsInFlight;
            record.IsActive = item.IsActive;

            record.ModifiedAtUtcTimeStamp = DateTime.UtcNow;

            _context.SaveChanges();

            return record.ToCore();
        }

        public void Delete(int id)
        {
            var position = new EfPosition { Id = id };
            _context.Position.Attach(position);
            _context.Position.Remove(position);
            _context.SaveChanges();
        }
    }
}
