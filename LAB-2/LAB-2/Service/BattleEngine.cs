using LAB_2.Model;
using LAB_2.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LAB_2.Service
{
    public class BattleEngine
    {
        public void StartCivilWar(Clan clan)
        {
            Console.Clear();
            Console.WriteLine($"⚔️ ПОЧАТОК БИТВИ В СЕРЕДИНІ КЛАНУ «{clan.Name}» ⚔️");
            Console.WriteLine("Раси зійшлися в турнірному поєдинку за абсолютну владу!\n");

            int round = 1;
            while (GetAliveRacesCount(clan.Members) > 1 && round <= 30)
            {
                Console.WriteLine($"\n--- РАУНД {round} ---");

                var activeFighters = clan.Members.Where(m => m.Health > 0).ToList();

                foreach (var attacker in activeFighters)
                {
                    if (attacker.Health <= 0) continue;
                    var target = clan.Members
                        .Where(m => m.Health > 0 && m.GetType() != attacker.GetType())
                        .OrderBy(m => CalculateDistance(attacker.X, attacker.Y, m.X, m.Y))
                        .FirstOrDefault();

                    if (target == null) break;

                    int distance = CalculateDistance(attacker.X, attacker.Y, target.X, target.Y);

                   
                    if (distance <= attacker.Weapon.Range)
                    {
                        Console.WriteLine($"💥 [{attacker.MapSymbol}] {attacker.Name} атакує [{target.MapSymbol}] {target.Name} зброєю {attacker.Weapon.Name} (Дистанція: {distance}):");
                        target.TakeDamage(attacker.Weapon.Damage);

                        if (target.Health <= 0)
                        {
                            Console.WriteLine($"   💀 {target.Name} поляг у бою!");
                        }
                    }
                    else
                    {
                        MoveTowards(attacker, target, clan.MapWidth, clan.MapHeight);
                        Console.WriteLine($"🏃 [{attacker.MapSymbol}] {attacker.Name} рухається ({attacker.Movement.Desc}) -> нова позиція ({attacker.X},{attacker.Y})");
                    }

                    Thread.Sleep(120); 
                }
                clan.DrawBattlefieldMap();

                round++;
                Console.WriteLine("\nНатисніть будь-яку клавішу для наступного раунду (або зачекайте)...");
                Thread.Sleep(600);
            }

            AnnounceWinner(clan.Members);
        }

        private static int CalculateDistance(int x1, int y1, int x2, int y2) =>
            Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));

        private static void MoveTowards(Character mover, Character target, int mapWidth, int mapHeight)
        {
            int dx = Math.Sign(target.X - mover.X);
            int dy = Math.Sign(target.Y - mover.Y);

            mover.X = Math.Clamp(mover.X + dx * mover.Movement.MoveDistance, 0, mapWidth - 1);
            mover.Y = Math.Clamp(mover.Y + dy * mover.Movement.MoveDistance, 0, mapHeight - 1);
        }

        private static int GetAliveRacesCount(List<Character> members) =>
            members.Where(m => m.Health > 0).Select(m => m.GetType()).Distinct().Count();

        private static void AnnounceWinner(List<Character> members)
        {
            var survivors = members.Where(m => m.Health > 0).ToList();
            var winningRace = survivors.FirstOrDefault()?.GetType().Name;

            Console.WriteLine("\n=======================================================");
            if (winningRace != null)
            {
                Console.WriteLine($"🏆 ПЕРЕМОГА РАСИ: {winningRace.ToUpper()}! Вціліло бійців: {survivors.Count}");
            }
            else
            {
                Console.WriteLine("💀 Нічия. На полі бою не лишилося живих.");
            }
            Console.WriteLine("=======================================================");
        }
    }
}