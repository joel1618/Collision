using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using Collision.Console.Interfaces;
using Microsoft.Practices.Unity;
using Collision.Sql.Ef.Repositories.Interfaces;
using Collision.Sql.Ef.Repositories;
using Collision.Core.Models;
using Newtonsoft.Json;
using System.Data.Entity;

namespace Collision.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            #if DEBUG
            Database.SetInitializer(new Collision.Sql.Ef.Migrations.Configuration());
            Collision.Sql.Ef.CollisionEntities db = new Collision.Sql.Ef.CollisionEntities();
            db.Database.Initialize(true);
            #endif

            var container = new UnityContainer();
            container.RegisterType<IPositionRepository, PositionRepository>();
            container.RegisterType<IAircraftRepository, AircraftRepository>();
            container.RegisterType<IConflictRepository, ConflictRepository>();
            container.RegisterType<IApplication, Application>();
            var app = container.Resolve<Application>();
            app.Run();
        }
    }
}
