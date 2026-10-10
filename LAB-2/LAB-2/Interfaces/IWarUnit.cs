using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Interfaces
{
    public interface IWarUnit
    {
        string Name { get; }
        int Health { get; set; }
        int X { get; set; }
        int Y { get; set; }
        bool IsAlive { get; }
        void TakeDamage(int damage);
        IEnumerable<IWarUnit> GetFlattenedUnits();
    }
}
