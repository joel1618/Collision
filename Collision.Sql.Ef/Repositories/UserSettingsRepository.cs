using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Sql.Ef.Repositories.Interfaces;
using System.Linq.Expressions;
using Collision.Sql.Ef.Repositories.Extensions;
using UserSettingEntity = Collision.Sql.Ef.UserSetting;
using UserSettingCore = Collision.Core.Models.UserSetting;

namespace Collision.Sql.Ef.Repositories
{
    public class UserSettingsRepository : BaseRepository, IUserSettingsRepository
    {
        public IEnumerable<UserSettingCore> Search(Expression<Func<UserSettingEntity, bool>> predicate, int page, int pageSize)
        {
            IQueryable<UserSettingEntity> records = _context.UserSettings;
            if (predicate != null)
            {
                records = records.Where(predicate);
            }
            return records.OrderBy(e => e.Id).Skip(page * pageSize).Take(pageSize).ToList().Select(x => x.ToCore());
        }
        public IQueryable<UserSettingEntity> BreezeSearch()
        {
            return _context.UserSettings;
        }
        public UserSettingCore Get(int id)
        {
            return _context.UserSettings.Find(id).ToCore();
        }
        public UserSettingCore Create(UserSettingCore item)
        {
            throw new NotImplementedException();
        }
        public UserSettingCore Update(int id, UserSettingCore item)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
