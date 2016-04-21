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
using Collision.Data.Repositories.Interfaces;
using Collision.Data.Repositories;
using Collision.Core.Models;
using Newtonsoft.Json;

namespace Collision.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
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
