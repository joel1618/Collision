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

namespace Collision.Controllers.v1.breeze
{
    [BreezeController]
    public class ConflictController
    {
        private readonly IConflictRepository _conflictRepository;
        public ConflictController(IConflictRepository conflictRepository)
        {
            _conflictRepository = conflictRepository;
        }

        [HttpGet]
        public IQueryable<Conflict> Search()
        {
            return _conflictRepository.Search();
        }
    }
}