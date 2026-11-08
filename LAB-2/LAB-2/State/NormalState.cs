using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.State
{
    public class NormalState : UnitState
    {
        public override string Name => "Нормальний";
        public override int Duration { get; set; } = 0;

        public override bool CanTakeTurn() => true;
        public override void OnTurnStart() { }
        public override void OnTurnEnd() { }
    }
}
