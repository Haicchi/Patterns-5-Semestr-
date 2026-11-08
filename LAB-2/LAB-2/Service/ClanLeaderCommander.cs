using LAB_2.Interfaces;
using LAB_2.Model;
using LAB_2.Records;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LAB_2.Service
{
    public class ClanLeaderCommander
    {
        private List<Action<CommandContext, Action<CommandContext>>> _middlewareList;
        private Action<CommandContext> _pipeline;

        public ClanLeaderCommander()
        {
            _middlewareList = new List<Action<CommandContext, Action<CommandContext>>>
            {
                UnwrapSquadMiddleware,
                ExecuteUnitMiddleware
            };

            _pipeline = BuildPipeline();
        }

        public void IssueCommand(ICommand command, IWarUnit target, int mapWidth, int mapHeight)
        {
            var context = new CommandContext(command, target, mapWidth, mapHeight);
            _pipeline(context);
        }

        private Action<CommandContext> BuildPipeline()
        {
            Action<CommandContext> pipeline = ctx => { };

            for (int i = _middlewareList.Count - 1; i >= 0; i--)
            {
                var currentMiddleware = _middlewareList[i];
                var nextStep = pipeline;

                pipeline = ctx => currentMiddleware(ctx, nextStep);
            }

            return pipeline;
        }

        private static void UnwrapSquadMiddleware(CommandContext ctx, Action<CommandContext> next)
        {
            if (ctx.Target is Squad squad)
            {
                foreach (var unit in squad.GetFlattenedUnits().Where(u => u.IsAlive))
                {
                    next(ctx with { Target = unit });
                }
            }
            else
            {
                next(ctx);
            }
        }

        private static void ExecuteUnitMiddleware(CommandContext ctx, Action<CommandContext> next)
        {
            if (ctx.Target.IsAlive)
            {
                ctx.Command.Execute(ctx.Target, ctx.MapWidth, ctx.MapHeight);
            }

            next(ctx);
        }
    }
}