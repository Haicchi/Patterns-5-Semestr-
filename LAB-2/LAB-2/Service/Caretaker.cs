using LAB_2.Interfaces;
using LAB_2.Model;
using LAB_2.Records;
using LAB_2.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LAB_2.Memento
{
    public class Caretaker
    {
        private static readonly string SavesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saves");
        private readonly List<IBattleMemento> _mementos = new();
        private readonly BattleEngine _originator;
        private readonly Clan _clan;

        public Caretaker(BattleEngine originator, Clan clan)
        {
            _originator = originator;
            _clan = clan;

            if (!Directory.Exists(SavesFolder))
            {
                Directory.CreateDirectory(SavesFolder);
            }
        }

        public void Backup()
        {
            _mementos.Add(_originator.Save(_clan));
        }

        public void Undo()
        {
            if (_mementos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nІсторія збережень порожня!");
                Console.ResetColor();
                return;
            }

            var memento = _mementos.Last();
            _mementos.Remove(memento);

            try
            {
                _originator.Restore(_clan, memento);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nСтан бою успішно відкочено до: {memento.GetName()}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка при відновленні стану: {ex.Message}");
                Console.ResetColor();
            }
        }

        public void SaveToSlot(string slotName, Type? playerRaceType)
        {
            if (_originator.Save(_clan) is not BattleMemento battleMemento) return;

            var dto = new SaveSlotDto
            {
                SlotName = slotName,
                SaveTime = DateTime.Now,
                ClanName = _clan.Name,
                PlayerRaceTypeName = playerRaceType?.AssemblyQualifiedName,
                BattleState = battleMemento
            };

            string safeFileName = string.Join("_", slotName.Split(Path.GetInvalidFileNameChars()));
            string filePath = Path.Combine(SavesFolder, $"{safeFileName}.json");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };

            string json = JsonSerializer.Serialize(dto, options);
            File.WriteAllText(filePath, json);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nСтан гри успішно збережено в файл-слот: «{slotName}» ({filePath})");
            Console.ResetColor();
        }

        public static List<SaveSlotDto> GetAllSaveSlots()
        {
            var slots = new List<SaveSlotDto>();
            if (!Directory.Exists(SavesFolder)) return slots;

            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };

            foreach (var file in Directory.GetFiles(SavesFolder, "*.json"))
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var dto = JsonSerializer.Deserialize<SaveSlotDto>(json, options);
                    if (dto != null) slots.Add(dto);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Не вдалося зчитати файл {Path.GetFileName(file)}: {ex.Message}");
                    Console.ResetColor();
                }
            }

            return slots.OrderByDescending(s => s.SaveTime).ToList();
        }

        public bool LoadFromSlot(SaveSlotDto slot, out Type? playerRaceType)
        {
            playerRaceType = !string.IsNullOrEmpty(slot.PlayerRaceTypeName)
                ? Type.GetType(slot.PlayerRaceTypeName)
                : null;

            try
            {
                _originator.Restore(_clan, slot.BattleState);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n📂 Слот «{slot.SlotName}» успішно завантажено! Раунд: {slot.BattleState.Round}");
                Console.ResetColor();
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Не вдалося відновити стан зі слота «{slot.SlotName}»: {ex.Message}");
                Console.ResetColor();
                return false;
            }
        }
    }
}