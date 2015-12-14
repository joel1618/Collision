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
using Collision.Console.Interfaces;
using Collision.Core.Models;
using Newtonsoft.Json;

namespace Collision.Console
{


    public class Application : IApplication
    {
        private readonly IPositionService _positionService;
        private readonly IAircraftService _aircraftService;
        private Dictionary<int, Task> handlePosition = new Dictionary<int, Task>();

        public Application(IPositionService positionService, IAircraftService aircraftService)
        {
            _positionService = positionService;
            _aircraftService = aircraftService;
        }

        public void Run()
        {
            //Go get the list from flightstats where flight starttime > datetime.now - 24 hours ago. 
            var aircrafts = _aircraftService.GetAll();
            foreach (var aircraft in aircrafts)
            {
                if (aircraft.IsActive)
                {
                    if (!handlePosition.ContainsKey(aircraft.Id))
                    {
                        System.Console.WriteLine("Handling position for " + aircraft.CarrierName + " flight " + aircraft.FlightNumber);
                        handlePosition.Add(aircraft.Id, Task.Factory.StartNew(() => new HandlePosition(
                            new PositionService(new Sql.Ef.CollisionEntities()),
                            new AircraftService(new Sql.Ef.CollisionEntities())).HandlePositions(aircraft)));
                    }
                }
                else
                {
                    //TODO: Set position record to inactive if it exists
                    
                    //TODO: Kill the HandlePosition Task and remove from handlePosition dictionary     
                }
            }
            //Sleep 5 minutes before getting the list again and going through it to see if any new flights have been added.
            Thread.Sleep(300000);
            System.Console.WriteLine("Go get new flights");
            Run();
        }
    }
}
