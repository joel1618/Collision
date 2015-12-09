using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using Collision.Sql.Ef.Interfaces;
using Collision.Sql.Ef.Repositories;
using Collision.Core.Models;

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
        private Dictionary<int, Task> handlePosition = new Dictionary<int, Task>();
        private Dictionary<int, Task> handleCollision = new Dictionary<int, Task>();

        public Application(IPositionRepository positionRepository, IAircraftRepository aircraftRepository)
        {
            _positionRepository = positionRepository;
            _aircraftRepository = aircraftRepository;
        }

        public void Run()
        {
            //Go get the list from flightstats where flight starttime > datetime.now - 24 hours ago.  
            var flights = _aircraftRepository.GetAll();            
            foreach(var flight in flights)
            {
                if (!handlePosition.ContainsKey(flight.Id))
                {
                    handlePosition.Add(flight.Id, Task.Factory.StartNew(() => HandlePosition(flight));
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
            var positions = _positionRepository.GetAll();
            Position _position = null;
            foreach (var position in positions)
            {
                if (position.AircraftId == aircraft.Id)
                {
                    _position = position;
                }
            }
            if(_position == null)
            {
                //No position yet exists for this aircraft and we need to create a new one

                //Call api for flight

                //Fill in object and calculate bounding box

                //Call HandleCollisions to start evaluating this position for potential collisions
            }
            else
            {
                //We found a position and need to update position from api and recalculate boundingbox

                //Call HandleCollisions to start evaluating this position for potential collisions
            }
            //Wait 30 seconds before evaluating this flight again.
            Thread.Sleep(30000);
            HandlePosition(aircraft);
        } 

        public void UpdateFlightInformation(Aircraft aircraft, Position position)
        {           
            //web service call
            //"https://api.flightstats.com/flex/flightstatus/rest/v2/json/flight/tracks/AA/100/dep/2015/12/5?appId=284fdac1&appKey=543f72a5d73e4fcf3dba3c4355413bd5&utc=true&includeFlightPlan=false&maxPositions=2"

            CalculateBoundingBox(position);
            handleCollision.Add(aircraft.Id, Task.Factory.StartNew(() => HandleCollision(position)));
        }

        public void CalculateBoundingBox(Position position)
        {

        }

        public void HandleCollision(Position position)
        {
            //Check to see if already evaluating position.  If so do nothing. 
        }
    }
}
