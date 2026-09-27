using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Character
{
    public class Elf : Character
    {
        public int Dexterity { get; set; }
        [NotMapped]
        public override char MapSymbol => 'E';

        public override Character Clone()
        {
            var clone = (Elf)base.Clone();
            return clone;
        }
    }
}
