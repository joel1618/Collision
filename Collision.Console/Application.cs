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
        private IConflictService _conflictService;
        private HashSet<int> handlePosition = new HashSet<int>();

        public Application(IPositionService positionService, IAircraftService aircraftService, IConflictService conflictService)
        {
            _positionService = positionService;
            _aircraftService = aircraftService;
            _conflictService = conflictService;
        }

        //TODO: connection pool stuff
        //TODO: threading optimization
        //TODO: figure out memory leak
        public void Run()
        {
            //Go get the list from flightstats where flight starttime > datetime.now - 24 hours ago. 
            System.Console.WriteLine("Getting aircraft list.");
            var aircrafts = _aircraftService.GetAll();
            foreach (var aircraft in aircrafts)
            {
                if (aircraft.IsActive)
                {
                    if (!handlePosition.Contains(aircraft.Id))
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
                        handlePosition.Add(aircraft.Id);
                    }
                }
                else
                {
                    //Remove from handlePosition dictionary 
                    if (handlePosition.Contains(aircraft.Id))
                    {
                        handlePosition.Remove(aircraft.Id);
                    }
                    //Set position record to inactive if it exists
                    var position = _positionService.GetByAircraftId(aircraft.Id);
                    if (position != null)
                    {
                        HandlePosition.NullifyPosition(position);
                        RemoveCollisions(position);
                        position.IsActive = false;
                        _positionService.Update(position.Id, position);
                    }
                    position = null;
                }
            }
            //Sleep before getting the list again and going through it to see if any new flights have been added.
            aircrafts = null;
            Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["handleAircraftTimeInterval"]));
            //Necessary in order to get changed data
            //_positionService = new PositionService(new Sql.Ef.CollisionEntities());
            //_aircraftService = new AircraftService(new Sql.Ef.CollisionEntities());
            Run();
        }

        private void RemoveCollisions(Position position)
        {
            var collisions = _conflictService.GetByPositionId1(position.Id);
            foreach (var collision in collisions)
            {
                _conflictService.Delete(collision.Id);
            }
        }
    }
}
