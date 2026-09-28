using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Weapon
{
    public class IronKingHammer:BaseWeapon
    {
        public override string Name => "Молот Залізного Короля";
        public override int Damage => 45;

        public override int Range => 1;
    }
}
