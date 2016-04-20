using Collision.Data.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Collision.Core.Models.FlightStats.Flight;
using PositionDomain = Collision.Core.Models.Position;

namespace Collision.Data.Repositories
{
    public class FlightStatsRepository : IFlightStatsRepository
    {
        private string baseUrl = "https://api.flightstats.com/flex/flightstatus/rest/v2/json/flight/tracks/";
        private string endUrl = "?appId=" + ConfigurationManager.AppSettings["appId"] + "&appKey=" + ConfigurationManager.AppSettings["appKey"] + "&utc=true&includeFlightPlan=false&maxPositions=2";
        
        public FlightStatsRepository()
        {

        }

        public FlightDomain Get(PositionDomain item)
        {
            using (var syncClient = new WebClient())
            {
                return (FlightDomain)JsonConvert.DeserializeObject(syncClient.DownloadString(GetUrl(item)));
            }
        }

        private string GetUrl(PositionDomain item)
        {
            return baseUrl +
                item.Aircraft.Carrier +
                "/" + item.Aircraft.FlightNumber +
                "/dep/" + DateTime.Now.Year +
                "/" + DateTime.Now.Month +
                "/" + DateTime.Now.Day +
                endUrl;
        }
    }
}
