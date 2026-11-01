using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Interfaces
{
    public interface IBattleConstants
    {
        public const int MaxAllowedHealth = 150;
        public const int DefaultHealAmount = 15;
        public const int DefaultArmorValue = 10;
        public const int DefaultRageBonusDamage = 10;

        public const int WarriorAuraDuration = 2;

        public const int WarriorAuraCooldown = 5;
        public const int DwarfAuraDuration = 3;
        public const int DwarfAuraCooldown = 6;
        public const int ElfAuraDuration = 3;
        public const int ElfAuraCooldown = 4;
    }
}
