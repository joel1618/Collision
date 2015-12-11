using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using Collision.Sql.Ef.Services.Interfaces;
using Collision.Sql.Ef.Services;
using Collision.Core.Models;
using Newtonsoft.Json;

namespace Collision.Console
{
    public class HandlePosition
    {
        private readonly IPositionService _positionService;
        private readonly IAircraftService _aircraftService;
        private Dictionary<int, Task> handleCollision = new Dictionary<int, Task>();

        public HandlePosition(IPositionService positionService, IAircraftService aircraftService)
        {
            _positionService = positionService;
            _aircraftService = aircraftService;
        }
        public void HandlePositions(Aircraft aircraft)
        {
            var _position = _positionService.GetByAircraftId(aircraft.Id);
            if (_position == null)
            {
                //No position yet exists for this aircraft and we need to create a new one
                _position = new Position();
                _position.AircraftId = aircraft.Id;
                //Call api for flight
                if (UpdateFlightInformation(aircraft, _position))
                {
                    //Create position in database
                    _position = _positionService.Create(_position);
                    //Calculate bounding box
                    CalculateBoundingBox(_position);
                    //Update position object in database
                    _position = _positionService.Update(_position.Id, _position);
                    //Call HandleCollisions to start evaluating this position for potential collisions
                    if (!handleCollision.ContainsKey(aircraft.Id))
                    {
                        handleCollision.Add(aircraft.Id, Task.Factory.StartNew(() => new HandleCollision(
                        new PositionService(new Sql.Ef.CollisionEntities())).HandleCollisions(_position.Id)));
                    }
                }
            }
            else
            {
                //We found a position and need to update position from api and recalculate boundingbox
                if (UpdateFlightInformation(aircraft, _position))
                {
                    //Call HandleCollisions to start evaluating this position for potential collisions
                    CalculateBoundingBox(_position);
                    //Update position object in database
                    _position = _positionService.Update(_position.Id, _position);
                    //Call HandleCollisions to start evaluating this position for potential collisions
                    if (!handleCollision.ContainsKey(aircraft.Id))
                    {
                        handleCollision.Add(aircraft.Id, Task.Factory.StartNew(() => new HandleCollision(
                        new PositionService(new Sql.Ef.CollisionEntities())).HandleCollisions(_position.Id)));
                    }
                }
            }
            //Wait 30 seconds before evaluating this flight again.
            Thread.Sleep(30000);
            HandlePositions(aircraft);
        }

        public bool UpdateFlightInformation(Aircraft aircraft, Position position)
        {
            var baseUrl = "https://api.flightstats.com/flex/flightstatus/rest/v2/json/flight/tracks/";
            //TODO: Use Stringbuilder here.
            var url =
                baseUrl +
                aircraft.Carrier +
                "/" + aircraft.FlightNumber +
                "/dep/" + DateTime.Now.Year +
                "/" + DateTime.Now.Month +
                "/" + DateTime.Now.Day +
                "?appId=" + ConfigurationManager.AppSettings["appId"] +
                "&appKey=" + ConfigurationManager.AppSettings["appKey"] +
                "&utc=true&includeFlightPlan=false&maxPositions=2";
            var syncClient = new WebClient();
            dynamic flight = JsonConvert.DeserializeObject(syncClient.DownloadString(url));
            if (flight.error != null)
            {
                if (position.Id != 0)
                {
                    _positionService.Delete(position.Id);
                }
                return false;
            }

            if (flight.flightTracks.Count != 0 && flight.flightTracks[0].positions.Count == 2)
            {
                position.Temp1Latitude = flight.flightTracks[0].positions[0].lat;
                position.Temp1Longitude = flight.flightTracks[0].positions[0].lon;
                position.Temp1Speed = flight.flightTracks[0].positions[0].speedMph * 1.60934;
                position.Temp1Altitude = flight.flightTracks[0].positions[0].altitudeFt * 0.3048;
                position.Temp1Heading = flight.flightTracks[0].heading;
                position.Temp1UtcTimeStamp = flight.flightTracks[0].positions[0].date;

                position.Temp2Latitude = flight.flightTracks[0].positions[1].lat;
                position.Temp2Longitude = flight.flightTracks[0].positions[1].lon;
                position.Temp2Speed = flight.flightTracks[0].positions[1].speedMph * 1.60934;
                position.Temp2Altitude = flight.flightTracks[0].positions[1].altitudeFt * 0.3048;
                position.Temp2UtcTimeStamp = flight.flightTracks[0].positions[1].date;

                position.IsInFlight = true;
            }
            else
            {
                position.IsInFlight = false;
                NullifyPosition(position);
            }
            position.IsActive = flight.appendix.airlines[0].active;
            return true;
        }


        //http://williams.best.vwh.net/avform.htm
        //http://stackoverflow.com/questions/6074476/calculating-a-future-position
        public void CalculateBoundingBox(Position position)
        {
            //Figure out bounding box
        }

        #region Helper
        //TODO: Remove any collision potentials associated with this aircraft
        public void NullifyPosition(Position position)
        {
            position.Temp1Latitude = new Nullable<decimal>();
            position.Temp1Longitude = new Nullable<decimal>();
            position.Temp1Speed = new Nullable<int>();
            position.Temp1Altitude = new Nullable<int>();
            position.Temp1Heading = new Nullable<int>();
            position.Temp1UtcTimeStamp = new Nullable<DateTime>();

            position.Temp2Latitude = new Nullable<decimal>();
            position.Temp2Longitude = new Nullable<decimal>();
            position.Temp2Speed = new Nullable<int>();
            position.Temp2Altitude = new Nullable<int>();
            position.Temp2Heading = new Nullable<int>();
            position.Temp2UtcTimeStamp = new Nullable<DateTime>();

            position.X1 = new Nullable<decimal>();
            position.Y1 = new Nullable<decimal>();
            position.Z1 = new Nullable<int>();

            position.X2 = new Nullable<decimal>();
            position.Y2 = new Nullable<decimal>();
            position.Z2 = new Nullable<int>();

            position.X3 = new Nullable<decimal>();
            position.Y3 = new Nullable<decimal>();
            position.Z3 = new Nullable<int>();

            position.X4 = new Nullable<decimal>();
            position.Y4 = new Nullable<decimal>();
            position.Z4 = new Nullable<int>();

            position.X5 = new Nullable<decimal>();
            position.Y5 = new Nullable<decimal>();
            position.Z5 = new Nullable<int>();

            position.X6 = new Nullable<decimal>();
            position.Y6 = new Nullable<decimal>();
            position.Z6 = new Nullable<int>();

            position.X7 = new Nullable<decimal>();
            position.Y7 = new Nullable<decimal>();
            position.Z7 = new Nullable<int>();

            position.X8 = new Nullable<decimal>();
            position.Y8 = new Nullable<decimal>();
            position.Z8 = new Nullable<int>();
        }
        #endregion
    }
}
