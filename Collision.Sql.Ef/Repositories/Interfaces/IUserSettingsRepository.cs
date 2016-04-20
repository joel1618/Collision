using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettingEntity = Collision.Sql.Ef.UserSetting;
using UserSettingCore = Collision.Core.Models.UserSetting;
using System.Linq.Expressions;

namespace Collision.Sql.Ef.Repositories.Interfaces
{
    public interface IUserSettingsRepository : IRepository<UserSettingCore>
    {
        IEnumerable<UserSettingCore> Search(Expression<Func<UserSettingEntity, bool>> predicate, int page, int pageSize);
        IQueryable<UserSettingEntity> BreezeSearch();
    }
}
