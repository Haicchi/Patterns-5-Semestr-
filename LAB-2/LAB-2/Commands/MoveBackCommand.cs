using LAB_2.Decorators;
using LAB_2.Interfaces;
using LAB_2.Model.Characters;
using LAB_2.Model.Decorators;
using System;
using System.Collections.Generic;

namespace LAB_2.Commands
{
    public class MoveBackCommand : ICommand
    {
        private IWarUnit _leader;
        private IEnumerable<IWarUnit>? _activeUnits;

        public string Name => "Назад (Відступ до Лідера)";

        public MoveBackCommand(IWarUnit leader, IEnumerable<IWarUnit>? activeUnits = null)
        {
            _leader = leader;
            _activeUnits = activeUnits;
        }

        public void Execute(IWarUnit unit, int mapWidth, int mapHeight)
        {
            if (!unit.IsAlive || _leader == null || !_leader.IsAlive) return;

            int dx = Math.Sign(_leader.X - unit.X);
            int dy = Math.Sign(_leader.Y - unit.Y);

            var baseChar = UnwrapToBaseCharacter(unit);
            int moveDist = baseChar?.Movement?.MoveDistance ?? 1;

            unit.MoveBy(dx * moveDist, dy * moveDist, mapWidth, mapHeight, _activeUnits);
            Console.WriteLine($"[{unit.MapSymbol}] {unit.Name} відступає під захист Лідера {_leader.Name} -> нова позиція ({unit.X}, {unit.Y})");
        }

        private static Character? UnwrapToBaseCharacter(IWarUnit unit)
        {
            while (unit is CharacterDecorator decorator)
            {
                unit = decorator.InnerUnit;
            }
            return unit as Character;
        }
    }
}