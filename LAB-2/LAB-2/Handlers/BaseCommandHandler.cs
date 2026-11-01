using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Handlers
{
    public abstract class BaseCommandHandler : ICommandHandler
    {
        protected ICommandHandler? _next;

        public ICommandHandler SetNext(ICommandHandler nextHandler)
        {
            _next = nextHandler;
            return nextHandler;
        }

        public virtual void RouteCommand(ICommand command, IWarUnit targetUnit, int mapWidth, int mapHeight)
        {
            _next?.RouteCommand(command, targetUnit, mapWidth, mapHeight);
        }
    }
}
