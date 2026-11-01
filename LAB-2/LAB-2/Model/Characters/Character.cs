using LAB_2.Handlers;
using LAB_2.Interfaces;
using LAB_2.Model.Movement;
using LAB_2.Model.Weapon;
using LAB_2.Records;
using LAB_2.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Characters
{
    public abstract class Character : IWarUnit
    {
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public int X { get; set; }
        public int Y { get; set; }        
        public bool IsAlive => Health > 0;

        public IWeapon Weapon { get; set; } = null!;
        public IMovement Movement { get; set; } = null!;
        public abstract char MapSymbol { get; }

        public void AttackTarget(Character target)
        {
            if (Weapon.CanHit(X, Y, target.X, target.Y))
            {
                Console.Write($"{Name}");
                Weapon.Attack(X, Y, target.X, target.Y);

                target.TakeDamage(Weapon.Damage);
            }
            else
            {
                Console.WriteLine($"{Name} не дістає до {target.Name} (Радіус зброї: {Weapon.Range})");
            }
        }
        public void MoveBy(int deltaX, int deltaY, int mapWidth, int mapHeight, IEnumerable<IWarUnit>? activeUnits = null)
        {
            int newX = Math.Clamp(X + deltaX, 0, mapWidth - 1);
            int newY = Math.Clamp(Y + deltaY, 0, mapHeight - 1);

            if (activeUnits != null)
            {
                bool isOccupied = activeUnits
                    .SelectMany(u => u.GetFlattenedUnits())
                    .Any(u => u.IsAlive && u != this && u.X == newX && u.Y == newY);

                if (isOccupied) return;
            }

            X = newX;
            Y = newY;
        }

        public virtual void TakeDamage(int rawDamage)
        {
            var context = new DamageContext(
                Target: this,
                RawDamage: rawDamage,
                CalculatedDamage: rawDamage
            );

            IHandler? pipeline = BuildDamagePipeline();
            if (pipeline != null)
            {
                pipeline.Handle(context);
            }
            else
            {
                Health = Math.Max(0, Health - rawDamage);
                Console.WriteLine($"   -> {Name} отримує {rawDamage} шкоди! (Залишилось HP: {Health})");
            }
        }
        public virtual IHandler BuildDamagePipeline()
        { 
            return new BaseHealthDamageHandler();
        }
        public IEnumerable<IWarUnit> GetFlattenedUnits()
        {
            if (IsAlive)
            {
                yield return this;
            }
        }

        public virtual Character Clone() => (Character)this.MemberwiseClone();
    }
}