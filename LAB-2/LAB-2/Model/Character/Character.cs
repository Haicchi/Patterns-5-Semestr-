using LAB_2.Interfaces;
using LAB_2.Model.Movement;
using LAB_2.Model.Weapon;
using LAB_2.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Character
{
    public abstract class Character
    {
        // 1. Первинний ключ для EF Core
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public WeaponType WeaponKind { get; set; }
        public MovementType MovementKind { get; set; }
        [NotMapped]
        private IWeapon? _weapon;
        [NotMapped]
        public IWeapon Weapon
        {
            get => _weapon ??= StrategyResolver.GetWeapon(WeaponKind);
            set
            {
                _weapon = value;
                WeaponKind = value switch
                {
                    Sword => WeaponType.Sword,
                    Bow => WeaponType.Bow,
                    Hammer => WeaponType.Hammer,
                    _ => WeaponKind
                };
            }
        }

        [NotMapped]
        private IMovement? _movement;
        [NotMapped]
        public IMovement Movement
        {
            get => _movement ??= StrategyResolver.GetMovement(MovementKind);
            set
            {
                _movement = value;
                MovementKind = value.Type;
            }
        }
        [NotMapped]
        public abstract char MapSymbol { get; }

        public void AttackTarget(Character target)
        {
            if (Weapon.CanHit(X, Y, target.X, target.Y))
            {
                Console.Write($"{Name}");
                Weapon.Attack(X, Y, target.X, target.Y);
                target.Health -= Weapon.Damage;
            }
            else
            {
                Console.WriteLine($"{Name} не дістає до {target.Name} (Радіус зброї: {Weapon.Range})");
            }
        }
        public virtual Character Clone()
        {
            var clone = (Character)this.MemberwiseClone();
            clone.Id = 0; 
            return clone;
        }
    }

    

      
    
}
