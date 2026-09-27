using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Weapon
{
    public abstract class BaseWeapon : IWeapon
    {
        public abstract string Name { get;}

        public abstract int Damage {  get;}

        public abstract int Range {  get;}

        public virtual void Attack(int ax, int ay, int tx, int ty)
        {
            Console.WriteLine($" б'є зброєю {Name} на {Damage} шкоди!");
        }

        public virtual bool CanHit(int ax, int ay, int tx, int ty)
        {
            return Math.Max(Math.Abs(tx - ax), Math.Abs(ty - ay)) <= Range;
        }
    }
}
