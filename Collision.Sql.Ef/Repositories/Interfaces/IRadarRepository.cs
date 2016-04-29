using Collision.Sql.Ef.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadarFlightEntity = Collision.Sql.Ef.Models.RadarFlight;

namespace Collision.Sql.Ef.Repositories.Interfaces
{
    public interface IRadarRepository
    {
        IQueryable<RadarFlightEntity> BreezeSearch();
    }
}
