using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Core.Models.Mock
{
    //This is the model being rturned from flightstats.com which is being used to gather the positional information
    public class Flight
    {
        public List<FlightTrack> flightTracks { get; set; }
        public Appendix appendix { get; set; }
        public Error error { get; set; }
    }

    public class FlightTrack
    {
        public List<Position> positions { get; set; }
        public double heading { get; set; }
    }

    public class Position
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public double speedMph { get; set; }
        public double altitudeFt { get; set; }
        public DateTime date { get; set; }
    }

    public class Appendix
    {
        public List<Airline> airlines { get; set; }
    }

    public class Airline
    {
        public bool active { get; set; }
    }

    public class Error
    {
        
    }
}
