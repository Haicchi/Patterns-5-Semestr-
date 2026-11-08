using System;

namespace LAB_2.Records
{
    public record SaveSlotDto
    {
        public string SlotName { get; init; } = string.Empty;
        public DateTime SaveTime { get; init; } = DateTime.Now;
        public string ClanName { get; init; } = string.Empty;
        public string? PlayerRaceTypeName { get; init; }
        public BattleMemento BattleState { get; init; } = null!;
    }
}