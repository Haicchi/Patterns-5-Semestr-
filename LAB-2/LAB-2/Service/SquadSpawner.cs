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
            int startX,
            int endX,
            int startY,
            int endY)
        {
            
            List<Character> squad = new();
            Character prototype = factory.CreateEquippedPrototype();
            int count = Random.Shared.Next(minCount, maxCount + 1);

            for (int i = 1; i <= count; i++)
            {
                Character soldier = prototype.Clone();
                soldier.Name = $"{prototype.Name} #{i}";
                soldier.X = Random.Shared.Next(startX, endX + 1);
                soldier.Y = Random.Shared.Next(startY, endY + 1);

                squad.Add(soldier);
            }

            return squad;
        }
    }
}