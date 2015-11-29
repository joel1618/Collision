using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.Sql.Ef.Interfaces
{
    public interface IPositionRepository
    {
        CorePosition Get(int id);
        CorePosition Create(CorePosition item);
        CorePosition Update(CorePosition item);
        void Delete(int id);
    }
}
