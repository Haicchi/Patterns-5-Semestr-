using LAB_2.Decorators;
using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using System;

namespace LAB_2.Model.Decorators
{
    public class RageDecorator : CharacterDecorator
    {
        private readonly int _bonusDamage;

        public RageDecorator(IWarUnit innerUnit, int bonusDamage = IBattleConstants.DefaultRageBonusDamage)
            : base(innerUnit)
        {
            _bonusDamage = bonusDamage;
            var baseCharacter = UnwrapToBaseCharacter(_innerUnit);
            if (baseCharacter != null && baseCharacter.Weapon != null)
            {
                baseCharacter.Weapon = new RageWeaponDecorator(baseCharacter.Weapon, _bonusDamage);
            }
        }
        public override string Name => $"{_innerUnit.Name} {IBattleLexems.RageSuffix}";

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