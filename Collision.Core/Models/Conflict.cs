using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Core.Models
{
    public class Conflict
    {
        public int Id { get; set; }
        public int PositionId1 { get; set; }
        public int PositionId2 { get; set; }
        public DateTime CreatedAtUtcTimeStamp { get; set; }
        public bool IsActive { get; set; }

        //Navigation Properties
        public Position Position1 { get; set; }
        public Position Position2 { get; set; }
    }
}
