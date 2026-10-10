using LAB_2.Factory;
using LAB_2.Interfaces;
using LAB_2.Model;
using LAB_2.Model.Characters;
using System;
using System.Linq;

namespace LAB_2.Service
{
    public class ClanInitializer
    {
        private readonly IStrategyFactory _strategyFactory;
        private readonly SquadSpawner _spawner;

        public ClanInitializer(IStrategyFactory? strategyFactory = null, SquadSpawner? spawner = null)
        {
            _strategyFactory = strategyFactory ?? new StrategyResolver();
            _spawner = spawner ?? new SquadSpawner();
        }

        public Clan CreateClan(string clanName, int mapWidth = 18, int mapHeight = 9)
        {
            return CreateCustomClan(clanName, mapWidth, mapHeight,
                warriorCount: (3, 6),
                elfCount: (3, 6),
                dwarfCount: (3, 6));
        }

        public Clan CreateCustomClan(
            string clanName,
            int mapWidth,
            int mapHeight,
            (int min, int max) warriorCount,
            (int min, int max) elfCount,
            (int min, int max) dwarfCount)
        {
            RaceLeader<Warrior>.Reset();
            RaceLeader<Elf>.Reset();
            RaceLeader<Dwarf>.Reset();

            var warriorFactory = new WarriorFactory(_strategyFactory);
            var elfFactory = new ElfFactory(_strategyFactory);
            var dwarfFactory = new DwarfFactory(_strategyFactory);

            var clan = new Clan(clanName, mapWidth, mapHeight);
            int zoneWidth = mapWidth / 3;

            int wCenterX = zoneWidth / 2;
            int wCenterY = mapHeight / 2;

            int eCenterX = zoneWidth + (zoneWidth / 2);
            int eCenterY = mapHeight / 2;
            int dCenterX = (zoneWidth * 2) + (zoneWidth / 2);
            int dCenterY = mapHeight / 2;

            int radius = Math.Max(3, zoneWidth / 2);
            var rawWarrior = (Warrior)warriorFactory.CreateEquippedPrototype();
            rawWarrior.X = wCenterX;
            rawWarrior.Y = wCenterY;
            RaceLeader<Warrior>.Initialize(rawWarrior, _strategyFactory);
            clan.Units.Add(RaceLeader<Warrior>.Instance.Leader);

            var wSpawned = _spawner.SpawnSquad(warriorFactory, warriorCount.min, warriorCount.max, wCenterX, wCenterY, radius, mapWidth, mapHeight)
                                   .Where(w => w != RaceLeader<Warrior>.Instance.Leader).ToList();

            var wBodyguards = new Squad("Охорона Лідера Воїнів");
            var wAssault = new Squad("Штурмовий загін Воїнів");
            foreach (var w in wSpawned.Take(2)) wBodyguards.AddUnit(w);
            foreach (var w in wSpawned.Skip(2)) wAssault.AddUnit(w);
            clan.Units.Add(wBodyguards);
            clan.Units.Add(wAssault);
            var rawElf = (Elf)elfFactory.CreateEquippedPrototype();
            rawElf.X = eCenterX;
            rawElf.Y = eCenterY;
            RaceLeader<Elf>.Initialize(rawElf, _strategyFactory);
            clan.Units.Add(RaceLeader<Elf>.Instance.Leader);

            var eSpawned = _spawner.SpawnSquad(elfFactory, elfCount.min, elfCount.max, eCenterX, eCenterY, radius, mapWidth, mapHeight)
                                   .Where(e => e != RaceLeader<Elf>.Instance.Leader).ToList();

            var eBodyguards = new Squad("Охорона Лідера Ельфів");
            var eAssault = new Squad("Штурмовий загін Ельфів");
            foreach (var e in eSpawned.Take(2)) eBodyguards.AddUnit(e);
            foreach (var e in eSpawned.Skip(2)) eAssault.AddUnit(e);
            clan.Units.Add(eBodyguards);
            clan.Units.Add(eAssault);
            var rawDwarf = (Dwarf)dwarfFactory.CreateEquippedPrototype();
            rawDwarf.X = dCenterX;
            rawDwarf.Y = dCenterY;
            RaceLeader<Dwarf>.Initialize(rawDwarf, _strategyFactory);
            clan.Units.Add(RaceLeader<Dwarf>.Instance.Leader);

            var dSpawned = _spawner.SpawnSquad(dwarfFactory, dwarfCount.min, dwarfCount.max, dCenterX, dCenterY, radius, mapWidth, mapHeight)
                                   .Where(d => d != RaceLeader<Dwarf>.Instance.Leader).ToList();

            var dBodyguards = new Squad("Охорона Лідера Гномів");
            var dAssault = new Squad("Штурмовий загін Гномів");
            foreach (var d in dSpawned.Take(2)) dBodyguards.AddUnit(d);
            foreach (var d in dSpawned.Skip(2)) dAssault.AddUnit(d);
            clan.Units.Add(dBodyguards);
            clan.Units.Add(dAssault);

            return clan;
        }
    }
}