using LAB_2.Handlers;
using LAB_2.Interfaces;

namespace LAB_2.Model.Characters
{
    public class Dwarf : Character
    {
        public int Toughness { get; set; } = 20;

        public override char MapSymbol => 'D';

        public override IHandler BuildDamagePipeline()
        {
            double reductionRatio = Toughness / 100.0;

            IHandler toughnessHandler = new ToughnessDamageHandler(reductionRatio);
            IHandler healthHandler = new BaseHealthDamageHandler();
            toughnessHandler.SetNext(healthHandler);
            return toughnessHandler;
        }

        public override Character Clone() => (Dwarf)base.Clone();
    }
}