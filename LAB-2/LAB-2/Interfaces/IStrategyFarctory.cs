using LAB_2.Model.Movement;
using LAB_2.Model.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Interfaces
{
    public interface IStrategyFactory
    {
        IWeapon GetWeapon(WeaponType type);
        IMovement GetMovement(MovementType type);
    }
}
