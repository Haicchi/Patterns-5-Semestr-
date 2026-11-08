using LAB_2.Decorators;
using LAB_2.Enums;
using LAB_2.Interfaces;
using LAB_2.Model;
using LAB_2.Model.Characters;
using LAB_2.Model.Decorators;
using LAB_2.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LAB_2.Mediator
{
    public class BattleMediator : IBattleMediator
    {
        private readonly Clan _clan;
        private readonly LeaderTacticsManager _tacticsManager = new();

        private readonly Dictionary<Type, LeaderAuraController> _auraControllers = new()
        {
            [typeof(Warrior)] = new(duration: IBattleConstants.WarriorAuraDuration, cooldown: IBattleConstants.WarriorAuraCooldown),
            [typeof(Dwarf)] = new(duration: IBattleConstants.DwarfAuraDuration, cooldown: IBattleConstants.DwarfAuraCooldown),
            [typeof(Elf)] = new(duration: IBattleConstants.ElfAuraDuration, cooldown: IBattleConstants.ElfAuraCooldown)
        };

        public BattleMediator(Clan clan)
        {
            _clan = clan;
        }

        public void Notify(object sender, BattleEvent battleEvent, object? payload = null)
        {
            switch (battleEvent)
            {
                case BattleEvent.RoundStarted:
                    ProcessAuras();
                    SyncAllBodyguards();
                    break;

                case BattleEvent.UnitDied:
                    if (payload is IWarUnit deadUnit) HandleUnitDeath(deadUnit);
                    break;
            }
        }

        private void ProcessAuras()
        {
            foreach (var controller in _auraControllers.Values)
            {
                controller.ProcessTurn();
            }

            ProcessRaceAura<Warrior>(_auraControllers[typeof(Warrior)]);
            ProcessRaceAura<Dwarf>(_auraControllers[typeof(Dwarf)]);
            ProcessRaceAura<Elf>(_auraControllers[typeof(Elf)]);

            if (_auraControllers[typeof(Elf)].IsActive)
            {
                foreach (var u in _clan.Units.SelectMany(u => u.GetFlattenedUnits()).OfType<HealDecorator>())
                {
                    u.ApplyHealTick();
                }
            }
        }

        private void ProcessRaceAura<T>(LeaderAuraController ctrl) where T : Character
        {
            if (!RaceLeader<T>.IsElected || !RaceLeader<T>.Instance.Leader.IsAlive) return;
            var leader = RaceLeader<T>.Instance.Leader;

            if (ctrl.IsReady && ctrl.TryActivate())
            {
                Console.WriteLine($"Лідер раси {typeof(T).Name} ({leader.Name}) активував бойову ауру!");
            }

            if (ctrl.IsActive)
            {
                foreach (var m in _clan.Units.SelectMany(u => u.GetFlattenedUnits()).Where(m => GetBaseRaceType(m) == typeof(T) && m.IsAlive && m is not CharacterDecorator))
                {
                    if (CalculateDistance(m.X, m.Y, leader.X, leader.Y) <= 4)
                    {
                        Console.WriteLine($"   🟢 {m.Name} потрапив під дію аури!");
                    }
                }
            }
        }

        private void SyncAllBodyguards()
        {
            Sync<Warrior>(IBattleLexems.WarriorBodyguardSquad);
            Sync<Elf>(IBattleLexems.ElfBodyguardSquad);
            Sync<Dwarf>(IBattleLexems.DwarfBodyguardSquad);
        }

        private void Sync<T>(string lexem) where T : Character
        {
            if (RaceLeader<T>.IsElected && RaceLeader<T>.Instance.Leader is { IsAlive: true } l &&
                _clan.Units.OfType<Squad>().FirstOrDefault(s => s.Name.Contains(lexem)) is { } squad)
            {
                _tacticsManager.SyncBodyguardsWithLeader(l, squad, _clan.MapWidth, _clan.MapHeight);
            }
        }

        private void HandleUnitDeath(IWarUnit deadUnit)
        {
            Type raceType = GetBaseRaceType(deadUnit);

            if (IsRaceLeader(deadUnit, raceType))
            {
                Console.WriteLine($"\nЛідер раси {raceType.Name} поліг! Загін охоплений панікою!");

                var survivors = _clan.Units
                    .SelectMany(u => u.GetFlattenedUnits())
                    .Where(u => u.IsAlive && GetBaseRaceType(u) == raceType)
                    .ToList();

                foreach (var soldier in survivors)
                {
                    soldier.TakeDamage(soldier.Health);
                }
            }
        }

        private static Type GetBaseRaceType(IWarUnit u)
        {
            while (u is CharacterDecorator d) u = d.InnerUnit;
            return (u as Character)?.GetType() ?? u.GetType();
        }

        private static bool IsRaceLeader(IWarUnit unit, Type raceType)
        {
            if (raceType == typeof(Warrior) && RaceLeader<Warrior>.IsElected) return RaceLeader<Warrior>.Instance.Leader == unit;
            if (raceType == typeof(Elf) && RaceLeader<Elf>.IsElected) return RaceLeader<Elf>.Instance.Leader == unit;
            if (raceType == typeof(Dwarf) && RaceLeader<Dwarf>.IsElected) return RaceLeader<Dwarf>.Instance.Leader == unit;
            return false;
        }

        private static int CalculateDistance(int x1, int y1, int x2, int y2) => Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
    }
}