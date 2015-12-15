using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collision.Core.Models.Mock;

namespace Collision.Console
{
    public class MockData
    {
        private decimal minLatitude = 0, maxLatitude = 0;
        private decimal minLongitude = 0, maxLongitude = 0;
        private decimal minAltitude = 0, maxAltitude = 0;
        private decimal minSpeed = 0, maxSpeed = 0;
        private decimal minHeading = 0, maxHeading = 0;
        public MockData()
        {

        }

        public Flight Get()
        {
            Flight flight = new Flight();
            Random random = new Random();

            
            int randomNumber = random.Next(0, 100);
            return flight;
        }
    }
}
