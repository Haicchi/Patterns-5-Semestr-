using LAB_2.Interfaces;
using LAB_2.Model;
using LAB_2.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LAB_2.Service
{
    public class LeaderTacticsManager
    {
        public void SyncBodyguardsWithLeader(Character leader, Squad bodyguardSquad, int mapWidth, int mapHeight)
        {
            if (!bodyguardSquad.IsAlive || !leader.IsAlive) return;

            int offsetX = -1;
            foreach (var unit in bodyguardSquad.GetFlattenedUnits())
            {
                unit.X = Math.Clamp(leader.X + offsetX, 0, mapWidth - 1);
                unit.Y = Math.Clamp(leader.Y, 0, mapHeight - 1);

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
                int dx = Math.Sign(target.X - unit.X);
                int dy = Math.Sign(target.Y - unit.Y);

                unit.X = Math.Clamp(unit.X + dx, 0, mapWidth - 1);
                unit.Y = Math.Clamp(unit.Y + dy, 0, mapHeight - 1);
            }
        }
    }
}