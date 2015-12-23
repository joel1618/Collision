﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreConflict = Collision.Core.Models.Conflict;

namespace Collision.Sql.Ef.Repositories.Interfaces
{
    public interface IConflictRepository : IRepository<CoreConflict>
    {
        IEnumerable<CoreConflict> GetByPositionId1(int positionId1);
        CoreConflict GetByPositionId1AndPositionId2(int positionId1, int positionId2);
        IEnumerable<CoreConflict> GetByPositionId1OrPositionId2(int positionId);
    }
}