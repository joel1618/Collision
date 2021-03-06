﻿using System;
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
using ConflictEntity = Collision.Sql.Ef.Conflict;
using ConflictCore = Collision.Core.Models.Conflict;

namespace Collision.Controllers.v1.breeze
{
    [BreezeController]
    public class ConflictBreezeApiController : ApiController
    {
        private readonly IConflictRepository _conflictRepository;
        public ConflictBreezeApiController(IConflictRepository conflictRepository)
        {
            _conflictRepository = conflictRepository;
        }

        [HttpGet]
        public IEnumerable<ConflictEntity> Search()
        {
            return _conflictRepository.BreezeSearch();
        }
    }
}