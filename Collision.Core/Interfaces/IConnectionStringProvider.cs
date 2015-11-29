using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Core.Interfaces
{
    public interface IConnectionStringProvider
    {
        string Name { get; }
        string ConnectionString { get; }
    }
}
