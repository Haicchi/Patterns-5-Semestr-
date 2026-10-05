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

namespace LAB_2.Model.Characters
{
    public abstract class Character
    {
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

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

       
        public virtual void TakeDamage(int rawDamage)
        {
            Health -= rawDamage;
            if (Health < 0) Health = 0;
            Console.WriteLine($"   -> {Name} отримує {rawDamage} шкоди! (Залишилось HP: {Health})");
        }

        public virtual Character Clone() => (Character)this.MemberwiseClone();
    }
}