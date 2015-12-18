using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Core.Models;

namespace Collision.Console
{
    public static class Helper
    {
        public static void NullifyPosition(Position position)
        {
            position.Latitude1 = new Nullable<decimal>();
            position.Longitude1 = new Nullable<decimal>();
            position.Speed1 = new Nullable<int>();
            position.Altitude1 = new Nullable<int>();
            position.Heading1 = new Nullable<int>();
            position.UtcTimeStamp1 = new Nullable<DateTime>();

            position.Latitude2 = new Nullable<decimal>();
            position.Longitude2 = new Nullable<decimal>();
            position.Speed2 = new Nullable<int>();
            position.Altitude2 = new Nullable<int>();
            position.Heading2 = new Nullable<int>();
            position.UtcTimeStamp2 = new Nullable<DateTime>();

            position.Latitude3 = new Nullable<decimal>();
            position.Longitude3 = new Nullable<decimal>();
            position.Speed3 = new Nullable<int>();
            position.Altitude3 = new Nullable<int>();
            position.Heading3 = new Nullable<int>();
            position.UtcTimeStamp3 = new Nullable<DateTime>();

            position.Radius = new Nullable<decimal>();

            position.X1 = new Nullable<decimal>();
            position.Y1 = new Nullable<decimal>();
            position.Z1 = new Nullable<int>();

            position.X2 = new Nullable<decimal>();
            position.Y2 = new Nullable<decimal>();
            position.Z2 = new Nullable<int>();

            position.X3 = new Nullable<decimal>();
            position.Y3 = new Nullable<decimal>();
            position.Z3 = new Nullable<int>();
        }
    }
}
