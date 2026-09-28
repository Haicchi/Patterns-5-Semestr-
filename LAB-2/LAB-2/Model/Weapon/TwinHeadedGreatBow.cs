using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Weapon
{
    public class TwinHeadedGreatBow:BaseWeapon
    {
        public override string Name => "Великий Двоголовий Лук";
        public override int Damage => 20;

        public override int Range => 6;
    }
}
