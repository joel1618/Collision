using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Data.Repositories.Interfaces;
using Collision.Data.Extensions;
using CoreAircraft = Collision.Core.Models.Aircraft;

namespace Collision.Data.Repositories
{
    public class AircraftRepository : IAircraftRepository
    {
        private CollisionEntities _context;

        public AircraftRepository(CollisionEntities context)
        {
            _context = context;
        }
        public IEnumerable<CoreAircraft> Search()
        {
            return _context.Aircraft.Select(x => x.ToCore());
        }
        public IEnumerable<CoreAircraft> GetAll()
        {
            return _context.Aircraft.ToList().Select(x => x.ToCore());
        }
        public CoreAircraft Get(int id)
        {
            return _context.Aircraft.Find(id).ToCore();
        }
        public CoreAircraft Create(CoreAircraft item)
        {
            throw new NotImplementedException();
        }
        public CoreAircraft Update(int id, CoreAircraft item)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
