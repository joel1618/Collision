﻿using System;
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
        private Queue<Aircraft> queueAircraft = new Queue<Aircraft>();

        public Application(IPositionService positionService, IAircraftService aircraftService, IConflictService conflictService)
        {
            _positionService = positionService;
            _aircraftService = aircraftService;
            _conflictService = conflictService;
        }

        public void Run()
        {
            //Go get the list from flightstats where flight starttime > datetime.now - 24 hours ago. 
            System.Console.WriteLine("Getting aircraft list.");
            List<List<Aircraft>> aircraftLists = splitList(_aircraftService.GetAllActive().ToList());

            foreach(List<Aircraft> aircraftList in aircraftLists)
            {
                ThreadStart action = () =>
                {
                    HandlePosition(aircraftList);
                };
                Thread thread = new Thread(action, Int32.Parse(ConfigurationManager.AppSettings["threadStackSize"])) { IsBackground = true };
                thread.Start();
            }
            //Sleep before getting the list again and going through it to see if any new flights have been added.
            Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["handleAircraftTimeInterval"]));
            Run();
        }        

        private void HandlePosition(List<Aircraft> aircrafts)
        {
            var position = new HandlePosition(new PositionService(new Sql.Ef.CollisionEntities()),new AircraftService(new Sql.Ef.CollisionEntities()),new ConflictService(new Sql.Ef.CollisionEntities()));
            position.HandlePositions(aircrafts);
        }

        private static List<List<Aircraft>> splitList(List<Aircraft> aircrafts, int nSize = 30)
        {
            var list = new List<List<Aircraft>>();
            for (int i = 0; i < aircrafts.Count; i += nSize)
            {
                list.Add(aircrafts.GetRange(i, Math.Min(nSize, aircrafts.Count - i)));
            }
            return list;
        }
    }
}
