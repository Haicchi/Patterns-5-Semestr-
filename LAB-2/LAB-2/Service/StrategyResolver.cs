using LAB_2.Interfaces;
using LAB_2.Model.Movement;
using LAB_2.Model.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Service
{
    public class StrategyResolver:IStrategyFactory
    {
        public IWeapon GetWeapon(WeaponType type) => type switch
        {
            WeaponType.Sword => new Sword(),
            WeaponType.Bow => new Bow(),
            WeaponType.Hammer => new Hammer(),
            WeaponType.BattleAxe => new BattleAxe(),
            WeaponType.SwiftDaggers => new SwiftDaggers(),
            WeaponType.TwinHeadedGreatBow => new TwinHeadedGreatBow(),
            WeaponType.MoonLightGreatSword => new MoonLightGreatSword(),
            WeaponType.IronKingHammer => new IronKingHammer(),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        public IMovement GetMovement(MovementType type) => type switch
        {
            MovementType.March => new MarchMovement(),
            MovementType.AgileSprint => new AgileMovement(),
            MovementType.HeavyMove => new HeavyMovement(),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
