using LAB_2.Commands;
using LAB_2.Decorators;
using LAB_2.Enums;
using LAB_2.Interfaces;
using LAB_2.Mediator;
using LAB_2.Memento;
using LAB_2.Model;
using LAB_2.Model.Characters;
using LAB_2.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LAB_2.Service
{
    public class BattleEngine
    {
        private readonly ClanLeaderCommander _commander = new();
        private readonly PlayerTurnController _playerController;
        private int _currentRound = 1;

        public int CurrentRound => _currentRound;

        public BattleEngine()
        {
            _playerController = new PlayerTurnController(_commander);
        }

        public void StartCivilWar(Clan clan, Type? playerRaceType = null, Caretaker? caretaker = null)
        {
            Console.Clear();
            Console.WriteLine($"ПОЧАТОК БИТВИ В СЕРЕДИНІ КЛАНУ «{clan.Name}» \nРаси зійшлися в турнірному поєдинку за абсолютну владу!\n");

            if (playerRaceType != null)
            {
                Console.WriteLine($"Ви керуєте расою: {playerRaceType.Name.ToUpper()}\n");
            }

            IBattleMediator mediator = new BattleMediator(clan);
            CombatService combatService = new(mediator);

            for (_currentRound = 1; GetAliveRacesCount(clan.Units) > 1 && _currentRound <= 30; _currentRound++)
            {
                Console.WriteLine($"\n--- РАУНД {_currentRound} ---");
                mediator.Notify(this, BattleEvent.RoundStarted);

                var activeUnits = clan.Units
                    .SelectMany(u => u.GetFlattenedUnits())
                    .Where(m => m.IsAlive)
                    .ToList();

                foreach (var attacker in activeUnits)
                {
                    if (!attacker.IsAlive) continue;

                    var type = GetRaceType(attacker);
                    var baseChar = Unwrap(attacker);

                    if (playerRaceType != null && type == playerRaceType)
                    {
                        _playerController.ProcessTurn(
                            attacker,
                            baseChar,
                            clan,
                            combatService,
                            mediator,
                            caretaker
                        );
                    }
                    else
                    {
                        if (baseChar != null)
                        {
                            baseChar.OnTurnStart();

                            if (!baseChar.IsAlive) continue;

                            if (!baseChar.CanTakeTurn())
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine($"[{baseChar.Name}] пропускає хід через статус: {baseChar.CurrentState.Name}");
                                Console.ResetColor();

                                baseChar.OnTurnEnd();
                                continue;
                            }
                        }

                        var target = clan.Units.SelectMany(u => u.GetFlattenedUnits())
                            .Where(m => m.IsAlive && GetRaceType(m) != type)
                            .MinBy(m => Dist(attacker.X, attacker.Y, m.X, m.Y));

                        if (target == null) break;

                        var leader = GetLeader(type);
                        int dist = Dist(attacker.X, attacker.Y, target.X, target.Y);

                        ICommand cmd = (baseChar?.Health < IBattleConstants.MaxAllowedHealth / 2 && leader is { IsAlive: true } && leader != attacker)
                            ? new MoveBackCommand(leader, clan.Units)
                            : dist <= (baseChar?.Weapon?.Range ?? 1)
                                ? new AttackCommand(target, combatService)
                                : new MoveForwardCommand(target, clan.Units);

                        _commander.IssueCommand(cmd, attacker, clan.MapWidth, clan.MapHeight);

                        baseChar?.OnTurnEnd();
                        Thread.Sleep(120);
                    }
                }

                clan.DrawBattlefieldMap();
                Console.WriteLine("\nНатисніть будь-яку клавішу для наступного раунду (або зачекайте)...");
                Thread.Sleep(600);
            }

            AnnounceWinner(clan.Units);
        }

        public IBattleMemento Save(Clan clan)
        {
            var unitMementos = clan.Units
                .SelectMany(u => u.GetFlattenedUnits())
                .Select(Unwrap)
                .Where(c => c != null)
                .Select(c => (CharacterMemento)c!.Save())
                .ToList();

            return new BattleMemento(_currentRound, unitMementos);
        }

        public void Restore(Clan clan, IBattleMemento memento)
        {
            if (memento is not BattleMemento concrete)
            {
                throw new ArgumentException($"Невідомий клас знімка бою: {memento}");
            }

            _currentRound = concrete.Round;
            var characters = clan.Units
                .SelectMany(u => u.GetFlattenedUnits())
                .Select(Unwrap)
                .Where(c => c != null)
                .ToList();

            for (int i = 0; i < characters.Count && i < concrete.UnitMementos.Count; i++)
            {
                characters[i]!.Restore(concrete.UnitMementos[i]);
            }

            Console.WriteLine($"Стан клану «{clan.Name}» відновлено до раунду {_currentRound}.");
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