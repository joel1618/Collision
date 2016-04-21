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
using Collision.Sql.Ef;
using Breeze.ContextProvider.EF6;
using Breeze.WebApi2;

namespace Collision.Controllers.v1.breeze
{
    [BreezeController]
    public class MetadataController : ApiController
    {
        // ~/breeze/Metadata
        [HttpGet]
        public string MetadataEdmx()
        {
            var meta = new EFContextProvider<CollisionEntities>().Metadata();
            return meta;
        }

        [HttpGet]
        public string MetadataDbContext()
        {
            var meta = new EFContextProvider<CollisionDbContext>().Metadata();
            return meta;
        }
    }
}