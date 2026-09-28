using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using LAB_2.Model.Movement;
using LAB_2.Model.Weapon;
using System;

namespace LAB_2.Factory
{
    public class DwarfFactory : IClanMemberFactory
    {
        private readonly IStrategyFactory _strategyFactory;

        private static readonly WeaponType[] DwarfWeapons = { WeaponType.Hammer,WeaponType.BattleAxe };

        public DwarfFactory(IStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
        }

        public Character CreateCharacter() => new Dwarf
        {
            Name = "Гном",
            Health = 140,
            Toughness = 15
        };

        public IWeapon CreateWeapon(WeaponType? preferredWeapon = null)
        {
            WeaponType type = preferredWeapon.HasValue && Array.Exists(DwarfWeapons, w => w == preferredWeapon.Value)
                ? preferredWeapon.Value
                : DwarfWeapons[Random.Shared.Next(DwarfWeapons.Length)];

            return _strategyFactory.GetWeapon(type);
        }

        public IMovement CreateMovement() =>
            _strategyFactory.GetMovement(MovementType.HeavyMove);

        public Character CreateEquippedPrototype(WeaponType? preferredWeapon = null)
        {
            var dwarf = CreateCharacter();
            dwarf.Weapon = CreateWeapon(preferredWeapon);
            dwarf.Movement = CreateMovement();
            return dwarf;
        }
    }
}