using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using Collision.Sql.Ef.Services.Interfaces;
using Collision.Sql.Ef.Services;

namespace Collision.v1.API
{
    public class ConflictController : ApiController
    {
        private readonly IConflictService _conflictService;
        public ConflictController(IConflictService conflictService)
        {
            _conflictService = conflictService; 
        }

        //public HttpResponseMessage Search()
        //{
        //    return Request.CreateResponse(HttpStatusCode.NotImplemented, null);
        //}
    }
}