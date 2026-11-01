using LAB_2.Interfaces;
using LAB_2.Records;
using System.Linq;

namespace LAB_2.Handlers
{
    public class SquadDamageHandler : DamageHandler
    {
        private readonly IWarUnit _squad;

        public SquadDamageHandler(IWarUnit squad)
        {
            _squad = squad;
        }

        protected override DamageContext ProcessDamage(DamageContext context)
        { 
            var targetUnit = _squad.GetFlattenedUnits().FirstOrDefault(u => u.IsAlive);

            if (targetUnit != null)
            { 
                targetUnit.TakeDamage(context.CalculatedDamage);
            }

            return context;
        }
    }
}