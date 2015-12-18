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
        private Queue<Aircraft> queueAircraft = new Queue<Aircraft>();

        public Application(IPositionService positionService, IAircraftService aircraftService, IConflictService conflictService)
        {
            _positionService = positionService;
            _aircraftService = aircraftService;
            _conflictService = conflictService;
        }
         
        /*TODO: Threads execution time needs to be synchronized so that we can accurately project. 
                select max(ModifiedAtUtcTimeStamp) - min(ModifiedAtUtcTimeStamp) from Position where IsActive = 1
                Right now seeing a differential of about 55 seconds.
                One potential solution here would be to have app running on multiple machines, 
                and each app responsible for a range of flights or faster machine.  CPU and IO are pegged.    
        */
        //TODO: May need to wrap the services using (var context = new MyDbContext(ConnectionString)) {} so that the connection isn't held onto.
        //Right now after about 1 hour, the app has used 2Gb of memory and connections to database are consistent.  No crashes. GC seems to take care of things right before the 2Gb mark.  
        //No substantial memory leak after that?  
        public void Run()
        {
            do
            {
                //TODO:  May have a problem bringing everything into mem here.  
                System.Console.WriteLine("Getting aircraft list.");

                //Deactive positions and remove conflicts related to the position.
                HandleInActiveAircraft();

                List<List<Aircraft>> aircraftLists = splitList(_aircraftService.GetAllActive().ToList(), Int32.Parse(ConfigurationManager.AppSettings["aircraftPerThread"]));

                foreach (List<Aircraft> aircraftList in aircraftLists)
                {
                    ThreadStart action = () =>
                    {
                        HandlePosition(aircraftList);
                    };
                    Thread thread = new Thread(action, Int32.Parse(ConfigurationManager.AppSettings["threadStackSize"])) { IsBackground = true };
                    thread.Start();
                }

                //Sleep before getting the list again and going through it to see if any new flights have been added or a flight has been set to inactive.
                Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["handleAircraftTimeInterval"]));
            } while (true);
        }         
        
        private void HandlePosition(List<Aircraft> aircrafts)
        {
            var position = new HandlePosition(new PositionService(new Sql.Ef.CollisionEntities()),new AircraftService(new Sql.Ef.CollisionEntities()),new ConflictService(new Sql.Ef.CollisionEntities()));
            position.HandlePositions(aircrafts);
        }

        private void HandleInActiveAircraft()
        {
            List<Aircraft> inactiveAircraftList = _aircraftService.GetAllInActive().ToList();
            foreach (var aircraft in inactiveAircraftList)
            {
                var position = _positionService.GetByAircraftId(aircraft.Id);
                if (position != null)
                {
                    var conflicts = _conflictService.GetByPositionId1OrPositionId2(position.Id);
                    foreach (var conflict in conflicts)
                    {
                        _conflictService.Delete(conflict.Id);
                    }
                    position.IsActive = false;
                    Helper.NullifyPosition(position);
                    _positionService.Update(position.Id, position);
                }
            }
        }

        private static List<List<Aircraft>> splitList(List<Aircraft> aircrafts, int size)
        {
            var list = new List<List<Aircraft>>();
            for (int i = 0; i < aircrafts.Count; i += size)
            {
                list.Add(aircrafts.GetRange(i, Math.Min(size, aircrafts.Count - i)));
            }
            return list;
        }
    }
}
