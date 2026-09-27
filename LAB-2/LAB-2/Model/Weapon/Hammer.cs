using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Weapon
{
    public class Hammer:BaseWeapon
    {
        public override string Name => "Бойовий Молот";
        public override int Damage => 35;

        public override int Range => 1;

    }
}
