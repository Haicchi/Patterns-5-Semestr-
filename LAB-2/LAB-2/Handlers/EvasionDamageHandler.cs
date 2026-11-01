using LAB_2.Records;
using System;

namespace LAB_2.Handlers
{
    public class EvasionDamageHandler : DamageHandler
    {
        private double _evasionChance;
        private static Random _random = new();
        public EvasionDamageHandler(double evasionChance)
        {
            _evasionChance = evasionChance;
        }

        protected override DamageContext ProcessDamage(DamageContext context)
        {
            if (_random.NextDouble() < _evasionChance)
            {
                Console.WriteLine($"[{context.Target.Name}] спритно ухилився від атаки!");
                return context with { IsBlocked = true, CalculatedDamage = 0 };
            }

            return context;
        }
    }
}