using LAB_2.Decorators;
using LAB_2.Model;
using LAB_2.Model.Characters;
using LAB_2.Model.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LAB_2.Service
{
    public class BattleEngine
    {
        private readonly LeaderAuraController _warriorAura = new LeaderAuraController(duration: 3, cooldown: 5);
        private readonly LeaderAuraController _dwarfAura = new LeaderAuraController(duration: 2, cooldown: 4);
        private readonly LeaderAuraController _elfAura = new LeaderAuraController(duration: 2, cooldown: 3);

        private readonly LeaderTacticsManager _tacticsManager = new LeaderTacticsManager();

        public void StartCivilWar(Clan clan)
        {
            Console.Clear();
            Console.WriteLine($"⚔️ ПОЧАТОК БИТВИ В СЕРЕДИНІ КЛАНУ «{clan.Name}» ⚔️");
            Console.WriteLine("Раси зійшлися в турнірному поєдинку за абсолютну владу!\n");

            int round = 1;
            while (GetAliveRacesCount(clan.Members) > 1 && round <= 30)
            {
                Console.WriteLine($"\n--- РАУНД {round} ---");
                UpdateBodyguardTactics(clan);
                ProcessAuras(clan.Members);

                var activeFighters = clan.Members.Where(m => m.Health > 0).ToList();

                foreach (var attacker in activeFighters)
                {
                    if (attacker.Health <= 0) continue;

                    Type attackerBaseType = GetBaseRaceType(attacker);
                    var target = clan.Members
                        .Where(m => m.Health > 0 && GetBaseRaceType(m) != attackerBaseType)
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
                CheckLeaderMorale(clan.Members);

                round++;
                Console.WriteLine("\nНатисніть будь-яку клавішу для наступного раунду (або зачекайте)...");
                Thread.Sleep(600);
            }

            AnnounceWinner(clan.Members);
        }

        private void UpdateBodyguardTactics(Clan clan)
        {
            if (RaceLeader<Warrior>.IsElected)
            {
                var leader = RaceLeader<Warrior>.Instance.Leader;
                var bodyguardSquad = clan.Units.OfType<Squad>().FirstOrDefault(s => s.Name.Contains("Охорона Лідера Воїнів"));
                if (bodyguardSquad != null)
                {
                    _tacticsManager.SyncBodyguardsWithLeader(leader, bodyguardSquad, clan.MapWidth, clan.MapHeight);
                }
            }

            if (RaceLeader<Elf>.IsElected)
            {
                var leader = RaceLeader<Elf>.Instance.Leader;
                var bodyguardSquad = clan.Units.OfType<Squad>().FirstOrDefault(s => s.Name.Contains("Охорона Лідера Ельфів"));
                if (bodyguardSquad != null)
                {
                    _tacticsManager.SyncBodyguardsWithLeader(leader, bodyguardSquad, clan.MapWidth, clan.MapHeight);
                }
            }

            if (RaceLeader<Dwarf>.IsElected)
            {
                var leader = RaceLeader<Dwarf>.Instance.Leader;
                var bodyguardSquad = clan.Units.OfType<Squad>().FirstOrDefault(s => s.Name.Contains("Охорона Лідера Гномів"));
                if (bodyguardSquad != null)
                {
                    _tacticsManager.SyncBodyguardsWithLeader(leader, bodyguardSquad, clan.MapWidth, clan.MapHeight);
                }
            }
        }

        private void ProcessAuras(List<Character> members)
        {
            _warriorAura.ProcessTurn();
            _dwarfAura.ProcessTurn();
            _elfAura.ProcessTurn();

            ProcessRaceAura<Warrior>(members, _warriorAura, (c) => new RageDecorator(c, bonusDamage: 10));
            ProcessRaceAura<Dwarf>(members, _dwarfAura, (c) => new ArmorDecorator(c, armorValue: 20));
            ProcessRaceAura<Elf>(members, _elfAura, (c) => new HealDecorator(c, healAmount: 15));
            if (_elfAura.IsActive)
            {
                foreach (var member in members)
                {
                    if (member is HealDecorator healer)
                    {
                        healer.ApplyHealTick();
                    }
                }
            }
        }

        private void ProcessRaceAura<T>(List<Character> members, LeaderAuraController controller, Func<Character, Character> decoratorFactory) where T : Character
        {
            if (!RaceLeader<T>.IsElected) return;

            var leader = RaceLeader<T>.Instance.Leader;
            if (leader.Health <= 0) return;
            if (controller.IsReady)
            {
                controller.TryActivate();
                Console.WriteLine($"✨ [АУРА] Лідер раси {typeof(T).Name} ({leader.Name}) активував бойову ауру!");
            }
            int auraRadius = 4;
            if (controller.IsActive)
            {
                for (int i = 0; i < members.Count; i++)
                {
                    var member = members[i];
                    if (GetBaseRaceType(member) == typeof(T) && member.Health > 0 && !(member is CharacterDecorator))
                    {
                        int dist = CalculateDistance(member.X, member.Y, leader.X, leader.Y);
                        if (dist <= auraRadius)
                        {
                            members[i] = decoratorFactory(member);
                            Console.WriteLine($"  🟢 {members[i].Name} потрапив під дію аури!");
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < members.Count; i++)
                {
                    var member = members[i];
                    if (GetBaseRaceType(member) == typeof(T) && member is CharacterDecorator decorator)
                    {
                        members[i] = decorator.InnerCharacter;
                        Console.WriteLine($"  ⚪ Дія аури закінчилася для {members[i].Name}. Баф знято!");
                    }
                }
            }
        }

        private static Type GetBaseRaceType(Character character)
        {
            Character current = character;
            while (current is CharacterDecorator decorator)
            {
                current = decorator.InnerCharacter;
            }
            return current.GetType();
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
            members.Where(m => m.Health > 0).Select(m => GetBaseRaceType(m)).Distinct().Count();

        private static void AnnounceWinner(List<Character> members)
        {
            var survivors = members.Where(m => m.Health > 0).ToList();
            var winningRace = survivors.FirstOrDefault() != null ? GetBaseRaceType(survivors.First()).Name : null;

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

        private static void CheckLeaderMorale(List<Character> members)
        {
            CheckRaceMorale<Warrior>(members, "Воїни");
            CheckRaceMorale<Elf>(members, "Ельфи");
            CheckRaceMorale<Dwarf>(members, "Гноми");
        }

        private static void CheckRaceMorale<T>(List<Character> members, string raceName) where T : Character
        {
            if (RaceLeader<T>.IsElected)
            {
                var leader = RaceLeader<T>.Instance.Leader;
                if (leader.Health <= 0)
                {
                    var survivorsOfRace = members.Where(m => GetBaseRaceType(m) == typeof(T) && m.Health > 0).ToList();
                    if (survivorsOfRace.Count > 0)
                    {
                        Console.WriteLine($"\n🏳️ Лідер раси [{raceName}] ({leader.Name}) поліг у бою! Загін охоплений панікою і здається в полон!");
                        foreach (var soldier in survivorsOfRace)
                        {
                            soldier.Health = 0;
                        }
                    }
                }
            }
        }
    }
}