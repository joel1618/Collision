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
            //Go get the list from flightstats where flight starttime > datetime.now - 24 hours ago.  
            var flights = _aircraftService.GetAll();            
            foreach(var flight in flights)
            {
                if (!handlePosition.ContainsKey(flight.Id))
                {
                    handlePosition.Add(flight.Id, Task.Factory.StartNew(() => HandlePosition(flight)));
                }
            }
            //Sleep 5 minutes before getting the list again and going through it.
            Thread.Sleep(300000);
            System.Console.WriteLine("Go get new flights");
            Run();
        }

        public void HandlePosition(Aircraft aircraft)
        {
            //TODO: Get the position by AircraftId.  Need to implement Search endpoint.
            var _position = _positionService.GetByAircraftId(aircraft.Id);
            if(_position == null)
            {
                //No position yet exists for this aircraft and we need to create a new one
                _position = new Position();
                _position.AircraftId = aircraft.Id;
                //Call api for flight
                UpdateFlightInformation(aircraft, _position);
                //Create position in database

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

        public void UpdateFlightInformation(Aircraft aircraft, Position position)
        {
            //web service call
            //"https://api.flightstats.com/flex/flightstatus/rest/v2/json/flight/tracks/AA/100/dep/2015/12/5?appId=284fdac1&appKey=543f72a5d73e4fcf3dba3c4355413bd5&utc=true&includeFlightPlan=false&maxPositions=2"
            var url = "https://api.flightstats.com/flex/flightstatus/rest/v2/json/flight/tracks/" + 
                aircraft.Carrier + "/" + 
                aircraft.FlightNumber + "/dep/" +
                DateTime.Now.Year + "/" +
                DateTime.Now.Month + "/" +
                DateTime.Now.Day + "?appId=284fdac1&appKey=543f72a5d73e4fcf3dba3c4355413bd5&utc=true&includeFlightPlan=false&maxPositions=2";
            var syncClient = new WebClient();
            var content = syncClient.DownloadString(url);
            //Fill in latitude/longitude/speed/heading/altitude/time temp properties with values from api.
           
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
