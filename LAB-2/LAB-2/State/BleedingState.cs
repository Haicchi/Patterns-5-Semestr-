using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.State
{
    public class BleedingState : UnitState
    {
        private readonly int _damagePerTurn;

        public override string Name => "Кровотеча";
        public override int Duration { get; set; }

        public BleedingState(int duration = IBattleConstants.BleedDuration, int damagePerTurn = IBattleConstants.BleedDamage)
        {
            Duration = duration;
            _damagePerTurn = damagePerTurn;
        }

        public override bool CanTakeTurn() => true;

        public override void OnTurnStart()
        {
            _unit.Health -= _damagePerTurn;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[{_unit.Name}] стікає кров'ю та втрачає {_damagePerTurn} HP! (Залишилось HP: {_unit.Health})");
            Console.ResetColor();
        }

        public override void OnTurnEnd()
        {
            Duration--;
            if (Duration <= 0)
            {
                Console.WriteLine($"У [{_unit.Name}] зупинилася кровотеча.");
                _unit.TransitionTo(new NormalState());
            }
        }
    }
}
