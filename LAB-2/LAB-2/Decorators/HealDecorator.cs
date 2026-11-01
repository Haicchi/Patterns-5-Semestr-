using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using System;

namespace LAB_2.Decorators
{
    public class HealDecorator : CharacterDecorator
    {
        private int _healAmount;

        public HealDecorator(IWarUnit innerUnit, int healAmount = IBattleConstants.DefaultHealAmount)
            : base(innerUnit)
        {
            _healAmount = healAmount;
            ApplyHealTick();
        }

        public override string Name => $"{_innerUnit.Name} {IBattleLexems.HealSuffix}";

        public void ApplyHealTick()
        {
            if (_innerUnit.Health <= 0) return;

            var baseCharacter = UnwrapToBaseCharacter(_innerUnit);
            if (baseCharacter != null)
            {
                baseCharacter.Health = Math.Min(baseCharacter.Health + _healAmount, IBattleConstants.MaxAllowedHealth);
                Console.WriteLine($"[{Name}] відновлює {_healAmount} HP! (Поточне HP: {Health})");
            }
        }

        protected override IHandler? GetDecoratorHandler()
        {
            return null;
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