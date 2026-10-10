using LAB_2.Model;
using LAB_2.Model.Characters;
using System;
using System.Linq;

namespace LAB_2.Service
{
    public class GameMenu
    {
        private readonly ClanInitializer _initializer = new();
        private readonly BattleEngine _battleEngine = new();
        private Clan? _clan;

        public void Run()
        {
            // Початковий клан
            _clan = _initializer.CreateClan("Північні Вовки", mapWidth: 18, mapHeight: 9);

            while (true)
            {
                Console.WriteLine("\n╔══════════════════════════════════════════════════════╗");
                Console.WriteLine($"║            ГОЛОВНЕ МЕНЮ: «БІЙ КЛАНІВ»                ║");
                Console.WriteLine($"║ Клан: {_clan.Name,-20} Живих: {_clan.Members.Count(m => m.Health > 0),-3}                   ║");
                Console.WriteLine("╠══════════════════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Швидка авто-генерація клану                       ║");
                Console.WriteLine("║ 2. СТВОРИТИ ВЛАСНИЙ КЛАН (Конструктор гравця)        ║");
                Console.WriteLine("║ 3. Текстовий звіт складу клану та координат          ║");
                Console.WriteLine("║ 4. Псевдографічна карта фронту                       ║");
                Console.WriteLine("║ 5. Інформація про глав рас                           ║");
                Console.WriteLine("║ 6. Викликати підкріплення                            ║");
                Console.WriteLine("║ 7. РОЗПОЧАТИ БІЙ                                     ║");
                Console.WriteLine("║ 0. Вихід                                             ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════╝");
                Console.Write("Ваш вибір: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _clan = _initializer.CreateClan("Північні Вовки", 18, 9);
                        Console.WriteLine("Клан за замовчуванням згенеровано!");
                        break;

                    case "2":
                        CreateCustomClanDialogue();
                        break;

                    case "3":
                        _clan.PrintTextReport();
                        break;

                    case "4":
                        _clan.DrawBattlefieldMap();
                        break;

                    case "5":
                        _clan.DisplayLeadersReport();
                        break;

                    case "6":
                        SpawnReinforcement();
                        break;

                    case "7":
                        _battleEngine.StartCivilWar(_clan);
                        break;

                    case "0":
                        Console.WriteLine("Вихід з програми. До зустрічі!");
                        return;

                    default:
                        Console.WriteLine("Невірний пункт меню, спробуйте ще раз.");
                        break;
                }
            }
        }


        private void CreateCustomClanDialogue()
        {
            Console.WriteLine("\n--- КОНСТРУКТОР НОВОГО КЛАНУ ---");

            Console.Write("Введіть назву клану: ");
            string name = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(name)) name = "Сталевий Легіон";

            int width = ReadInt("Ширина поля бою (за замовчуванням 18): ", 18, 6, 100);
            int height = ReadInt("Висота поля бою (за замовчуванням 9, кратно 3): ", 9, 3, 100);

            Console.WriteLine("\nНалаштування кількості бійців у загонах (мін / макс):");
            var wMin = ReadInt("Мінімум воїнів: ", 2, 1, 50);
            var wMax = ReadInt("Максимум воїнів: ", 5, wMin, 50);

            var eMin = ReadInt("Мінімум ельфів: ", 2, 1, 50);
            var eMax = ReadInt("Максимум ельфів: ", 5, eMin, 50);

            var dMin = ReadInt("Мінімум гномів: ", 2, 1, 50);
            var dMax = ReadInt("Максимум гномів: ", 5, dMin, 50);

            _clan = _initializer.CreateCustomClan(
                name,
                width,
                height,
                (wMin, wMax),
                (eMin, eMax),
                (dMin, dMax));

            Console.WriteLine($"\n🛡️ Клан «{_clan.Name}» успішно засновано! Загальна чисельність армії: {_clan.Members.Count} бійців.");
        }

        private static int ReadInt(string prompt, int defaultValue, int min, int max)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int result) && result >= min && result <= max)
            {
                return result;
            }

            return defaultValue;
        }

        private void SpawnReinforcement()
        {
            if (_clan == null || _clan.Members.Count == 0) return;
            var regularSoldiers = _clan.Members.Where(m =>
                m.Health > 0 &&
                (!RaceLeader<Warrior>.IsElected || RaceLeader<Warrior>.Instance.Leader != m) &&
                (!RaceLeader<Elf>.IsElected || RaceLeader<Elf>.Instance.Leader != m) &&
                (!RaceLeader<Dwarf>.IsElected || RaceLeader<Dwarf>.Instance.Leader != m)
            ).ToList();

            if (regularSoldiers.Count == 0)
            {
                Console.WriteLine("Немає рядових бійців для створення підкріплення (лишилися тільки лідери)!");
                return;
            }
            var donor = regularSoldiers[Random.Shared.Next(regularSoldiers.Count)];
            var clone = donor.Clone();

            clone.Name = $"{donor.Name} (Підкріплення)";
            clone.X = Random.Shared.Next(0, _clan.MapWidth);
            clone.Y = Random.Shared.Next(0, _clan.MapHeight);

            _clan.Members.Add(clone);

            Console.WriteLine($"\nУспішно клоновано підкріплення від рядового бійця [{donor.Name}]!");
            Console.WriteLine($"   Новий боєць: {clone.Name} | Позиція: ({clone.X},{clone.Y}) | Зброя: {clone.Weapon.Name}");
        }
    }
}