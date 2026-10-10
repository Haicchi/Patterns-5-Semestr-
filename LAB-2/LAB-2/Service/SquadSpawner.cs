using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using System;
using System.Collections.Generic;

namespace LAB_2.Service
{
    public class SquadSpawner
    {
        public List<Character> SpawnSquad(
            IClanMemberFactory factory,
            int minCount,
            int maxCount,
            int centerX,
            int centerY,
            int radius,
            int mapWidth,
            int mapHeight)
        {
            List<Character> squad = new();
            Character prototype = factory.CreateEquippedPrototype();

            int count = Random.Shared.Next(minCount, maxCount + 1);

            for (int i = 1; i <= count; i++)
            {
                Character soldier = prototype.Clone();
                soldier.Name = $"{prototype.Name} #{i}";
                int offsetX = Random.Shared.Next(-radius, radius + 1);
                int offsetY = Random.Shared.Next(-radius, radius + 1);

                
                soldier.X = Math.Clamp(centerX + offsetX, 0, mapWidth - 1);
                soldier.Y = Math.Clamp(centerY + offsetY, 0, mapHeight - 1);

                squad.Add(soldier);
            }

            return squad;
        }
    }
}