using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using IPositionRepository = Collision.Sql.Ef.Interfaces.IPositionRepository;
using PositionRepository = Collision.Sql.Ef.Repositories.PositionRepository;
using CorePosition = Collision.Core.Models.Position;

namespace Collision.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            container.RegisterType<IPositionRepository, PositionRepository>();
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
        private readonly IPositionRepository _repository;
        public Application(IPositionRepository repository)
        {
            _repository = repository;
        }
        
        private Dictionary<int, Task> taskList = new Dictionary<int, Task>();
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
            int[] flights = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            foreach(var flightId in flights)
            {
                if (!taskList.ContainsKey(flightId))
                {
                    var task = Task.Factory.StartNew(() => HandleFlight(flightId));
                    System.Console.WriteLine("Adding flightId " + flightId.ToString());
                    taskList.Add(flightId, task);
                }
            }
            Thread.Sleep(1000);
            System.Console.WriteLine("Go get new flights");
            Run();
            
        }

        public void HandleFlight(int flightId)
        {
            //What this will do is go get the flight data by flightId, then draw the bounding box, then repeat until the flight is over.  
            Random rnd = new Random();
            //simulating work.  
            Thread.Sleep(rnd.Next(10000, 20000));
            System.Console.WriteLine("Work for flightId " + flightId.ToString() + " is done.");
            taskList.Remove(flightId);
        } 

        public void HandleColisions()
        {
            //Get all flights within a certain relative position.  lat/long 
        }
    }
}
