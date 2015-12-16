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
        private IPositionService _positionService;
        private IAircraftService _aircraftService;
        private Dictionary<int, ThreadStart> handlePosition = new Dictionary<int, ThreadStart>();

        public Application(IPositionService positionService, IAircraftService aircraftService)
        {
            _positionService = positionService;
            _aircraftService = aircraftService;
        }

        //TODO: connection pool stuff
        //TODO: threading optimization
        public void Run()
        {
            //Go get the list from flightstats where flight starttime > datetime.now - 24 hours ago. 
            System.Console.WriteLine("Getting aircraft list.");
            var aircrafts = _aircraftService.GetAll();
            foreach (var aircraft in aircrafts)
            {
                if (aircraft.IsActive)
                {
                    if (!handlePosition.ContainsKey(aircraft.Id))
                    {                   
                        ThreadStart action = () =>
                        {
                            var handlePosition = new HandlePosition(
                           new PositionService(new Sql.Ef.CollisionEntities()),
                           new AircraftService(new Sql.Ef.CollisionEntities()),
                           new ConflictService(new Sql.Ef.CollisionEntities()));
                            handlePosition.HandlePositions(aircraft);
                        };
                        Thread thread = new Thread(action, Int32.Parse(ConfigurationManager.AppSettings["threadStackSize"])) { IsBackground = true };
                        thread.Start();
                        handlePosition.Add(aircraft.Id, action);
                    }
                }
                else
                {
                    //Remove from handlePosition dictionary 
                    if (handlePosition.ContainsKey(aircraft.Id))
                    {
                        handlePosition.Remove(aircraft.Id);
                    }
                    //Set position record to inactive if it exists
                    var position = _positionService.GetByAircraftId(aircraft.Id);
                    if (position != null)
                    {
                        HandlePosition.NullifyPosition(position);
                        position.IsActive = false;
                        _positionService.Update(position.Id, position);
                    }
                }
            }
            //Sleep before getting the list again and going through it to see if any new flights have been added.
            Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["handleAircraftTimeInterval"]));
            //Necessary in order to get changed data
            _positionService = new PositionService(new Sql.Ef.CollisionEntities());
            _aircraftService = new AircraftService(new Sql.Ef.CollisionEntities());
            Run();
        }
    }
}
