using LAB_2.Records;
using System;

namespace LAB_2.Handlers
{
    public class ToughnessDamageHandler : DamageHandler
    {
        private readonly double _reductionRatio;
        public ToughnessDamageHandler(double reductionRatio)
        {
            _reductionRatio = reductionRatio;
        }
        protected override DamageContext ProcessDamage(DamageContext context)
        {
            if (context.CalculatedDamage <= 0) return context;

            int reducedDamage = (int)Math.Round(context.CalculatedDamage * (1.0 - _reductionRatio));
            int blockedAmount = context.CalculatedDamage - reducedDamage;

            Console.WriteLine($"[{context.Target.Name}] завдяки стійкості заблокував {blockedAmount} од. шкоди!");

            return context with { CalculatedDamage = reducedDamage };
        }
    }
}