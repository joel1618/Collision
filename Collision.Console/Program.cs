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
using Collision.Sql.Ef.Services.Interfaces;
using Collision.Sql.Ef.Services;
using Collision.Core.Models;
using Newtonsoft.Json;

namespace Collision.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            container.RegisterType<IPositionService, PositionService>();
            container.RegisterType<IAircraftService, AircraftService>();
            container.RegisterType<IApplication, Application>();
            var app = container.Resolve<Application>();
            app.Run();
        }
    }
}
