using LAB_2.Interfaces;
using LAB_2.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LAB_2.Service
{
    public class LeaderTacticsManager
    {
        public void SyncBodyguardsWithLeader(IWarUnit leader, Squad bodyguardSquad, int mapWidth, int mapHeight)
        {
            if (!bodyguardSquad.IsAlive || !leader.IsAlive) return;

            int offsetX = -1;
            foreach (var unit in bodyguardSquad.GetFlattenedUnits())
            {
                if (!unit.IsAlive) continue;
                int targetX = Math.Clamp(leader.X + offsetX, 0, mapWidth - 1);
                int targetY = Math.Clamp(leader.Y, 0, mapHeight - 1);
                int deltaX = targetX - unit.X;
                int deltaY = targetY - unit.Y;

                unit.MoveBy(deltaX, deltaY, mapWidth, mapHeight);

                offsetX++;
                if (offsetX == 0) offsetX = 1;
            }
        }

        public void ProcessAssaultSquadTactics(Squad assaultSquad, List<IWarUnit> enemyUnits, int mapWidth, int mapHeight)
        {
            if (!assaultSquad.IsAlive) return;

            var target = enemyUnits
                .Where(e => e.IsAlive)
                .OrderBy(e => Math.Abs(e.X - assaultSquad.X) + Math.Abs(e.Y - assaultSquad.Y))
                .FirstOrDefault();

            if (target == null) return;

            foreach (var unit in assaultSquad.GetFlattenedUnits())
            {
                if (!unit.IsAlive) continue;

                int dx = Math.Sign(target.X - unit.X);
                int dy = Math.Sign(target.Y - unit.Y);
                unit.MoveBy(dx, dy, mapWidth, mapHeight);
            }
        }
    }
}