using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using Collision.Sql.Ef.Repositories.Interfaces;
using Collision.Sql.Ef.Repositories;
using Collision.Console.Interfaces;
using Collision.Core.Models;
using Newtonsoft.Json;
using AircraftEntity = Collision.Sql.Ef.Aircraft;
using AircraftCore = Collision.Core.Models.Aircraft;

namespace Collision.Console
{
    public class Application : IApplication
    {
        private IPositionRepository _positionRepository;
        private IAircraftRepository _aircraftRepository;
        private IConflictRepository _conflictRepository;
        private HandlePosition position = null;

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
                System.Console.WriteLine("Getting aircraft list.");

                List<AircraftCore> aircraftList = null;
                int page = 0;
                int pageSize = Int32.Parse(ConfigurationManager.AppSettings["aircraftPerThread"]);

                do {
                    aircraftList = _aircraftRepository.Search(null, page, pageSize).ToList();

                    if (aircraftList.Count() > 0)
                    {
                        var tempAircraftList = aircraftList;
                        ThreadStart action = () =>
                        {
                            HandlePosition(tempAircraftList);
                        };
                        Thread thread = new Thread(action, Int32.Parse(ConfigurationManager.AppSettings["threadStackSize"])) { IsBackground = true };
                        thread.Start();
                    }
                    page++;
                } while (aircraftList.Count() == pageSize);

                //Sleep before getting the list again and going through it to see if any new flights have been added or a flight has been set to inactive.
                Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["handleAircraftTimeInterval"]));
            } while (true);
        }         
        
        private void HandlePosition(List<AircraftCore> aircrafts)
        {
            position = new HandlePosition(
                    new PositionRepository(new Sql.Ef.CollisionEntities()),
                    new AircraftRepository(new Sql.Ef.CollisionEntities()),
                    new ConflictRepository(new Sql.Ef.CollisionEntities()));
            position.HandlePositions(aircrafts);
        }

        private static List<List<AircraftCore>> splitList(List<AircraftCore> aircrafts, int size)
        {
            var list = new List<List<AircraftCore>>();
            for (int i = 0; i < aircrafts.Count; i += size)
            {
                list.Add(aircrafts.GetRange(i, Math.Min(size, aircrafts.Count - i)));
            }
            return list;
        }
    }
}
