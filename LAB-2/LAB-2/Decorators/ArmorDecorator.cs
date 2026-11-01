using LAB_2.Handlers;
using LAB_2.Interfaces;

namespace LAB_2.Decorators
{
    public class ArmorDecorator : CharacterDecorator
    {
        private readonly int _armorValue;

        public ArmorDecorator(IWarUnit innerUnit, int armorValue = IBattleConstants.DefaultArmorValue)
            : base(innerUnit)
        {
            _armorValue = armorValue;
        }

        public override string Name => $"{_innerUnit.Name} {IBattleLexems.ArmorSuffix}";

        protected override IHandler? GetDecoratorHandler()
        {
            return new ArmorDamageHandler(_armorValue);
        }
    }
}