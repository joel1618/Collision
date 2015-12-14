﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Sql.Ef.Services.Interfaces;
using Collision.Sql.Ef.Extensions;
using CoreAircraft = Collision.Core.Models.Aircraft;

namespace Collision.Sql.Ef.Services
{
    public class AircraftService : IAircraftService, IDisposable
    {
        private CollisionEntities _context;

        public AircraftService(CollisionEntities context)
        {
            _context = context;
        }

        public IEnumerable<CoreAircraft> Search()
        {
            throw new NotImplementedException();
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

        #region IDisposable
        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    // Dispose other managed resources.
                }
                //release unmanaged resources.
            }
            _disposed = true;
        }
        #endregion
    }
}
