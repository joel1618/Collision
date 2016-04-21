using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using Collision.Sql.Ef.Repositories.Interfaces;
using Collision.ViewModels.Extensions;
using Microsoft.AspNet.Identity;
using UserCore = Collision.Core.Models.User;

namespace Collision.Controllers.v1
{
    public class UserApiController : ApiController
    {
        private readonly IUserSettingsRepository _userRepository;
        public UserApiController(IUserSettingsRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserCore GetCurrent()
        {
            return new UserCore()
            {
                Id = User.Identity.GetUserId(),
                Username = User.Identity.GetUserName()
            };
        }
    }
}