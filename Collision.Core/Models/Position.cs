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

        public decimal? Latitude1 { get; set; }
        public decimal? Longitude1 { get; set; }
        public int? Altitude1 { get; set; }
        public int? Speed1 { get; set; }
        public int? Heading1 { get; set; }
        public DateTime? UtcTimeStamp1 { get; set; }

        public decimal? Latitude2 { get; set; }
        public decimal? Longitude2 { get; set; }
        public int? Altitude2 { get; set; }
        public int? Speed2 { get; set; }
        public int? Heading2 { get; set; }
        public DateTime? UtcTimeStamp2 { get; set; }

        public decimal? Latitude3 { get; set; }
        public decimal? Longitude3 { get; set; }
        public int? Altitude3 { get; set; }
        public int? Speed3 { get; set; }
        public int? Heading3 { get; set; }
        public DateTime? UtcTimeStamp3 { get; set; }

        public decimal? Radius { get; set; }

        public decimal? X1 { get; set; }
        public decimal? Y1 { get; set; }
        public int? Z1 { get; set; }

        public decimal? X2 { get; set; }
        public decimal? Y2 { get; set; }
        public int? Z2 { get; set; }

        public decimal? X3 { get; set; }
        public decimal? Y3 { get; set; }
        public int? Z3 { get; set; }

        public DateTime CreatedAtUtcTimeStamp { get; set; }
        public DateTime? ModifiedAtUtcTimeStamp { get; set; }

        public bool IsInFlight { get; set; }
        public bool IsActive { get; set; }
    }
}
