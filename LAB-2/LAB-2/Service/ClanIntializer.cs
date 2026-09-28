using LAB_2.Factory;
using LAB_2.Interfaces;
using LAB_2.Model;
using LAB_2.Model.Characters;

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
        public Clan CreateClan(string clanName, int mapWidth = 15, int mapHeight = 10)
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
            int laneHeight = mapHeight / 3;

            var warriors = _spawner.SpawnSquad(warriorFactory, warriorCount.min, warriorCount.max, 0, mapWidth - 1, 0, laneHeight - 1);
            clan.Members.AddRange(warriors);

            var elves = _spawner.SpawnSquad(elfFactory, elfCount.min, elfCount.max, 0, mapWidth - 1, laneHeight, (laneHeight * 2) - 1);
            clan.Members.AddRange(elves);

            var dwarves = _spawner.SpawnSquad(dwarfFactory, dwarfCount.min, dwarfCount.max, 0, mapWidth - 1, laneHeight * 2, mapHeight - 1);
            clan.Members.AddRange(dwarves);
            clan.ElectLeaders(_strategyFactory);

            return clan;
        }
    }
}