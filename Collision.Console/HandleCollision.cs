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
using Collision.Core.Models;
using Newtonsoft.Json;

namespace Collision.Console
{
    public class HandleCollision
    {
        private readonly IPositionService _positionService;
        private Dictionary<int, Task> handleCollision = new Dictionary<int, Task>();

        public HandleCollision(IPositionService positionService)
        {
            _positionService = positionService;
        }


        public void HandleCollisions(int PositionId)
        {
            //Handle bounding box collisions

            //If found insert into another table with information about the collision (need to work out these details)
        }
    }
}
