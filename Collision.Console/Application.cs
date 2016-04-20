using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using Collision.Data.Repositories.Interfaces;
using Collision.Data.Repositories;
using Collision.Console.Interfaces;
using Collision.Core.Models;
using Collision.Business.Services;
using Newtonsoft.Json;

namespace Collision.Console
{
    public class Application : IApplication
    {
        private IPositionRepository _positionRepository;
        private IAircraftRepository _aircraftRepository;
        private IConflictRepository _conflictRepository;
        private HashSet<int> handlePosition = new HashSet<int>();
        private Queue<Aircraft> queueAircraft = new Queue<Aircraft>();

        public Application(IPositionRepository positionRepository, IAircraftRepository aircraftRepository, IConflictRepository conflictRepository)
        {
            _positionRepository = positionRepository;
            _aircraftRepository = aircraftRepository;
            _conflictRepository = conflictRepository;
        }
         
        /*TODO: Threads execution time needs to be synchronized so that we can accurately project. 
                select max(ModifiedAtUtcTimeStamp) - min(ModifiedAtUtcTimeStamp) from Position where IsActive = 1
                Right now seeing a differential of about 55 seconds.
                One potential solution here would be to have app running on multiple machines, 
                and each app responsible for a range of flights or faster machine.  CPU and IO are pegged.    
        */
        //TODO: May need to wrap the Repositorys using (var context = new MyDbContext(ConnectionString)) {} so that the connection isn't held onto.
        public void Run()
        {
            do
            {
                //TODO:  May have a problem bringing everything into mem here.  
                System.Console.WriteLine("Getting aircraft list.");

                List<List<Aircraft>> aircraftLists = splitList(_aircraftRepository.GetAll().ToList(), Int32.Parse(ConfigurationManager.AppSettings["aircraftPerThread"]));

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
            var positionRepository = new PositionRepository(new Data.CollisionEntities());
            var aircraftRepository = new AircraftRepository(new Data.CollisionEntities());
            var conflictRepository = new ConflictRepository(new Data.CollisionEntities());
            var flightStatsRepository = new FlightStatsRepository();
            var flightStatsMockRepository = new FlightStatsMockRepository();

            var positionService = new PositionService(flightStatsRepository, flightStatsMockRepository, positionRepository, conflictRepository);

            var position = new HandlePosition(positionRepository, aircraftRepository, conflictRepository, positionService);
            position.HandlePositions(aircrafts);
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
