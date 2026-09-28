using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Characters
{
    public class Dwarf : Character
    {
        public int Toughness { get; set; } = 20;

        public override char MapSymbol => 'D';

        public override void TakeDamage(int rawDamage)
        {
         
            double reductionMultiplier = 1.0 - (Toughness / 100.0);
            int reducedDamage = (int)Math.Round(rawDamage * reductionMultiplier);

            Health -= reducedDamage;
            if (Health < 0) Health = 0;

            Console.WriteLine($"   -> {Name} завдяки стійкості (Toughness: {Toughness}%) блокує частину удару й отримує {reducedDamage} шкоди замість {rawDamage}! (Залишилось HP: {Health})");
        }

        public override Character Clone() => (Dwarf)base.Clone();
    }
}