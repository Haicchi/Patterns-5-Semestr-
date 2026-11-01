using LAB_2.Records;
using System;

namespace LAB_2.Handlers
{
    public class ArmorDamageHandler : DamageHandler
    {
        private int _armorValue;

        public ArmorDamageHandler(int armorValue)
        {
            _armorValue = armorValue;
        }

        protected override DamageContext ProcessDamage(DamageContext context)
        {
            if (context.CalculatedDamage <= 0) return context;

            int mitigated = Math.Max(1, context.CalculatedDamage - (_armorValue / 2));
            int absorbed = context.CalculatedDamage - mitigated;

            Console.WriteLine($"Броня поглинула {absorbed} од. шкоди!");

            return context with { CalculatedDamage = mitigated };
        }
    }
}