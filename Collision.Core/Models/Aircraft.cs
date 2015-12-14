﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Core.Models
{
    public class Aircraft
    {
        public int Id { get; set; }
        public string Carrier { get; set; }
        public string CarrierName { get; set; }
        public string FlightNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
