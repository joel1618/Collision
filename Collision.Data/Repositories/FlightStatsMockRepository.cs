using Collision.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Core.Models.FlightStats;
using FlightDomain = Collision.Core.Models.FlightStats.Flight;
using PositionDomain = Collision.Core.Models.FlightStats.Position;

namespace Collision.Data.Repositories
{
    public class FlightStatsMockRepository : IFlightStatsMockRepository
    {
        //all mins and max's have been adjusted for their inverse min-max
        //-90 to 90
        private int minLatitude = -88, maxLatitude = 88;
        //-180 to 180
        private int minLongitude = -178, maxLongitude = 178;
        //0 to 30000
        private int minAltitude = 1000, maxAltitude = 29000;
        //0 to 1000
        private int minSpeed = 100, maxSpeed = 900;
        //Unused
        private int minHeading = 0, maxHeading = 360;
        Flight flight = null;

        public FlightStatsMockRepository()
        {
            flight = new Flight();

            flight.flightTracks = new List<FlightTrack>();
            FlightTrack flightTrack = new FlightTrack();
            flight.flightTracks.Add(flightTrack);

            flight.appendix = new Appendix();
            flight.appendix.airlines = new List<Airline>();
            Airline airline = new Airline();
            flight.appendix.airlines.Add(airline);

            flightTrack.positions = new List<PositionDomain>();
            PositionDomain position1 = new PositionDomain();
            flightTrack.positions.Add(position1);
            PositionDomain position2 = new PositionDomain();
            flightTrack.positions.Add(position2);
        }

        //TODO: Move not ranomdly but incrementally
        public FlightDomain Get()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            flight.appendix.airlines[0].active = true;

            var now = DateTime.UtcNow;

            flight.flightTracks[0].positions[1].lat = random.Next(minLatitude, maxLatitude);
            flight.flightTracks[0].positions[1].lon = random.Next(minLongitude, maxLongitude);
            flight.flightTracks[0].positions[1].altitudeFt = random.Next(minAltitude, maxAltitude);
            flight.flightTracks[0].positions[1].speedMph = random.Next(minSpeed, maxSpeed);
            flight.flightTracks[0].heading = random.Next(minHeading, maxHeading);
            flight.flightTracks[0].positions[1].date = now.AddSeconds(random.Next(-90, -30));

            random = new Random(Guid.NewGuid().GetHashCode());
            var isPositive = random.Next(0, 1);
            if(isPositive == 1)
            {
                flight.flightTracks[0].positions[0].lat = random.Next(
                int.Parse(flight.flightTracks[0].positions[1].lat.ToString()) + 1,
                int.Parse(flight.flightTracks[0].positions[1].lat.ToString()) + 2);
            }
            else
            {
                flight.flightTracks[0].positions[0].lat = random.Next(
                int.Parse(flight.flightTracks[0].positions[1].lat.ToString()) - 2,
                int.Parse(flight.flightTracks[0].positions[1].lat.ToString()) - 1);
            }

            isPositive = random.Next(0, 1);
            if (isPositive == 1)
            {
                flight.flightTracks[0].positions[0].lon = random.Next(
                int.Parse(flight.flightTracks[0].positions[1].lon.ToString()) + 1,
                int.Parse(flight.flightTracks[0].positions[1].lon.ToString()) + 2);
            }
            else
            {
                flight.flightTracks[0].positions[0].lon = random.Next(
               int.Parse(flight.flightTracks[0].positions[1].lon.ToString()) - 2,
               int.Parse(flight.flightTracks[0].positions[1].lon.ToString()) - 1);
            }

            isPositive = random.Next(0, 1);
            if (isPositive == 1)
            {
                flight.flightTracks[0].positions[0].altitudeFt = random.Next(
                int.Parse(flight.flightTracks[0].positions[1].altitudeFt.ToString()) + 100,
                int.Parse(flight.flightTracks[0].positions[1].altitudeFt.ToString()) + 1000);
            }
            else
            {
                flight.flightTracks[0].positions[0].altitudeFt = random.Next(
                int.Parse(flight.flightTracks[0].positions[1].altitudeFt.ToString()) - 1000,
                int.Parse(flight.flightTracks[0].positions[1].altitudeFt.ToString()) + 100);
            }

            isPositive = random.Next(0, 1);
            if (isPositive == 1)
            {
                flight.flightTracks[0].positions[0].speedMph = random.Next(
                int.Parse(flight.flightTracks[0].positions[1].speedMph.ToString()) + 0,
                int.Parse(flight.flightTracks[0].positions[1].speedMph.ToString()) + 100);
            }
            else
            {
                flight.flightTracks[0].positions[0].speedMph = random.Next(
                int.Parse(flight.flightTracks[0].positions[1].speedMph.ToString()) - 100,
                int.Parse(flight.flightTracks[0].positions[1].speedMph.ToString()) - 0);
            }
            flight.flightTracks[0].heading = flight.flightTracks[0].heading;
            flight.flightTracks[0].positions[0].date = now;
            return flight;
        }
    }
}
