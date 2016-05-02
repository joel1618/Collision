using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Sql.Ef.Models
{
    public class RadarFlight
    {
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public string Carrier { get; set; }
        public string CarrierName { get; set; }
        public string FlightNumber { get; set; }
        public double? Latitude2 { get; set; }
        public double? Longitude2 { get; set; }
        public double? Altitude2 { get; set; }
        public double? Speed2 { get; set; }
        public double? Heading2 { get; set; }
        public DateTime? CreatedAtUtcTimeStamp { get; set; }
        public DateTime? ModifiedAtUtcTimeStamp { get; set; }
        public bool IsConflict { get; set; }
    }
}
