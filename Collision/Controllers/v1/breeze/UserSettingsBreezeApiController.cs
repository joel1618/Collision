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
using Collision.Core.Models;
using Breeze.ContextProvider.EF6;
using Breeze.WebApi2;
using UserSettingEntity = Collision.Sql.Ef.UserSetting;

namespace Collision.Controllers.v1.breeze
{
    [BreezeController]
    public class UserSettingsBreezeApiController : ApiController
    {
        private readonly IUserSettingsRepository _userSettingsRepository;
        public UserSettingsBreezeApiController(IUserSettingsRepository userSettingsRepository)
        {
            _userSettingsRepository = userSettingsRepository;
        }

        [HttpGet]
        public IQueryable<UserSettingEntity> Search()
        {
            return _userSettingsRepository.BreezeSearch();
        }
    }
}