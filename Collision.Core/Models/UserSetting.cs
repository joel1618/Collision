using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Core.Models
{
    public class UserSetting
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string DistanceOfSearch { get; set; }
        public bool IsDistanceOfSearchInKilometers { get; set; }
    }
}
