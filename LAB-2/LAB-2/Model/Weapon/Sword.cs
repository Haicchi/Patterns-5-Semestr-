using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Weapon
{
    public class Sword:BaseWeapon
    {
        public override string Name => "Меч Лицара";
        public override int Damage => 25;

        public override int Range => 2;
    }
}
