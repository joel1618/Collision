using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using Collision.Sql.Ef.Interfaces;
using Collision.Sql.Ef.Repositories;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            container.RegisterType<IPositionRepository, PositionRepository>();
            container.RegisterType<IAircraftRepository, AircraftRepository>();
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
        private readonly IPositionRepository _positionRepository;
        private readonly IAircraftRepository _aircraftRepository;

        public Application(IPositionRepository positionRepository, IAircraftRepository aircraftRepository)
        {
            _positionRepository = positionRepository;
            _aircraftRepository = aircraftRepository;
        }
        
        private List<string> flightList = new List<string>();
        private List<string> flightListCurrentlyWorkingOn = new List<string>();
        public void Run()
        {
            //https://api.flightstats.com/flex/flightstatus/samples/v2/lts/FlightTrack_single_flight_defaults.json
            //General breakdown of what needs to happen
            //Get all of the flights that are active
            //Iterate through and spin up a new thread per flight.
            //Look in database for the flightId, if there update otherwise create.  Then go back to api and get latest data updating bounding box. 
            //(Algorithm relies on it being an active flight)

            //Another group of threads needs to be somehow looking at each flight and trying to find potential collisions.
            //Somehow collect and store status for potential collisions like flightId, distance, time, ect. ect.

            //Get All Flights

            //Pass flight to Task.  Keep track of those tasks that are running working on flights.  If the new flight list has a flight with a new task spin up new thread.
            //var position = new CorePosition()
            //{
            //    Name = "Joel",
            //    Temp1Latitude = 30.417991,
            //    Temp1Longitude = -97.690357,
            //    Temp1Altitude = 2000,
            //    Temp1Speed = 1000,
            //    Temp1Heading = 180,
            //    IsInFlight = false
            //};
            //position = _repository.Create(position);

            //Go get the list from flightstats where flight starttime > datetime.now - 24 hours ago.  
            var flights = _aircraftRepository.GetAll();
            int i = 0;
            /*
            foreach(var flight in flights)
            {
                if (!flightListCurrentlyWorkingOn.Contains(flight.Id))
                {
                    var task = Task.Factory.StartNew(() => HandleFlight(flight.Id));
                    System.Console.WriteLine("Adding flightId " + flight.Id.ToString());
                    flightListCurrentlyWorkingOn.Add(flight.Id);
                }
            }
            Thread.Sleep(1000);
            System.Console.WriteLine("Go get new flights");
            Run();*/
            
        }

        public void HandleFlight(string flightId)
        {
            //What this will do is go get the flight data by flightId, then draw the bounding box, then repeat until the flight is over.  
            //getting the fight by arline, number, and arrival date.
            //"https://api.flightstats.com/flex/flightstatus/rest/v2/json/flight/tracks/AA/100/dep/2015/12/5?appId=284fdac1&appKey=543f72a5d73e4fcf3dba3c4355413bd5&utc=true&includeFlightPlan=false&maxPositions=2""
            var flight = "";//web service call passing flightId

            //

            HandleFlight(flightId);
            //When we're done working on the flight remove it.
            //flightListCurrentlyWorkingOn.Remove(flightId);
        } 

        public void HandleCollisions()
        {
            //Get all flights within a certain relative position.  lat/long 
        }
    }
}
