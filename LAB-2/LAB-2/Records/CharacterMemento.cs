using LAB_2.Interfaces;
using System;

namespace LAB_2.Records
{
    public record CharacterMemento(
        string Name,
        int Health,
        int X,
        int Y,
        string? StateTypeName = null,
        int StateDuration = 0
    ) : IBattleMemento
    {
        public DateTime SavedAt { get; init; } = DateTime.Now;

        public string GetName() => $"{Name} (HP:{Health}, Pos:[{X},{Y}])";
        public DateTime GetDate() => SavedAt;
    }
}