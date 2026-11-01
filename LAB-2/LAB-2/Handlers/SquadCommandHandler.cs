using LAB_2.Interfaces;
using LAB_2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Handlers
{
    public class SquadCommandHandler : BaseCommandHandler
    {
        public override void RouteCommand(ICommand command, IWarUnit targetUnit, int mapWidth, int mapHeight)
        {
            if (!targetUnit.IsAlive) return;

            if (targetUnit is Squad squad)
            {
                Console.WriteLine($"Загін «{squad.Name}» прийняв наказ Глави: «{command.Name}»");

                foreach (var unit in squad.GetFlattenedUnits())
                {
                    base.RouteCommand(command, unit, mapWidth, mapHeight);
                }
                return;
            }

            base.RouteCommand(command, targetUnit, mapWidth, mapHeight);
        }
    }
}
