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
using PositionViewModel = Collision.ViewModels.PositionViewModel;
namespace Collision.Controllers.v1
{
    public class ConflictController : ApiController
    {
        private readonly IConflictRepository _conflictRepository;
        public ConflictController(IConflictRepository conflictRepository)
        {
            _conflictRepository = conflictRepository; 
        }        
    }
}