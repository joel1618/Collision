using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreAircraft = Collision.Core.Models.Aircraft;

namespace Collision.Sql.Ef.Services.Interfaces
{
    public interface IAircraftService : IService<CoreAircraft>
    {
        IEnumerable<CoreAircraft> GetAllInActive();
        IEnumerable<CoreAircraft> GetAllActive();
    }
}
