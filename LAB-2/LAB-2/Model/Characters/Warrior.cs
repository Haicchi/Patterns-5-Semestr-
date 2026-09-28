using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Characters
{
    public class Warrior : Character
    {
       
     
        public override char MapSymbol => 'W';

        public override Character Clone() => (Warrior)base.Clone();
    }
}
