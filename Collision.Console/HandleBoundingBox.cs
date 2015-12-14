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
    public class HandleBoundingBox
    {
        public void CalculateBoundingBox(Position position)
        {
            if (ValidateCanCalculate(position))
            {
                System.Console.WriteLine("Calculating bounding box for " + position.Aircraft.CarrierName + " flight " + position.Aircraft.FlightNumber);
                ConvertLatLonAltToXYZ(position);
                CalculateXYZ1(position);
                //TODO: Find the lat/lon/alt from x1/y1/z1
                //ConvertXYZ1toLatLonAlt1(position);
            }
        }

        //Here we are going to use vector linear algebra.
        //http://math.stackexchange.com/questions/83404/finding-a-point-along-a-line-in-three-dimensions-given-two-points
        public void CalculateXYZ1(Position position)
        {
            var productX = (double)(position.X3.Value - position.X2.Value);
            var productY = (double)(position.Y3.Value - position.Y2.Value);
            var productZ = (double)(position.Z3.Value - position.Z2.Value);

            var normalizedTotal = Math.Sqrt(productX * productX + productY * productY + productZ * productZ);

            var unitVectorX = productX / normalizedTotal;
            var unitVectorY = productY / normalizedTotal;
            var unitVectorZ = productZ / normalizedTotal;

            //distance traveled at current speed for 60 seconds (can make this dynamic later)
            var distance = position.Speed2.Value / 60;

            //Can now use this unit vector to find X1, Y1, Z1 
            position.X1 =  (decimal)(((double)position.X2.Value) + (double)distance * unitVectorX);
            position.Y1 = (decimal)(((double)position.Y2.Value) + (double)distance * unitVectorY);
            position.Z1 = (decimal)(((double)position.Z2.Value) + (double)distance * unitVectorZ);
        }

        //Taken from here https://github.com/substack/geodetic-to-ecef/blob/master/index.js
        public void ConvertLatLonAltToXYZ(Position position)
        {
            var a = 6378137; // wgs84.RADIUS;  equitorial radius (semi-major axis)
            var f = 1 / 298.257223563;// wgs84.FLATTENING;
            var e2 = (2 - f) * f; // first eccentricity squared

            //Position 1

            //Position 2
            var h2 = position.Altitude2 * 1000;
            var rlat2 = (double)position.Latitude2.Value / 180 * Math.PI;
            var rlon2 = (double)position.Longitude2.Value / 180 * Math.PI;

            var slat2 = Math.Sin(rlat2);
            var clat2 = Math.Cos(rlat2);

            var N2 = a / Math.Sqrt(1 - e2 * slat2 * slat2);

            position.X2 = (decimal)((N2 + (double)h2) * clat2 * Math.Cos(rlon2)) / 1000;
            position.Y2 = (decimal)((N2 + (double)h2) * clat2 * Math.Sin(rlon2)) / 1000;
            position.Z2 = (decimal)((N2 * (1 - e2) + (double)h2) * slat2) / 1000;

            //Position 3
            var h3 = position.Altitude3 * 1000;
            var rlat3 = (double)position.Latitude3.Value / 180 * Math.PI;
            var rlon3 = (double)position.Longitude3.Value / 180 * Math.PI;

            var slat3 = Math.Sin(rlat3);
            var clat3 = Math.Cos(rlat3);

            var N3 = a / Math.Sqrt(1 - e2 * slat3 * slat3);

            position.X3 = (decimal)((N3 + (double)h3) * clat3 * Math.Cos(rlon3)) / 1000;
            position.Y3 = (decimal)((N3 + (double)h3) * clat3 * Math.Sin(rlon3)) / 1000;
            position.Z3 = (decimal)((N3 * (1 - e2) + (double)h3) * slat3) / 1000;
        }

        //Make sure all necessary fields are available to calculate the bounds of the pill.
        private bool ValidateCanCalculate(Position position)
        {
            if(position == null)
            {
                return false;
            }
            if (!position.Latitude2.HasValue || !position.Longitude2.HasValue || !position.Altitude2.HasValue ||
                !position.Latitude3.HasValue || !position.Longitude3.HasValue || !position.Altitude3.HasValue){
                return false;
            }
            return true;
        }
    }
}
