using LAB_2.Commands;
using LAB_2.Decorators;
using LAB_2.Enums;
using LAB_2.Interfaces;
using LAB_2.Mediator;
using LAB_2.Memento;
using LAB_2.Model;
using LAB_2.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LAB_2.Service
{
    public class PlayerTurnController
    {
        private readonly ClanLeaderCommander _commander;

        public PlayerTurnController(ClanLeaderCommander commander)
        {
            _commander = commander;
        }

        public bool ProcessTurn(
            IWarUnit attacker,
            Character? baseChar,
            Clan clan,
            CombatService combatService,
            IBattleMediator mediator,
            Caretaker? caretaker)
        {
            if (baseChar == null || !baseChar.IsAlive) return true;
            baseChar.OnTurnStart();
            if (!baseChar.IsAlive) return true;
            if (!baseChar.CanTakeTurn())
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n[{baseChar.Name}] пропускає хід через статус: {baseChar.CurrentState.Name}");
                Console.ResetColor();

                baseChar.OnTurnEnd();
                return true;
            }

            while (attacker.IsAlive)
            {
                clan.DrawBattlefieldMap(attacker);

                int moveStep = baseChar?.Movement?.MoveDistance ?? 1;
                int weaponRange = baseChar?.Weapon?.Range ?? 1;
                bool isLeader = IsUnitRaceLeader(attacker);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n🎮 >>> ПОТОЧНИЙ ХІД: {baseChar?.Name?.ToUpper()} <<<");
                Console.ResetColor();

                Console.WriteLine($"   HP: {baseChar?.Health} | Стан: {baseChar?.CurrentState.Name} | Позиція: [{attacker.X}, {attacker.Y}] | Крок: {moveStep} кл. | Зброя: {baseChar?.Weapon?.Name} (Радіус: {weaponRange})");
                Console.WriteLine("Оберіть дію:");
                Console.WriteLine($" [W/A/S/D] Рух персонажем (на {moveStep} крок(ів))");
                Console.WriteLine(" [1] Атакувати ворога в радіусі ураження");

                if (isLeader)
                {
                    Console.WriteLine(" [2] Активувати Ауру Лідера");
                }

                Console.WriteLine(" [3] Зберегти стан гри");
                Console.WriteLine(" [4] Відкотити хід назад");
                Console.WriteLine(" [0] Пропустити хід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine()?.Trim().ToUpper() ?? "";

                switch (choice)
                {
                    case "W":
                        _commander.IssueCommand(new MoveToCommand(0, -moveStep), attacker, clan.MapWidth, clan.MapHeight);
                        baseChar.OnTurnEnd();
                        return true;

                    case "S":
                        _commander.IssueCommand(new MoveToCommand(0, moveStep), attacker, clan.MapWidth, clan.MapHeight);
                        baseChar.OnTurnEnd();
                        return true;

                    case "A":
                        _commander.IssueCommand(new MoveToCommand(-moveStep, 0), attacker, clan.MapWidth, clan.MapHeight);
                        baseChar.OnTurnEnd();
                        return true;

                    case "D":
                        _commander.IssueCommand(new MoveToCommand(moveStep, 0), attacker, clan.MapWidth, clan.MapHeight);
                        baseChar.OnTurnEnd();
                        return true;

                    case "1":
                        var target = SelectTargetInteractively(attacker, baseChar, clan);
                        if (target != null)
                        {
                            _commander.IssueCommand(new AttackCommand(target, combatService), attacker, clan.MapWidth, clan.MapHeight);
                            baseChar.OnTurnEnd();
                            return true;
                        }
                        break;

                    case "2":
                        if (isLeader)
                        {
                            mediator.Notify(attacker, BattleEvent.RoundStarted);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Активація аури!");
                            Console.ResetColor();
                            baseChar.OnTurnEnd();
                            return true;
                        }
                        Console.WriteLine("Цей юніт не є Лідером раси!");
                        break;

                    case "3":
                        Console.Write("\nВведіть назву для слота збереження: ");
                        string slotName = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(slotName))
                        {
                            slotName = $"Save_Round_{baseChar?.Name}";
                        }
                        caretaker?.SaveToSlot(slotName, attacker.GetType());
                        break;

                    case "4":
                        caretaker?.Undo();
                        return false;

                    case "0":
                        baseChar.OnTurnEnd();
                        return true;

                    default:
                        Console.WriteLine("Невідома команда! Спробуйте ще раз.");
                        break;
                }
            }

            return true;
        }

        private static IWarUnit? SelectTargetInteractively(IWarUnit attacker, Character? baseChar, Clan clan)
        {
            var attackerType = Unwrap(attacker)?.GetType() ?? attacker.GetType();
            int weaponRange = baseChar?.Weapon?.Range ?? 1;

            var validEnemies = clan.Units
                .SelectMany(u => u.GetFlattenedUnits())
                .Where(m =>
                {
                    if (!m.IsAlive) return false;
                    if ((Unwrap(m)?.GetType() ?? m.GetType()) == attackerType) return false;

                    int dist = Math.Max(Math.Abs(attacker.X - m.X), Math.Abs(attacker.Y - m.Y));
                    return dist <= weaponRange;
                })
                .ToList();

            if (!validEnemies.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nНемає досяжних ворогів у радіусі ураження зброї «{baseChar?.Weapon?.Name}» ({weaponRange} кл.)!");
                Console.ResetColor();
                return null;
            }

            Console.WriteLine($"\n--- Вороги в радіусі ураження ({weaponRange} кл.) ---");
            for (int i = 0; i < validEnemies.Count; i++)
            {
                var unwrapped = Unwrap(validEnemies[i]);
                int dist = Math.Max(Math.Abs(attacker.X - validEnemies[i].X), Math.Abs(attacker.Y - validEnemies[i].Y));
                Console.WriteLine($"[{i + 1}] {unwrapped?.Name} (HP: {unwrapped?.Health}, Pos: [{validEnemies[i].X},{validEnemies[i].Y}], Дистанція: {dist})");
            }

            Console.Write("Введіть номер цілі (або 0 для скасування): ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= validEnemies.Count)
            {
                return validEnemies[idx - 1];
            }

            return null;
        }

        private static bool IsUnitRaceLeader(IWarUnit unit)
        {
            var baseChar = Unwrap(unit);
            if (baseChar == null) return false;

            var type = baseChar.GetType();
            return (type == typeof(Warrior) && RaceLeader<Warrior>.IsElected && RaceLeader<Warrior>.Instance.Leader == baseChar) ||
                   (type == typeof(Elf) && RaceLeader<Elf>.IsElected && RaceLeader<Elf>.Instance.Leader == baseChar) ||
                   (type == typeof(Dwarf) && RaceLeader<Dwarf>.IsElected && RaceLeader<Dwarf>.Instance.Leader == baseChar);
        }

        private static Character? Unwrap(IWarUnit u)
        {
            while (u is CharacterDecorator d) u = d.InnerUnit;
            return u as Character;
        }
    }
}