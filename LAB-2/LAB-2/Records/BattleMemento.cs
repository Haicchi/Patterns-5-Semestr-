using System;
using System.Collections.Generic;
using LAB_2.Interfaces;

namespace LAB_2.Records
{
    public record BattleMemento : IBattleMemento
    {
        public int Round { get; init; }
        public List<CharacterMemento> UnitMementos { get; init; } = new();
        public DateTime Date { get; init; } = DateTime.Now;

        public BattleMemento() { }

        public BattleMemento(int round, List<CharacterMemento> unitMementos)
        {
            Round = round;
            UnitMementos = unitMementos;
            Date = DateTime.Now;
        }

        public string GetName() => $"Раунд {Round} (Бійців: {UnitMementos.Count})";
        public DateTime GetDate() => Date;
    }
}