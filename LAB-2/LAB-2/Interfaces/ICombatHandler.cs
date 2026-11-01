using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Interfaces
{
    public interface ICommandHandler
    {
        ICommandHandler SetNext(ICommandHandler nextHandler);
        void RouteCommand(ICommand command, IWarUnit targetUnit, int mapWidth, int mapHeight);
    }
}
