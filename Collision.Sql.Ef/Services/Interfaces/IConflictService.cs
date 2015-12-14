using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreConflict = Collision.Core.Models.Conflict;

namespace Collision.Sql.Ef.Services.Interfaces
{
    public interface IConflictService : IService<CoreConflict>
    {
        IEnumerable<CoreConflict> GetByPositionId1(int positionId1);
        CoreConflict GetByPositionId1AndPositionId2(int positionId1, int positionId2);
    }
}
