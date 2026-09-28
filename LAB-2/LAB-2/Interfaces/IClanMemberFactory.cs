using LAB_2.Model.Characters;
using LAB_2.Model.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Interfaces
{
    public interface IClanMemberFactory
    {
        Character CreateCharacter();
        IWeapon CreateWeapon(WeaponType? preferredWeapon = null);
        IMovement CreateMovement();
        Character CreateEquippedPrototype(WeaponType? preferredWeapon = null);
    }
}
