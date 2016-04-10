using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSettingEntity = Collision.Sql.Ef.UserSetting;
using UserSettingCore = Collision.Core.Models.UserSetting;

namespace Collision.Sql.Ef.Repositories.Extensions
{

    public static class UserSettingExtensions
    {
        public static UserSettingCore ToCore(this UserSettingEntity item)
        {
            if (item == null)
            {
                return null;
            }

            return new UserSettingCore()
            {
                Id = item.Id,
                UserId = new Guid(item.UserId),
                Distance = item.Distance
            };
        }

        public static UserSettingEntity ToEntity(this UserSettingCore item)
        {
            if (item == null)
            {
                return null;
            }

            return new UserSettingEntity()
            {
                Id = item.Id,
                UserId = item.UserId.ToString(),
                Distance = item.Distance
            };
        }
    }
}
