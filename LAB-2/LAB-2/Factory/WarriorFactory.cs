using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using LAB_2.Model.Movement;
using LAB_2.Model.Weapon;
using System;

namespace LAB_2.Factory
{
    public class WarriorFactory : IClanMemberFactory
    {
        private readonly IStrategyFactory _strategyFactory;
        private static readonly WeaponType[] WarriorWeapons =
        {
            WeaponType.Hammer,
            WeaponType.BattleAxe,
            WeaponType.Sword,
            WeaponType.Bow,
            WeaponType.SwiftDaggers
        };
        private static readonly MovementType[] AllMovements = Enum.GetValues<MovementType>();

        public WarriorFactory(IStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
        }

        public Character CreateCharacter() => new Warrior
        {
            Name = "Воїн",
            Health = 120
        };

        public IWeapon CreateWeapon(WeaponType? preferredWeapon = null)
        {
            var type = preferredWeapon ?? WarriorWeapons[Random.Shared.Next(WarriorWeapons.Length)];
            return _strategyFactory.GetWeapon(type);
        }

        public IMovement CreateMovement()
        {
            var type = AllMovements[Random.Shared.Next(AllMovements.Length)];
            return _strategyFactory.GetMovement(type);
        }

        public Character CreateEquippedPrototype(WeaponType? preferredWeapon = null)
        {
            var warrior = CreateCharacter();
            warrior.Weapon = CreateWeapon(preferredWeapon);
            warrior.Movement = CreateMovement();
            return warrior;
        }
    }
}