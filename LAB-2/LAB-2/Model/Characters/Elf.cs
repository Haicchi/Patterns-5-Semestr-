using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Characters
{
    public class Elf : Character
    {
        public int Dexterity { get; set; } = 15;
        
        public override char MapSymbol => 'E';

        public override void TakeDamage(int rawDamage)
        {
            
            int roll = Random.Shared.Next(1, 101);

            if (roll <= Dexterity)
            {
                Console.WriteLine($"   -> {Name} завдяки спритності (Dexterity: {Dexterity}%) блискавично ухиляється від атаки! Шкоди не отримано (HP: {Health})");
                return;
            }
            base.TakeDamage(rawDamage);
        }
        public override Character Clone() => (Elf)base.Clone();
    }
}
