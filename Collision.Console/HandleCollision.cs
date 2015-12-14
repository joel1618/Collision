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
using Collision.Core.Models;
using Newtonsoft.Json;
using CoreConflict = Collision.Core.Models.Conflict;

namespace Collision.Console
{
    public class HandleCollision
    {
        private readonly IPositionService _positionService;
        private readonly IConflictService _conflictService;
        private Dictionary<int, Task> handleCollision = new Dictionary<int, Task>();

        public HandleCollision(IPositionService positionService, IConflictService conflictService)
        {
            _positionService = positionService;
            _conflictService = conflictService;
        }

        //Handle bounding box (pill) collisions
        public void HandleCollisions(int positionId)
        {
            var position1 = _positionService.Get(positionId);
            //Check preexisting collisions
            var collisions = _conflictService.GetByPositionId1(position1.Id);
            if(collisions != null)
            {
                foreach(var collision in collisions)
                {
                    HandleConflict(position1, collision.Position2);
                }
            }

            //TODO: Prune and sweep
            var positions = _positionService.GetAll();

            foreach (Position position2 in positions)
            {
                HandleConflict(position1, position2);
            }
        }

        public void HandleConflict(Position position1, Position position2)
        {
            //Find shortest distance between position1 xyz2 -> xyz1 line segment and position2 xyz2 -> xyz1 line segment.
            //https://www.youtube.com/watch?v=HC5YikQxwZA
            var distance = FindShortestDistanceBetweenLine(position1, position2);

            //If the distance is < position1.radius + position2.radius then we have a collision.
            if (distance < (double)(position1.Radius + position2.Radius))
            {
                //If collision found insert into another table with information about the collision (need to work out these details)
                var collisionExists = _conflictService.GetByPositionId1AndPositionId2(position1.Id, position2.Id);
                if (collisionExists == null)
                {
                    _conflictService.Create(new CoreConflict()
                    {
                        PositionId1 = position1.Id,
                        PositionId2 = position2.Id,
                        IsActive = true
                    });
                }
            }
            else
            {
                //Remove the collision from the database
                var collisionExists = _conflictService.GetByPositionId1AndPositionId2(position1.Id, position2.Id);
                if (collisionExists == null)
                {
                    _conflictService.Delete(collisionExists.Id);
                }
            }
        }

        //https://www.john.geek.nz/2009/03/code-shortest-distance-between-any-two-line-segments/
        private class point
        {
            public double x;
            public double y;
            public double z;
        }

        private double FindShortestDistanceBetweenLine(Position position1, Position position2)
        {
            double EPS = 0.00000001;

            point delta21 = new point();
            delta21.x = (double)(position1.X2 - position1.X1);
            delta21.y = (double)(position1.Y2 - position1.Y1);
            delta21.x = (double)(position1.Z2 - position1.Z1);

            point delta41 = new point();
            delta41.x = (double)(position2.X2 - position2.X1);
            delta41.y = (double)(position2.Y2 - position2.Y1);
            delta41.z = (double)(position2.Z2 - position2.Z1);

            point delta13 = new point();
            delta13.x = (double)(position1.X1 - position2.X1);
            delta13.y = (double)(position1.Y1 - position2.Y1);
            delta13.z = (double)(position1.Z1 - position2.Z1);

            double a = dot(delta21, delta21);
            double b = dot(delta21, delta41);
            double c = dot(delta41, delta41);
            double d = dot(delta21, delta13);
            double e = dot(delta41, delta13);
            double D = a * c - b * b;

            double sc, sN, sD = D;
            double tc, tN, tD = D;

            if (D < EPS)
            {
                sN = 0.0;
                sD = 1.0;
                tN = e;
                tD = c;
            }
            else
            {
                sN = (b * e - c * d);
                tN = (a * e - b * d);
                if (sN < 0.0)
                {
                    sN = 0.0;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD)
                {
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }

            if (tN < 0.0)
            {
                tN = 0.0;

                if (-d < 0.0)
                    sN = 0.0;
                else if (-d > a)
                    sN = sD;
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD)
            {
                tN = tD;
                if ((-d + b) < 0.0)
                    sN = 0;
                else if ((-d + b) > a)
                    sN = sD;
                else
                {
                    sN = (-d + b);
                    sD = a;
                }
            }

            if (Math.Abs(sN) < EPS) sc = 0.0;
            else sc = sN / sD;
            if (Math.Abs(tN) < EPS) tc = 0.0;
            else tc = tN / tD;

            point dP = new point();
            dP.x = delta13.x + (sc * delta21.x) - (tc * delta41.x);
            dP.y = delta13.y + (sc * delta21.y) - (tc * delta41.y);
            dP.z = delta13.z + (sc * delta21.z) - (tc * delta41.z);

            return Math.Sqrt(dot(dP, dP));
        }

        private double dot(point c1, point c2)
        {
            return (c1.x * c2.x + c1.y * c2.y + c1.z * c2.z);
        }

        private double norm(point c1)
        {
            return Math.Sqrt(dot(c1, c1));
        }
    }
}
