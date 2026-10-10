using LAB_2.Model.Characters;
using LAB_2.Model.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Decorators
{
    public class HealDecorator:CharacterDecorator
    {
        private readonly int _healAmount;
        private const int MaxAllowedHealth = 150;

        public HealDecorator(Character innerCharacter, int healAmount = 15) : base(innerCharacter)
        {
            _healAmount = healAmount;
            ApplyHealTick();
        }
        public void ApplyHealTick()
        {
            if (Health <= 0) return;

            int newHealth = _innerCharacter.Health + _healAmount;
            _innerCharacter.Health = Math.Min(newHealth, MaxAllowedHealth);
            Health = _innerCharacter.Health; 
        }
        protected override Character ApplySpecificDecorator(Character character)
        {
            return new HealDecorator(character, _healAmount);
        }
    }
}
