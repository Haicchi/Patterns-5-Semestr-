using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Character
{
    public class Dwarf : Character
    {
        public int MagicResist { get; set; }
        [NotMapped]
        public override char MapSymbol => 'D';

        public override Character Clone()
        {
            var clone = (Dwarf)base.Clone();
            return clone;
        }
    }
}
