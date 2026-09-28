using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Weapon
{
    public class MoonLightGreatSword:BaseWeapon
    {
        public override string Name => "Великий Меч Місячного Сяйва";
        public override int Damage => 35;

        public override int Range => 3;
    }
}
