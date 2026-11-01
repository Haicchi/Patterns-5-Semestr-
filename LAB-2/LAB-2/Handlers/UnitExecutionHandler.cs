using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Handlers
{
    public class UnitExecutionHandler : BaseCommandHandler
    {
        public override void RouteCommand(ICommand command, IWarUnit targetUnit, int mapWidth, int mapHeight)
        {
            if (!targetUnit.IsAlive) return;

            command.Execute(targetUnit, mapWidth, mapHeight);
        }
    }
}
