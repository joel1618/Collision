using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using Collision.Sql.Ef.Repositories.Interfaces;
using Collision.Sql.Ef.Repositories;

namespace Collision.v1.API
{
    public class ConflictController : ApiController
    {
        private readonly IConflictRepository _conflictRepository;
        public ConflictController(IConflictRepository conflictRepository)
        {
            _conflictRepository = conflictRepository; 
        }

        //public HttpResponseMessage Search()
        //{
        //    return Request.CreateResponse(HttpStatusCode.NotImplemented, null);
        //}
    }
}