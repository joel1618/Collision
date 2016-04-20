using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Collision.Core.Models.FlightStats.Flight;
using PositionDomain = Collision.Core.Models.Position;

namespace Collision.Data.Repositories.Interfaces
{
    public interface IFlightStatsRepository
    {
        FlightDomain Get(PositionDomain item);
    }
}
