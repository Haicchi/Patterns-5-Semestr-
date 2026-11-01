using LAB_2.Decorators;
using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using LAB_2.Service;
using System;

namespace LAB_2.Commands
{
    public class AttackCommand : ICommand
    {
        private readonly IWarUnit _targetEnemy;
        private readonly CombatService _combatService;

        public string Name => "Битися (Атака)";

        public AttackCommand(IWarUnit targetEnemy, CombatService combatService)
        {
            _targetEnemy = targetEnemy;
            _combatService = combatService;
        }

        public void Execute(IWarUnit attacker, int mapWidth, int mapHeight)
        {
            if (!attacker.IsAlive || _targetEnemy == null || !_targetEnemy.IsAlive) return;

            int distance = Math.Max(Math.Abs(attacker.X - _targetEnemy.X), Math.Abs(attacker.Y - _targetEnemy.Y));
            var baseAttacker = UnwrapToBaseCharacter(attacker);

            int weaponRange = baseAttacker?.Weapon?.Range ?? 1;
            int weaponDamage = baseAttacker?.Weapon?.Damage ?? 10;
            string weaponName = baseAttacker?.Weapon?.Name ?? "Рукопашна";

            bool attacked = _combatService.TryExecuteAttack(
                attacker,
                _targetEnemy,
                weaponDamage,
                distance,
                weaponRange,
                weaponName
            );

            if (!attacked)
            {
                Console.WriteLine($"[{attacker.MapSymbol}] {attacker.Name} не може дістати [{_targetEnemy.MapSymbol}] {_targetEnemy.Name} (Дистанція: {distance}, Дальність: {weaponRange})!");
            }
        }

        private static Character? UnwrapToBaseCharacter(IWarUnit unit)
        {
            while (unit is CharacterDecorator decorator)
            {
                unit = decorator.InnerUnit;
            }
            return unit as Character;
        }
    }
}