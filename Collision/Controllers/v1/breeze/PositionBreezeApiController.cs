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
using PositionEntity = Collision.Sql.Ef.Position;

namespace Collision.Controllers.v1.breeze
{
    [BreezeController]
    public class PositionBreezeApiController : ApiController
    {
        private readonly IPositionRepository _positionRepository;
        public PositionBreezeApiController(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        [HttpGet]
        public IEnumerable<PositionEntity> Search()
        {
            return _positionRepository.BreezeSearch();
        }
    }
}