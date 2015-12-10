using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Sql.Ef.Services.Interfaces
{
    public interface IService<TEntity>
    {
        IEnumerable<TEntity> Search();
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        TEntity Create(TEntity entity);
        TEntity Update(int id, TEntity entity);
        void Delete(int id);
    }
}
