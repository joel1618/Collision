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
            //Handle bounding box (pill) collisions

            //Find any within a certain distance

            //Find shortest distance between position1 xyz2 -> xyz1 line segment and position2 xyz2 -> xyz1 line segment.
            
            //If the distance is < position1.radius + position2.radius then we have a collision.

            //If collision found insert into another table with information about the collision (need to work out these details)
        }
    }
}
