using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.State
{
    public class StunnedState : UnitState
    {
        public override string Name => "Приголомшений";
        public override int Duration { get; set; }

        public StunnedState(int duration = IBattleConstants.StunDuration)
        {
            Duration = duration;
        }

        public override bool CanTakeTurn() => false;

        public override void OnTurnStart()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[{_unit.Name}] приголомшений і не може ходити в цьому раунді!");
            Console.ResetColor();
        }

        public override void OnTurnEnd()
        {
            Duration--;
            if (Duration <= 0)
            {
                Console.WriteLine($"[{_unit.Name}] оговтався від приголомшення.");
                _unit.TransitionTo(new NormalState());
            }
        }
    }
}
