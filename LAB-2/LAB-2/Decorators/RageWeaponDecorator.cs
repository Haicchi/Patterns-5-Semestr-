using LAB_2.Interfaces;

namespace LAB_2.Decorators
{
    public class RageWeaponDecorator : IWeapon
    {
        private readonly IWeapon _innerWeapon;
        private readonly int _bonusDamage;

        public RageWeaponDecorator(IWeapon innerWeapon, int bonusDamage)
        {
            _innerWeapon = innerWeapon;
            _bonusDamage = bonusDamage;
        }

        public string Name => $"{_innerWeapon.Name} (Лють)";
        public int Damage => _innerWeapon.Damage + _bonusDamage;
        public int Range => _innerWeapon.Range;

        public bool CanHit(int ax, int ay, int tx, int ty)
        {
            return _innerWeapon.CanHit(ax, ay, tx, ty);
        }

        public void Attack(int ax, int ay, int tx, int ty)
        {
            _innerWeapon.Attack(ax, ay, tx, ty);
        }
    }
}