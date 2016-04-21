using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Sql.Ef.Repositories.Interfaces
{
    public interface IRepository<TEntity>
    {
        TEntity Get(int id);
        TEntity Create(TEntity entity);
        TEntity Update(int id, TEntity entity);
        void Delete(int id);
    }
}
