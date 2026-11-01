using LAB_2.Handlers;
using LAB_2.Interfaces;
using LAB_2.Records;
using System.Collections.Generic;
using System.Linq;

namespace LAB_2.Model
{
    public class Squad : IWarUnit
    {
        private readonly List<IWarUnit> _members = new();

        public string Name { get; }

        public char MapSymbol => _members.FirstOrDefault(u => u.IsAlive)?.MapSymbol ?? 'S';

        public int Health => _members.Sum(m => m.Health);
        public int X => _members.FirstOrDefault(u => u.IsAlive)?.X ?? 0;
        public int Y => _members.FirstOrDefault(u => u.IsAlive)?.Y ?? 0;

        public bool IsAlive => _members.Any(m => m.IsAlive);

        public Squad(string name)
        {
            Name = name;
        }

        public IHandler? BuildDamagePipeline()
        {
            return new SquadDamageHandler(this);
        }

        public void TakeDamage(int damage)
        {
            var context = new DamageContext(
                Target: this,
                RawDamage: damage,
                CalculatedDamage: damage
            );

            IHandler? pipeline = BuildDamagePipeline();
            if (pipeline != null)
            {
                pipeline.Handle(context);
            }
            else
            {
                var target = _members.FirstOrDefault(m => m.IsAlive);
                target?.TakeDamage(damage);
            }
        }

        public void AddUnit(IWarUnit unit) => _members.Add(unit);
        public void RemoveUnit(IWarUnit unit) => _members.Remove(unit);

        public void MoveBy(int deltaX, int deltaY, int mapWidth, int mapHeight, IEnumerable<IWarUnit>? activeUnits = null)
        {
            foreach (var unit in _members)
            {
                unit.MoveBy(deltaX, deltaY, mapWidth, mapHeight, activeUnits);
            }
        }

        public IEnumerable<IWarUnit> GetFlattenedUnits()
        {
            foreach (var member in _members)
            {
                foreach (var sub in member.GetFlattenedUnits())
                {
                    yield return sub;
                }
            }
        }
    }
}