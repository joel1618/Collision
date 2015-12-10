using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using Collision.Sql.Ef.Services.Interfaces;
using Collision.Sql.Ef.Services;
using Collision.Core.Models;
using Newtonsoft.Json;

namespace Collision.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            container.RegisterType<IPositionService, PositionService>();
            container.RegisterType<IAircraftService, AircraftService>();
            container.RegisterType<IApplication, Application>();
            var app = container.Resolve<Application>();
            app.Run();
        }
    }
    public interface IApplication
    {
        void Run();
    }

    public class Application : IApplication
    {
        private readonly IPositionService _positionService;
        private readonly IAircraftService _aircraftService;
        private Dictionary<int, Task> handlePosition = new Dictionary<int, Task>();
        private Dictionary<int, Task> handleCollision = new Dictionary<int, Task>();

        public Application(IPositionService positionService, IAircraftService aircraftService)
        {
            _positionService = positionService;
            _aircraftService = aircraftService;
        }

        public void Run()
        {
            //Clear all position data on initial run

            //Continue
            RunStep2();
        }

        public void RunStep2()
        {

            //Go get the list from flightstats where flight starttime > datetime.now - 24 hours ago.  
            var aircrafts = _aircraftService.GetAll();
            foreach (var aircraft in aircrafts)
            {
                if (!handlePosition.ContainsKey(aircraft.Id))
                {
                    handlePosition.Add(aircraft.Id, Task.Factory.StartNew(() => HandlePosition(aircraft)));
                }
            }
            //Sleep 5 minutes before getting the list again and going through it to see if any new flights have been added.
            Thread.Sleep(300000);
            System.Console.WriteLine("Go get new flights");
            RunStep2();
        }

        public void HandlePosition(Aircraft aircraft)
        {
            var _position = _positionService.GetByAircraftId(aircraft.Id);
            if(_position == null)
            {
                //No position yet exists for this aircraft and we need to create a new one
                _position = new Position();
                _position.AircraftId = aircraft.Id;
                //Call api for flight
                UpdateFlightInformation(aircraft, _position);
                //Create position in database
                _position = _positionService.Create(_position);
                //Calculate bounding box
                CalculateBoundingBox(_position);
                //Update position object in database

                //Call HandleCollisions to start evaluating this position for potential collisions
                if (!handleCollision.ContainsKey(_position.Id))
                {
                    handleCollision.Add(_position.Id, Task.Factory.StartNew(() => HandleCollision(_position)));
                }
            }
            else
            {
                //We found a position and need to update position from api and recalculate boundingbox
                UpdateFlightInformation(aircraft, _position);
                //Call HandleCollisions to start evaluating this position for potential collisions
                CalculateBoundingBox(_position);
                //Update position object in database

                //Call HandleCollisions to start evaluating this position for potential collisions
                if (!handleCollision.ContainsKey(_position.Id))
                {
                    handleCollision.Add(_position.Id, Task.Factory.StartNew(() => HandleCollision(_position)));
                }
            }
            //Wait 30 seconds before evaluating this flight again.
            Thread.Sleep(30000);
            HandlePosition(aircraft);
        } 

        //TODO: Eventually move appId and appKey to config file
        public void UpdateFlightInformation(Aircraft aircraft, Position position)
        {
            var baseUrl = "https://api.flightstats.com/flex/flightstatus/rest/v2/json/flight/tracks/";
            //TODO: Use Stringbuilder here.
            var url = baseUrl + 
                aircraft.Carrier + "/" + 
                aircraft.FlightNumber + "/dep/" +
                DateTime.Now.Year + "/" +
                DateTime.Now.Month + "/" +
                DateTime.Now.Day + "?appId=284fdac1&appKey=543f72a5d73e4fcf3dba3c4355413bd5&utc=true&includeFlightPlan=false&maxPositions=2";
            var syncClient = new WebClient();
            dynamic content = JsonConvert.DeserializeObject(syncClient.DownloadString(url));

            position.Temp1Latitude = content.flightTracks[0].positions[0].lat;
            position.Temp1Longitude = content.flightTracks[0].positions[0].lon;
            position.Temp1Speed = content.flightTracks[0].positions[0].speedMph * 1.60934;
            position.Temp1Altitude = content.flightTracks[0].positions[0].altitudeFt * 0.3048;
            position.Temp1UtcTimeStamp = content.flightTracks[0].positions[0].date;

            position.Temp2Latitude = content.flightTracks[0].positions[1].lat;
            position.Temp2Longitude = content.flightTracks[0].positions[1].lon;
            position.Temp2Speed = content.flightTracks[0].positions[1].speedMph * 1.60934;
            position.Temp2Altitude = content.flightTracks[0].positions[1].altitudeFt * 0.3048;
            position.Temp2UtcTimeStamp = content.flightTracks[0].positions[1].date;

            position.IsActive = content.appendix.airlines[0].active;
        }

        public void CalculateBoundingBox(Position position)
        {
            //Figure out bounding box
        }

        public void HandleCollision(Position position)
        {
            //Handle bounding box collisions

            //If found insert into another table with information about the collision (need to work out these details)
        }
    }
}
