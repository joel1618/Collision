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
using RadarFlightEntity = Collision.Sql.Ef.Models.RadarFlight;

namespace Collision.Controllers.v1.breeze
{
    [BreezeController]
    public class RadarBreezeApiController : ApiController
    {
        private readonly IRadarRepository radarRepository;
        public RadarBreezeApiController(IRadarRepository radarRepository)
        {
            this.radarRepository = radarRepository;
        }

        [HttpGet]
        public IQueryable<RadarFlightEntity> Search()
        {
            return radarRepository.BreezeSearch();
        }
    }
}