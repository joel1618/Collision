﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreAircraft = Collision.Core.Models.Aircraft;

namespace Collision.Data.Repositories.Interfaces
{
    public interface IAircraftRepository : IRepository<CoreAircraft>
    {
    }
}