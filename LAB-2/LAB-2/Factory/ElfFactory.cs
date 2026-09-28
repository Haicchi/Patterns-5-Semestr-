using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using LAB_2.Model.Movement;
using LAB_2.Model.Weapon;
using System;

namespace LAB_2.Factory
{
    public class ElfFactory : IClanMemberFactory
    {
        private readonly IStrategyFactory _strategyFactory;
        private static readonly WeaponType[] ElfWeapons =
        {
            WeaponType.Bow,
            WeaponType.SwiftDaggers
        };

        public ElfFactory(IStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
        }

        public Character CreateCharacter() => new Elf
        {
            Name = "Ельф",
            Health = 100,
            Dexterity = 15
        };

        public IWeapon CreateWeapon(WeaponType? preferredWeapon = null)
        {
            WeaponType type = preferredWeapon.HasValue && Array.Exists(ElfWeapons, w => w == preferredWeapon.Value)
                ? preferredWeapon.Value
                : ElfWeapons[Random.Shared.Next(ElfWeapons.Length)];

            return _strategyFactory.GetWeapon(type);
        }

        public IMovement CreateMovement() =>
            _strategyFactory.GetMovement(MovementType.AgileSprint);

        public Character CreateEquippedPrototype(WeaponType? preferredWeapon = null)
        {
            var elf = CreateCharacter();
            elf.Weapon = CreateWeapon(preferredWeapon);
            elf.Movement = CreateMovement();
            return elf;
        }
    }
}