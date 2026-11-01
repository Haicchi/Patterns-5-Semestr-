using LAB_2.Handlers;
using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Service
{
    public class ClanLeaderCommander
    {
        private readonly ICommandHandler _pipeline;

        public ClanLeaderCommander()
        {
            var squadHandler = new SquadCommandHandler();
            squadHandler.SetNext(new UnitExecutionHandler());
            _pipeline = squadHandler;
        }

        public void IssueCommand(ICommand command, IWarUnit target, int mapWidth, int mapHeight)
        {
            _pipeline.RouteCommand(command, target, mapWidth, mapHeight);
        }
    }
}
