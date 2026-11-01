using LAB_2.Enums;
using LAB_2.Interfaces;
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

        public bool TryExecuteAttack(IWarUnit attacker, IWarUnit target, int weaponDamage, int distance, int weaponRange, string weaponName)
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

            return true;
        }
    }
}