﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Core.Models
{
    public class Position
    {
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public string Name { get; set; }
        public string FlightId { get; set; }
        public string FlightIdentity { get; set; }

        public decimal Temp1Latitude { get; set; }
        public decimal Temp1Longitude { get; set; }
        public int Temp1Altitude { get; set; }
        public int Temp1Speed { get; set; }
        public int? Temp1Heading { get; set; }
        public DateTime Temp1UtcTimeStamp { get; set; }

        public decimal? Temp2Latitude { get; set; }
        public decimal? Temp2Longitude { get; set; }
        public int? Temp2Altitude { get; set; }
        public int? Temp2Speed { get; set; }
        public int? Temp2Heading { get; set; }
        public DateTime? Temp2UtcTimeStamp { get; set; }

        public decimal? X1 { get; set; }
        public decimal? Y1 { get; set; }
        public int? Z1 { get; set; }

        public decimal? X2 { get; set; }
        public decimal? Y2 { get; set; }
        public int? Z2 { get; set; }

        public decimal? X3 { get; set; }
        public decimal? Y3 { get; set; }
        public int? Z3 { get; set; }

        public decimal? X4 { get; set; }
        public decimal? Y4 { get; set; }
        public int? Z4 { get; set; }

        public decimal? X5 { get; set; }
        public decimal? Y5 { get; set; }
        public int? Z5 { get; set; }

        public decimal? X6 { get; set; }
        public decimal? Y6 { get; set; }
        public int? Z6 { get; set; }

        public decimal? X7 { get; set; }
        public decimal? Y7 { get; set; }
        public int? Z7 { get; set; }

        public decimal? X8 { get; set; }
        public decimal? Y8 { get; set; }
        public int? Z8 { get; set; }

        public DateTime CreatedAtUtcTimeStamp { get; set; }

        public bool IsInFlight { get; set; }
        public bool IsActive { get; set; }
    }
}
