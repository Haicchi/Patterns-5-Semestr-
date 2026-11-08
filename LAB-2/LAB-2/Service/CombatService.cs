using LAB_2.Decorators;
using LAB_2.Enums;
using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using LAB_2.Model.Weapon;
using LAB_2.State;
using System;

namespace LAB_2.Service
{
    public class CombatService
    {
        private readonly IBattleMediator? _mediator;

        public CombatService(IBattleMediator? mediator = null)
        {
            _mediator = mediator;
        }

        public bool TryExecuteAttack(
            IWarUnit attacker,
            IWarUnit target,
            int weaponDamage,
            int distance,
            int weaponRange,
            string weaponName,
            WeaponType weaponType)
        {
            if (distance > weaponRange) return false;

            int finalDamage = Math.Max(1, weaponDamage);
            target.TakeDamage(finalDamage);

            Console.WriteLine($"[{attacker.MapSymbol}] {attacker.Name} завдає {finalDamage} шкоди [{target.MapSymbol}] {target.Name} ({weaponName})");

            if (!target.IsAlive)
            {
                Console.WriteLine($"[{target.MapSymbol}] {target.Name} поліг у бою!");
                _mediator?.Notify(this, BattleEvent.UnitDied, target);
            }
            else
            {
                var targetChar = Unwrap(target);
                if (targetChar != null)
                {
                    if (Random.Shared.Next(100) < 15)
                    {
                        ApplyStatusByWeaponType(targetChar, weaponType);
                    }
                }
            }

            return true;
        }

        private static void ApplyStatusByWeaponType(Character target, WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Sword:
                case WeaponType.SwiftDaggers:
                case WeaponType.MoonLightGreatSword:
                    target.TransitionTo(new BleedingState(duration: 2, damagePerTurn: 10));
                    break;
                case WeaponType.Bow:
                case WeaponType.TwinHeadedGreatBow:
                    target.TransitionTo(new PoisonedState(duration: 3, damagePerTurn: 8));
                    break;
                case WeaponType.Hammer:
                case WeaponType.IronKingHammer:
                case WeaponType.BattleAxe:
                    target.TransitionTo(new StunnedState(duration: 1));
                    break;
            }
        }

        private static Character? Unwrap(IWarUnit u)
        {
            while (u is CharacterDecorator d) u = d.InnerUnit;
            return u as Character;
        }
    }
}