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
        private IPositionService _positionService;
        private IAircraftService _aircraftService;
        private IConflictService _conflictService;
        private HandleCollision collision = null;
        private string baseUrl = "https://api.flightstats.com/flex/flightstatus/rest/v2/json/flight/tracks/";
        private string endUrl = "?appId=" + ConfigurationManager.AppSettings["appId"] + "&appKey=" + ConfigurationManager.AppSettings["appKey"] + "&utc=true&includeFlightPlan=false&maxPositions=2";
        private dynamic flight = null;

        //TODO: Figure out why not executing quickly
        public HandlePosition(IPositionService positionService, IAircraftService aircraftService, IConflictService conflictService)
        {
            _positionService = positionService;
            _aircraftService = aircraftService;
            _conflictService = conflictService;
        }

        public void HandlePositions(List<Aircraft> aircrafts)
        {
            do
            {
                foreach (Aircraft aircraft in aircrafts)
                {
                    Position _position = null;
                    if (!aircraft.IsActive)
                    {
                        _position = _positionService.GetByAircraftId(aircraft.Id);
                        NullifyPosition(_position);
                        //Remove collision potentials associated with this position
                        RemoveCollisions(_position);
                        _position.IsActive = false; _position.IsInFlight = false;
                        _positionService.Update(_position.Id, _position);
                    }
                    else {
                        System.Console.WriteLine("Handling position for " + aircraft.CarrierName + " flight " + aircraft.FlightNumber);
                        _position = _positionService.GetByAircraftId(aircraft.Id);
                    }

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
                            //Calculate the positions bounds
                            HandleBoundingBox.CalculateBoundingBox(_position);
                            //Update position object in database
                            _position = _positionService.Update(_position.Id, _position);
                            //Call HandleCollisions to start evaluating this position for potential collisions
                            HandleCollision(aircraft, _position);
                        }
                        else
                        {
                            //There was an error from the API
                        }
                    }
                    else
                    {
                        //We found a position and need to update position from api and recalculate boundingbox
                        if (UpdateFlightInformation(aircraft, _position))
                        {
                            //Calculate the positions bounds
                            HandleBoundingBox.CalculateBoundingBox(_position);
                            //Update position object in database
                            _position = _positionService.Update(_position.Id, _position);
                            //Call HandleCollisions to start evaluating this position for potential collisions
                            HandleCollision(aircraft, _position);
                        }
                        else
                        {
                            //There was an error from the API
                        }
                    }
                }
            } while (true);
        }

        public void HandleCollision(Aircraft aircraft, Position position)
        {
            if (collision == null)
            {
                collision = new HandleCollision(
                        new PositionService(new Sql.Ef.CollisionEntities()),
                        new ConflictService(new Sql.Ef.CollisionEntities()));
            }
            collision.HandleCollisions(position);
        }

        #region API 
        public bool UpdateFlightInformation(Aircraft aircraft, Position position)
        {     
            //Testing the application.
            if (bool.Parse(ConfigurationManager.AppSettings["mockData"]))
            {
                flight = new MockData().Get();
            }
            else
            {
                using (var syncClient = new WebClient())
                {
                    flight = JsonConvert.DeserializeObject(syncClient.DownloadString(GetUrl(aircraft)));
                }
            }
            if (flight.error != null)
            {
                if (position.Id != 0)
                {
                    System.Console.WriteLine("API cannot find " + aircraft.CarrierName + " flight " + aircraft.FlightNumber + ". Removing position.");
                    //Remove collision potentials associated with this position
                    RemoveCollisions(position);
                    //Delete the position
                    _positionService.Delete(position.Id);
                }
                return false;
            }

            if (flight.flightTracks.Count != 0 && flight.flightTracks[0].positions.Count == 2)
            {
                position.Latitude2 = (decimal)flight.flightTracks[0].positions[0].lat;
                position.Longitude2 = (decimal)flight.flightTracks[0].positions[0].lon;
                position.Speed2 = Convert.ToDecimal(flight.flightTracks[0].positions[0].speedMph * 1.60934); //convert to kilometers per hour
                position.Altitude2 = Convert.ToDecimal(flight.flightTracks[0].positions[0].altitudeFt * 0.3048 * .001); //convert to kilometers
                position.Heading2 = (decimal)flight.flightTracks[0].heading;
                position.UtcTimeStamp2 = flight.flightTracks[0].positions[0].date;

                position.Latitude3 = (decimal)flight.flightTracks[0].positions[1].lat;
                position.Longitude3 = (decimal)flight.flightTracks[0].positions[1].lon;
                position.Speed3 = Convert.ToDecimal(flight.flightTracks[0].positions[1].speedMph * 1.60934); //convert to kilometers per hour
                position.Altitude3 = Convert.ToDecimal(flight.flightTracks[0].positions[1].altitudeFt * 0.3048 * .001); //convert to kilometers
                position.Heading3 = (decimal)flight.flightTracks[0].heading;
                position.UtcTimeStamp3 = flight.flightTracks[0].positions[1].date;

                position.Radius = 5;

                position.IsInFlight = true;
            }
            else
            {
                position.IsInFlight = false;
                NullifyPosition(position);
                //Remove collision potentials associated with this position
                RemoveCollisions(position);
            }
            position.IsActive = flight.appendix.airlines[0].active;

            return true;
        }
        #endregion

        #region Helper        
        public static void NullifyPosition(Position position)
        {
            position.Latitude1 = new Nullable<decimal>();
            position.Longitude1 = new Nullable<decimal>();
            position.Speed1 = new Nullable<int>();
            position.Altitude1 = new Nullable<int>();
            position.Heading1 = new Nullable<int>();
            position.UtcTimeStamp1 = new Nullable<DateTime>();

            position.Latitude2 = new Nullable<decimal>();
            position.Longitude2 = new Nullable<decimal>();
            position.Speed2 = new Nullable<int>();
            position.Altitude2 = new Nullable<int>();
            position.Heading2 = new Nullable<int>();
            position.UtcTimeStamp2 = new Nullable<DateTime>();

            position.Latitude3 = new Nullable<decimal>();
            position.Longitude3 = new Nullable<decimal>();
            position.Speed3 = new Nullable<int>();
            position.Altitude3 = new Nullable<int>();
            position.Heading3 = new Nullable<int>();
            position.UtcTimeStamp3 = new Nullable<DateTime>();

            position.Radius = new Nullable<decimal>();

            position.X1 = new Nullable<decimal>();
            position.Y1 = new Nullable<decimal>();
            position.Z1 = new Nullable<int>();

            position.X2 = new Nullable<decimal>();
            position.Y2 = new Nullable<decimal>();
            position.Z2 = new Nullable<int>();

            position.X3 = new Nullable<decimal>();
            position.Y3 = new Nullable<decimal>();
            position.Z3 = new Nullable<int>();
        }

        private void RemoveCollisions(Position position)
        {
            var collisions = _conflictService.GetByPositionId1(position.Id);
            foreach (var collision in collisions)
            {
                _conflictService.Delete(collision.Id);
            }
        }
        
        private string GetUrl(Aircraft aircraft)
        {
            return baseUrl +
                    aircraft.Carrier +
                    "/" + aircraft.FlightNumber +
                    "/dep/" + DateTime.Now.Year +
                    "/" + DateTime.Now.Month +
                    "/" + DateTime.Now.Day +
                    endUrl;
        }
        #endregion
    }
}
