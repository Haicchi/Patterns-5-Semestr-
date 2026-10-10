using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using LAB_2.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LAB_2.Model
{
    public class Clan
    {
        public string Name { get; }
        public List<IWarUnit> Units { get; } = new();

        public int MapWidth { get; }
        public int MapHeight { get; }

        public Clan(string name, int mapWidth = 15, int mapHeight = 10)
        {
            Name = name;
            MapWidth = mapWidth;
            MapHeight = mapHeight;
        }

        public List<Character> Members => Units
            .SelectMany(u => u.GetFlattenedUnits())
            .OfType<Character>()
            .ToList();

        public void AssembleClan(
            IClanMemberFactory warriorFactory,
            IClanMemberFactory elfFactory,
            IClanMemberFactory dwarfFactory,
            SquadSpawner spawner,
            IStrategyFactory strategyFactory,
            (int min, int max) warriorCount,
            (int min, int max) elfCount,
            (int min, int max) dwarfCount)
        {
            Units.Clear();

            int thirdW = MapWidth / 3;

            int wCenterX = thirdW / 2;
            int wCenterY = MapHeight / 2;

            int eCenterX = thirdW + (thirdW / 2);
            int eCenterY = MapHeight / 2;

            int dCenterX = (thirdW * 2) + (thirdW / 2);
            int dCenterY = MapHeight / 2;

            int radius = Math.Max(2, thirdW / 3);
            var rawWarrior = (Warrior)warriorFactory.CreateEquippedPrototype();
            rawWarrior.X = wCenterX; rawWarrior.Y = wCenterY;
            RaceLeader<Warrior>.Initialize(rawWarrior, strategyFactory);
            Units.Add(RaceLeader<Warrior>.Instance.Leader);

            var wSpawned = spawner.SpawnSquad(warriorFactory, warriorCount.min, warriorCount.max, wCenterX, wCenterY, radius, MapWidth, MapHeight)
                                  .Where(w => w != RaceLeader<Warrior>.Instance.Leader).ToList();

            var wBodyguards = new Squad("Охорона Лідера Воїнів");
            var wAssault = new Squad("Штурмовий загін Воїнів");
            foreach (var warrior in wSpawned.Take(2)) wBodyguards.AddUnit(warrior);
            foreach (var warrior in wSpawned.Skip(2)) wAssault.AddUnit(warrior);

            Units.Add(wBodyguards);
            Units.Add(wAssault);
            var rawElf = (Elf)elfFactory.CreateEquippedPrototype();
            rawElf.X = eCenterX; rawElf.Y = eCenterY;
            RaceLeader<Elf>.Initialize(rawElf, strategyFactory);
            Units.Add(RaceLeader<Elf>.Instance.Leader);

            var eSpawned = spawner.SpawnSquad(elfFactory, elfCount.min, elfCount.max, eCenterX, eCenterY, radius, MapWidth, MapHeight)
                                  .Where(e => e != RaceLeader<Elf>.Instance.Leader).ToList();

            var eBodyguards = new Squad("Охорона Лідера Ельфів");
            var eAssault = new Squad("Штурмовий загін Ельфів");

            foreach (var elf in eSpawned.Take(2)) eBodyguards.AddUnit(elf);
            foreach (var elf in eSpawned.Skip(2)) eAssault.AddUnit(elf);

            Units.Add(eBodyguards);
            Units.Add(eAssault);
            var rawDwarf = (Dwarf)dwarfFactory.CreateEquippedPrototype();
            rawDwarf.X = dCenterX; rawDwarf.Y = dCenterY;
            RaceLeader<Dwarf>.Initialize(rawDwarf, strategyFactory);
            Units.Add(RaceLeader<Dwarf>.Instance.Leader);

            var dSpawned = spawner.SpawnSquad(dwarfFactory, dwarfCount.min, dwarfCount.max, dCenterX, dCenterY, radius, MapWidth, MapHeight)
                                  .Where(d => d != RaceLeader<Dwarf>.Instance.Leader).ToList();

            var dBodyguards = new Squad("Охорона Лідера Гномів");
            var dAssault = new Squad("Штурмовий загін Гномів");

            foreach (var dwarf in dSpawned.Take(2)) dBodyguards.AddUnit(dwarf);
            foreach (var dwarf in dSpawned.Skip(2)) dAssault.AddUnit(dwarf);

            Units.Add(dBodyguards);
            Units.Add(dAssault);
        }

        public void ElectLeaders(IStrategyFactory strategyFactory)
        {
            var warriors = Members.OfType<Warrior>().ToList();
            if (warriors.Count > 0 && !RaceLeader<Warrior>.IsElected)
            {
                var leader = warriors[Random.Shared.Next(warriors.Count)];
                RaceLeader<Warrior>.Initialize(leader, strategyFactory);
            }

            var elves = Members.OfType<Elf>().ToList();
            if (elves.Count > 0 && !RaceLeader<Elf>.IsElected)
            {
                var leader = elves[Random.Shared.Next(elves.Count)];
                RaceLeader<Elf>.Initialize(leader, strategyFactory);
            }

            var dwarves = Members.OfType<Dwarf>().ToList();
            if (dwarves.Count > 0 && !RaceLeader<Dwarf>.IsElected)
            {
                var leader = dwarves[Random.Shared.Next(dwarves.Count)];
                RaceLeader<Dwarf>.Initialize(leader, strategyFactory);
            }
        }

        public void PrintTextReport()
        {
            var members = Members;
            Console.WriteLine($"\n=== ТЕКСТОВИЙ СКЛАД КЛАНУ «{Name}» (Всього: {members.Count}) ===");
            Console.WriteLine("{0,-25} {1,-10} {2,-12} {3,-18} {4,-15}", "Ім'я", "HP", "Координати", "Зброя (Дальність)", "Тип руху");
            Console.WriteLine(new string('-', 85));

            foreach (var member in members)
            {
                Console.WriteLine("{0,-25} {1,-10} ({2,2},{3,2})       {4,-18} {5,-15}",
                    member.Name,
                    member.Health,
                    member.X,
                    member.Y,
                    $"{member.Weapon.Name} (d:{member.Weapon.Range})",
                    member.Movement.Desc);
            }
            Console.WriteLine(new string('-', 85));
        }

        public void DrawBattlefieldMap()
        {
            Console.WriteLine($"\n=== ПСЕВДОГРАФІЧНА КАРТА РОЗТАШУВАННЯ ФРОНТУ ({MapWidth}x{MapHeight}) ===");
            Console.WriteLine("Легенда: [W] - Воїн, [E] - Ельф, [D] - Гном, [*] - Лідер раси, [.] - Порожньо\n");

            string[,] grid = new string[MapHeight, MapWidth];
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    grid[y, x] = " . ";
                }
            }

            foreach (var m in Members)
            {
                if (m.X >= 0 && m.X < MapWidth && m.Y >= 0 && m.Y < MapHeight)
                {
                    bool isLeader = (RaceLeader<Warrior>.IsElected && RaceLeader<Warrior>.Instance.Leader == m) ||
                                    (RaceLeader<Elf>.IsElected && RaceLeader<Elf>.Instance.Leader == m) ||
                                    (RaceLeader<Dwarf>.IsElected && RaceLeader<Dwarf>.Instance.Leader == m);

                    grid[m.Y, m.X] = isLeader ? $"*{m.MapSymbol}*" : $" {m.MapSymbol} ";
                }
            }

            Console.Write("   +");
            for (int x = 0; x < MapWidth; x++) Console.Write("---");
            Console.WriteLine("+");

            for (int y = 0; y < MapHeight; y++)
            {
                Console.Write($"{y,2} |");
                for (int x = 0; x < MapWidth; x++)
                {
                    Console.Write(grid[y, x]);
                }
                Console.WriteLine("|");
            }

            Console.Write("   +");
            for (int x = 0; x < MapWidth; x++) Console.Write("---");
            Console.WriteLine("+");
        }

        public void DisplayLeadersReport()
        {
            Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║                         ГЛАВИ РАС КЛАНУ «{Name,-20}»                         ║");
            Console.WriteLine("╠═══════════════════════════════════════════════════════════════════════════════════════╣");

            if (RaceLeader<Warrior>.IsElected)
            {
                var w = RaceLeader<Warrior>.Instance.Leader;
                Console.WriteLine($"║ ВОЇНИ : {w.Name,-25} | HP: {w.Health,-4} | Поз: ({w.X,2},{w.Y,2}) | Зброя: {w.Weapon.Name,-12} ║");
            }

            if (RaceLeader<Elf>.IsElected)
            {
                var e = RaceLeader<Elf>.Instance.Leader;
                Console.WriteLine($"║ ЕЛЬФИ : {e.Name,-25} | HP: {e.Health,-4} | Поз: ({e.X,2},{e.Y,2}) | Зброя: {e.Weapon.Name,-12} ║");
            }

            if (RaceLeader<Dwarf>.IsElected)
            {
                var d = RaceLeader<Dwarf>.Instance.Leader;
                Console.WriteLine($"║ ГНОМИ : {d.Name,-25} | HP: {d.Health,-4} | Поз: ({d.X,2},{d.Y,2}) | Зброя: {d.Weapon.Name,-12} ║");
            }

            Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════════════╝\n");
        }
    }
}