using LAB_2.Decorators;
using LAB_2.Model.Characters;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LAB_2.Model.Decorators
{
    public class ArmorDecorator : CharacterDecorator
    {
        private readonly int _armorValue;

        public ArmorDecorator(Character innerCharacter, int armorValue = 10) : base(innerCharacter)
        {
            _armorValue = armorValue;
            Name = $"{innerCharacter.Name} [У латах]";
            Health += _armorValue;
        }

        public override void TakeDamage(int damage)
        {
            int mitigated = Math.Max(1, damage - (_armorValue / 2));
            _innerCharacter.TakeDamage(mitigated);
            Health = _innerCharacter.Health;
        }

        protected override Character ApplySpecificDecorator(Character character)
        {
            return new ArmorDecorator(character, _armorValue);
        }
    }
}
                    
            