using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using Collision.Sql.Ef.Repositories.Interfaces;
using Collision.ViewModels.Extensions;

using Collision.Sql.Ef.Repositories;
using UserSettingCore = Collision.Core.Models.UserSetting;

namespace Collision.Controllers.v1
{
    public class UserSettingsApiController : ApiController
    {
        private readonly IUserSettingsRepository _userSettingsRepository;
        public UserSettingsApiController(IUserSettingsRepository userSettingsRepository)
        {
            _userSettingsRepository = userSettingsRepository;
        }

        public UserSettingCore Get(int id)
        {
            return _userSettingsRepository.Get(id);
        }
        public UserSettingCore Create(UserSettingCore item)
        {
            return _userSettingsRepository.Create(item);
        }
        public UserSettingCore Update(int id, UserSettingCore item)
        {
            return _userSettingsRepository.Update(item.Id, item);
        }
        public void Delete(int id)
        {
            _userSettingsRepository.Delete(id);
        }
    }
}