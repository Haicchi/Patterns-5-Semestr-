using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model
{


    public class Squad : IWarUnit
    {
        public string Name { get; }
        private readonly List<IWarUnit> _members = new();

        public int Health
        {
            get => _members.Sum(m => m.Health);
            set { }
        }
        public int X
        {
            get => _members.Where(m => m.IsAlive).Select(m => m.X).DefaultIfEmpty(0).Average(x => (double)x) > 0
                ? (int)_members.Where(m => m.IsAlive).Select(m => m.X).Average()
                : 0;
            set => throw new NotSupportedException("Не можна змінити координати загону напряму. Рухайте окремих бійців або використовуйте метод переміщення загону.");
        }

        public int Y
        {
            get => _members.Where(m => m.IsAlive).Select(m => m.Y).DefaultIfEmpty(0).Average(y => (double)y) > 0
                ? (int)_members.Where(m => m.IsAlive).Select(m => m.Y).Average()
                : 0;
            set => throw new NotSupportedException("Не можна змінити координати загону напряму.");
        }

        public bool IsAlive => _members.Any(m => m.IsAlive);

        public Squad(string name)
        {
            Name = name;
        }

        public void AddUnit(IWarUnit unit) => _members.Add(unit);
        public void RemoveUnit(IWarUnit unit) => _members.Remove(unit);

        public void TakeDamage(int damage)
        {
            var target = _members.FirstOrDefault(m => m.IsAlive);
            target?.TakeDamage(damage);
        }
        public void MoveBy(int deltaX, int deltaY, int mapWidth, int mapHeight)
        {
            foreach (var member in _members.SelectMany(m => m.GetFlattenedUnits()))
            {
                member.X = Math.Clamp(member.X + deltaX, 0, mapWidth - 1);
                member.Y = Math.Clamp(member.Y + deltaY, 0, mapHeight - 1);
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
