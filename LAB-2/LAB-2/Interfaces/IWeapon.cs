using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Interfaces
{
    public interface IWeapon
    {
        string Name { get; }
        int Damage { get; }
        int Range { get; }
        bool CanHit(int ax, int ay, int tx, int ty);
        void Attack(int ax, int ay, int tx, int ty);
    }
}
