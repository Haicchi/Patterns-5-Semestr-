using LAB_2.Decorators;
using LAB_2.Model.Characters;

namespace LAB_2.Model.Decorators
{
    public class RageDecorator : CharacterDecorator
    {
        private readonly int _bonusDamage;

        public RageDecorator(Character innerCharacter, int bonusDamage = 5) : base(innerCharacter)
        {
            _bonusDamage = bonusDamage;
            Name = $"{innerCharacter.Name} [У люті]";
            Weapon = new RageWeaponDecorator(innerCharacter.Weapon, _bonusDamage);
        }

        protected override Character ApplySpecificDecorator(Character character)
        {
            return new RageDecorator(character, _bonusDamage);
        }
    }
}