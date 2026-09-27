using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Character
{
    public class Warrior : Character
    {
        public int Armor { get; set; }
        [NotMapped]
        public override char MapSymbol => 'W';

        public override Character Clone()
        {
            var clone = (Warrior)base.Clone();
            return clone;
        }
    }
}
