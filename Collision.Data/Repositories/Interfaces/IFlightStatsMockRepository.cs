using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Collision.Core.Models.FlightStats.Flight;

namespace Collision.Data.Repositories.Interfaces
{
    public interface IFlightStatsMockRepository
    {
        FlightDomain Get();
    }
}
