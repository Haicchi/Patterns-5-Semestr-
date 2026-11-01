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
        int Health { get;}
        int X { get;}
        int Y { get;}
        bool IsAlive { get; }
        char MapSymbol { get; }
        void TakeDamage(int damage);
        IHandler? BuildDamagePipeline();
        IEnumerable<IWarUnit> GetFlattenedUnits();
        void MoveBy(int deltaX, int deltaY, int mapWidth, int mapHeight, IEnumerable<IWarUnit>? activeUnits = null);
    }
}
