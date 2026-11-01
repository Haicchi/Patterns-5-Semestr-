using LAB_2.Model.Characters;
using LAB_2.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Handlers
{
    public class BaseHealthDamageHandler : DamageHandler
    {
        protected override DamageContext ProcessDamage(DamageContext context)
        {
            if (context.CalculatedDamage <= 0) return context;

            if (context.Target is Character character)
            {
                character.Health = Math.Max(0, character.Health - context.CalculatedDamage);
                Console.WriteLine($" [{character.Name}] отримав {context.CalculatedDamage} шкоди. Залишок HP: {character.Health}");
            }

            return context;
        }
    }
}
