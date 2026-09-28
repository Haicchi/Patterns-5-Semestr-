using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using LAB_2.Model.Weapon;
using System;

namespace LAB_2.Service
{
    public sealed class RaceLeader<T> where T : Character
    {
        private static RaceLeader<T>? _instance;

        public T Leader { get; }

        private RaceLeader(T leader, IStrategyFactory strategyFactory)
        {
            Leader = leader;
            ApplyLeaderBuffs(strategyFactory);
        }
        public static RaceLeader<T> Initialize(T candidate, IStrategyFactory strategyFactory)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException($"Лідер для {typeof(T).Name} вже призначений!");
            }

            _instance = new RaceLeader<T>(candidate, strategyFactory);
            return _instance;
        }

        public static RaceLeader<T> Instance =>
            _instance ?? throw new InvalidOperationException($"Лідера для {typeof(T).Name} ще не обрано!");

        public static bool IsElected => _instance != null;

        public static void Reset() => _instance = null;

        private void ApplyLeaderBuffs(IStrategyFactory strategyFactory)
        {
            Leader.Health += 50;

            switch (Leader)
            {
                case Warrior:
                    Leader.Name = $"Лорд-Командувач {Leader.Name}";
                    Leader.Weapon = strategyFactory.GetWeapon(WeaponType.MoonLightGreatSword);
                    break;

                case Elf:
                    Leader.Name = $"Верховний Слідопит {Leader.Name}";
                    Leader.Weapon = strategyFactory.GetWeapon(WeaponType.TwinHeadedGreatBow);
                    break;

                case Dwarf:
                    Leader.Name = $"Тан Підземель {Leader.Name}";
                    Leader.Weapon = strategyFactory.GetWeapon(WeaponType.IronKingHammer);
                    break;

                default:
                    Leader.Name = $"Ватажок {Leader.Name}";
                    break;
            }
        }

        public void DisplayLeaderInfo()
        {
            Console.WriteLine($"★ [{typeof(T).Name.ToUpper()}] {Leader.Name} | HP: {Leader.Health} | Зброя: {Leader.Weapon.Name} (Дальність: {Leader.Weapon.Range}) | Рух: {Leader.Movement.Desc}");
        }
    }
}