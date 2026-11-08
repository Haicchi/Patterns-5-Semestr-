using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.State
{
    public class PoisonedState : UnitState
    {
        private readonly int _damagePerTurn;

        public override string Name => "Отруєння";
        public override int Duration { get; set; }

        public PoisonedState(int duration = IBattleConstants.PoisonDuration, int damagePerTurn = IBattleConstants.PoisonDamage)
        {
            Duration = duration;
            _damagePerTurn = damagePerTurn;
        }

        public override bool CanTakeTurn() => true;

        public override void OnTurnStart()
        {
            _unit.Health -= _damagePerTurn;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{_unit.Name}] страждає від отрути та втрачає {_damagePerTurn} HP! (Залишилось HP: {_unit.Health})");
            Console.ResetColor();
        }

        public override void OnTurnEnd()
        {
            Duration--;
            if (Duration <= 0)
            {
                Console.WriteLine($"Дія отрути на [{_unit.Name}] закінчилася.");
                _unit.TransitionTo(new NormalState());
            }
        }
    }
}
