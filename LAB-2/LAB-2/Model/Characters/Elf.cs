using LAB_2.Handlers;
using LAB_2.Interfaces;
using LAB_2.Model.Characters;

namespace LAB_2.Model.Characters
{
    public class Elf : Character
    {
        public int Dexterity { get; set; } = 15;

        public override char MapSymbol => 'E';
        public override IHandler? BuildDamagePipeline()
        {
            double evasionChance = Dexterity / 100.0;

            IHandler evasionHandler = new EvasionDamageHandler(evasionChance);
            IHandler healthHandler = new BaseHealthDamageHandler();
            evasionHandler.SetNext(healthHandler);
            return evasionHandler;
        }

        public override Character Clone() => (Elf)base.Clone();
    }
}