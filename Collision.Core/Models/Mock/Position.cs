using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Core.Models.Mock
{
    public class Position
    {
        public decimal? lat { get; set; }
        public decimal? lon { get; set; }
        public decimal? speedMph { get; set; }
        public decimal? altitudeFt { get; set; }
        public decimal? heading { get; set; }
        public DateTime? date { get; set; }
    }
}
