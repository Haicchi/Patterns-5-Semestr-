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
    public static class StrategyResolver
    {
        public static IWeapon GetWeapon(WeaponType type) => type switch
        {
            WeaponType.Sword => new Sword(),
            WeaponType.Bow => new Bow(),
            WeaponType.Hammer => new Hammer(),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        public static IMovement GetMovement(MovementType type) => type switch
        {
            MovementType.March => new MarchMovement(),
            MovementType.AgileSprint => new AgileMovement(),
            MovementType.HeavyMove => new HeavyMovement(),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
