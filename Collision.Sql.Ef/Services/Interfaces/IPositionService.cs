using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.Sql.Ef.Services.Interfaces
{
    public interface IPositionService : IService<CorePosition>
    {
        CorePosition GetByAircraftId(int id);
    }
}
