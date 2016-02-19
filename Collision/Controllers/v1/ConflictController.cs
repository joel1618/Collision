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

        //TODO: Implement BreezeJS for searching 
        //public HttpResponseMessage Search()
        //{
        //    return Request.CreateResponse(HttpStatusCode.NotImplemented, null);
        //}

        [Route("conflicts/getbyquadrant")]
        [HttpGet]
        public HttpResponseMessage GetByQuadrant(PositionViewModel item)
        {
            try {
                var conflicts = _conflictRepository.GetByQuadrant(item.ToCore());
                return Request.CreateResponse(HttpStatusCode.NotImplemented, conflicts/*TODO:.ToViewModel()*/);
            }
            catch(Exception)// ex)
            {
                //_loggingRepository.create(ex);
                return Request.CreateErrorResponse((HttpStatusCode)500, "There was an error retrieving the conflicts");
            }
        }

        
    }
}