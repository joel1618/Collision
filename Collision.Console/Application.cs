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
        private Dictionary<int, CancellationTokenSource> handlePosition = new Dictionary<int, CancellationTokenSource>();

        public Application(IPositionService positionService, IAircraftService aircraftService)
        {
            _positionService = positionService;
            _aircraftService = aircraftService;
        }

        public void Run()
        {
            //Go get the list from flightstats where flight starttime > datetime.now - 24 hours ago. 
            System.Console.WriteLine("Getting aircraft list");
            var aircrafts = _aircraftService.GetAll();
            foreach (var aircraft in aircrafts)
            {
                if (aircraft.IsActive)
                {
                    if (!handlePosition.ContainsKey(aircraft.Id))
                    {
                        System.Console.WriteLine("Handling position for " + aircraft.CarrierName + " flight " + aircraft.FlightNumber);
                        CancellationTokenSource ts = new CancellationTokenSource();
                        CancellationToken ct = ts.Token;
                        Task.Factory.StartNew(() => new HandlePosition(
                           new PositionService(new Sql.Ef.CollisionEntities()),
                           new AircraftService(new Sql.Ef.CollisionEntities())).HandlePositions(aircraft), ct);
                        handlePosition.Add(aircraft.Id, ts);
                    }
                }
                else
                {
                    //Remove from handlePosition dictionary 
                    if (handlePosition.ContainsKey(aircraft.Id))
                    {
                        //var ts = handlePosition[aircraft.Id];
                        ////TODO: Figure out why this is not killing the tasks.
                        //ts.Cancel();
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
