using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Decorators
{
    public abstract class CharacterDecorator : Character
    {
        protected readonly Character _innerCharacter;

        protected CharacterDecorator(Character innerCharacter)
        {
            _innerCharacter = innerCharacter;
            Weapon = innerCharacter.Weapon;
            Movement = innerCharacter.Movement;
            Name = innerCharacter.Name;
            Health = innerCharacter.Health;
            X = innerCharacter.X;
            Y = innerCharacter.Y;
        }
        public override char MapSymbol => _innerCharacter.MapSymbol;
        public Character InnerCharacter => _innerCharacter;

        public override void TakeDamage(int damage)
        {
            _innerCharacter.TakeDamage(damage);
            Health = _innerCharacter.Health;
        }

        public override Character Clone()
        {
            var clonedInner = _innerCharacter.Clone();
            return ApplySpecificDecorator(clonedInner);
        }

        protected abstract Character ApplySpecificDecorator(Character character);
    }
}
