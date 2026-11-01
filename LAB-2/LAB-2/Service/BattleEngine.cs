using LAB_2.Commands;
using LAB_2.Decorators;
using LAB_2.Enums;
using LAB_2.Interfaces;
using LAB_2.Mediator;
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
        private readonly ClanLeaderCommander _commander = new();

        public void StartCivilWar(Clan clan)
        {
            Console.Clear();
            Console.WriteLine($"ПОЧАТОК БИТВИ В СЕРЕДИНІ КЛАНУ «{clan.Name}» ⚔️\nРаси зійшлися в турнірному поєдинку за абсолютну владу!\n");

            IBattleMediator mediator = new BattleMediator(clan);
            CombatService combatService = new(mediator);

            for (int round = 1; GetAliveRacesCount(clan.Units) > 1 && round <= 30; round++)
            {
                Console.WriteLine($"\n--- РАУНД {round} ---");
                mediator.Notify(this, BattleEvent.RoundStarted);

                foreach (var attacker in clan.Units.SelectMany(u => u.GetFlattenedUnits()).Where(m => m.IsAlive).ToList())
                {
                    if (!attacker.IsAlive) continue;

                    var type = GetRaceType(attacker);
                    var target = clan.Units.SelectMany(u => u.GetFlattenedUnits())
                        .Where(m => m.IsAlive && GetRaceType(m) != type)
                        .MinBy(m => Dist(attacker.X, attacker.Y, m.X, m.Y));

                    if (target == null) break;

                    var baseChar = Unwrap(attacker);
                    var leader = GetLeader(type);
                    int dist = Dist(attacker.X, attacker.Y, target.X, target.Y);

                    ICommand cmd = (baseChar?.Health < IBattleConstants.MaxAllowedHealth / 2 && leader is { IsAlive: true } && leader != attacker)
                        ? new MoveBackCommand(leader, clan.Units)
                        : dist <= (baseChar?.Weapon?.Range ?? 1)
                            ? new AttackCommand(target, combatService)
                            : new MoveForwardCommand(target, clan.Units);

                    _commander.IssueCommand(cmd, attacker, clan.MapWidth, clan.MapHeight);
                    Thread.Sleep(120);
                }

                clan.DrawBattlefieldMap();
                Console.WriteLine("\nНатисніть будь-яку клавішу для наступного раунду (або зачекайте)...");
                Thread.Sleep(600);
            }

            AnnounceWinner(clan.Units);
        }

        private static Type GetRaceType(IWarUnit u) => Unwrap(u)?.GetType() ?? u.GetType();

        private static Character? Unwrap(IWarUnit u)
        {
            while (u is CharacterDecorator d) u = d.InnerUnit;
            return u as Character;
        }

        private static Character? GetLeader(Type t) =>
            t == typeof(Warrior) && RaceLeader<Warrior>.IsElected ? RaceLeader<Warrior>.Instance.Leader :
            t == typeof(Elf) && RaceLeader<Elf>.IsElected ? RaceLeader<Elf>.Instance.Leader :
            t == typeof(Dwarf) && RaceLeader<Dwarf>.IsElected ? RaceLeader<Dwarf>.Instance.Leader : null;

        private static int Dist(int x1, int y1, int x2, int y2) => Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));

        private static int GetAliveRacesCount(List<IWarUnit> units) =>
            units.SelectMany(u => u.GetFlattenedUnits()).Where(m => m.IsAlive).Select(GetRaceType).Distinct().Count();

        private static void AnnounceWinner(List<IWarUnit> units)
        {
            var survivors = units.SelectMany(u => u.GetFlattenedUnits()).Select(Unwrap).Where(m => m is { IsAlive: true }).Distinct().ToList();
            string? winner = survivors.FirstOrDefault() is { } s ? GetRaceType(s).Name.ToUpper() : null;
            Console.WriteLine($"\n=======================================================\n" +
                (winner != null ? $"ПЕРЕМОГА РАСИ: {winner}! Вціліло бійців: {survivors.Count}" : "Нічия. На полі бою не лишилося живих.") +
                "\n=======================================================");
        }
    }
}