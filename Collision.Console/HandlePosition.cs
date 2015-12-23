using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using Collision.Sql.Ef.Repositories.Interfaces;
using Collision.Sql.Ef.Repositories;
using Collision.Core.Models;
using Newtonsoft.Json;

namespace Collision.Console
{
    public class HandlePosition
    {
        private IPositionRepository _positionRepository;
        private IAircraftRepository _aircraftRepository;
        private IConflictRepository _conflictRepository;
        private HandleCollision collision = null;
        private string baseUrl = "https://api.flightstats.com/flex/flightstatus/rest/v2/json/flight/tracks/";
        private string endUrl = "?appId=" + ConfigurationManager.AppSettings["appId"] + "&appKey=" + ConfigurationManager.AppSettings["appKey"] + "&utc=true&includeFlightPlan=false&maxPositions=2";
        private dynamic flight = null;

        //TODO: Figure out why not executing quickly
        public HandlePosition(IPositionRepository positionRepository, IAircraftRepository aircraftRepository, IConflictRepository conflictRepository)
        {
            _positionRepository = positionRepository;
            _aircraftRepository = aircraftRepository;
            _conflictRepository = conflictRepository;
        }

        public void HandlePositions(List<Aircraft> aircrafts)
        {
            do
            {
                foreach (Aircraft aircraft in aircrafts)
                {
                    if (aircraft.IsActive)
                    {
                        System.Console.WriteLine("Handling position for " + aircraft.CarrierName + " flight " + aircraft.FlightNumber);
                        var position = _positionRepository.GetByAircraftId(aircraft.Id);
                        if (position == null)
                        {
                            //No position yet exists for this aircraft and we need to create a new one
                            position = new Position();
                            position.AircraftId = aircraft.Id;
                            //Call api for flight
                            if (UpdateFlightInformation(aircraft, position))
                            {
                                //Create position in database
                                position = _positionRepository.Create(position);
                                //Calculate the positions bounds
                                HandleBoundingBox.CalculateBoundingBox(position);
                                //Update position object in database
                                position = _positionRepository.Update(position.Id, position);
                                //Call HandleCollisions to start evaluating this position for potential collisions
                                HandleCollision(aircraft, position);
                            }
                            else
                            {
                                //There was an error from the API
                            }
                        }
                        else
                        {
                            //We found a position and need to update position from api and recalculate boundingbox
                            if (UpdateFlightInformation(aircraft, position))
                            {
                                //Calculate the positions bounds
                                HandleBoundingBox.CalculateBoundingBox(position);
                                //Update position object in database
                                position = _positionRepository.Update(position.Id, position);
                                //Call HandleCollisions to start evaluating this position for potential collisions
                                HandleCollision(aircraft, position);
                            }
                            else
                            {
                                //There was an error from the API
                            }
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Handling inactive aircraft " + aircraft.CarrierName + " flight " + aircraft.FlightNumber);
                        HandleInActiveAircraft(aircraft);
                    }

                }
            } while (true);
        }

        public void HandleCollision(Aircraft aircraft, Position position)
        {
            if (collision == null)
            {
                collision = new HandleCollision(
                        new PositionRepository(new Sql.Ef.CollisionEntities()),
                        new ConflictRepository(new Sql.Ef.CollisionEntities()));
            }
            collision.HandleCollisions(position);
        }

        public void HandleInActiveAircraft(Aircraft aircraft)
        {
            var position = _positionRepository.GetByAircraftId(aircraft.Id);
            Helper.NullifyPosition(position);
            //Remove collision potentials associated with this position
            RemoveCollisions(position);
            position.IsActive = false; position.IsInFlight = false;
            _positionRepository.Update(position.Id, position);
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
                    _positionRepository.Delete(position.Id);
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
                Helper.NullifyPosition(position);
                //Remove collision potentials associated with this position
                RemoveCollisions(position);
            }
            position.IsActive = flight.appendix.airlines[0].active;

            return true;
        }
        #endregion

        #region Helper  

        private void RemoveCollisions(Position position)
        {
            var collisions = _conflictRepository.GetByPositionId1(position.Id);
            foreach (var collision in collisions)
            {
                _conflictRepository.Delete(collision.Id);
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
