using LAB_2.Interfaces;
using LAB_2.Records;
using System.Collections.Generic;

namespace LAB_2.Decorators
{
    public abstract class CharacterDecorator : IWarUnit
    {
        protected IWarUnit _innerUnit;

        protected CharacterDecorator(IWarUnit innerUnit)
        {
            _innerUnit = innerUnit;
        }

        public virtual string Name => _innerUnit.Name;
        public virtual int Health => _innerUnit.Health;
        public virtual char MapSymbol => _innerUnit.MapSymbol;
        public virtual int X => _innerUnit.X;
        public virtual int Y => _innerUnit.Y;
        public virtual bool IsAlive => _innerUnit.IsAlive;

        public IWarUnit InnerUnit => _innerUnit;

        public virtual void MoveBy(int deltaX, int deltaY, int mapWidth, int mapHeight, IEnumerable<IWarUnit>? activeUnits = null)
        {
            _innerUnit.MoveBy(deltaX, deltaY, mapWidth, mapHeight, activeUnits);
        }

        public virtual void TakeDamage(int damage)
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
                _innerUnit.TakeDamage(damage);
            }
        }

        public virtual IHandler? BuildDamagePipeline()
        {
            IHandler? innerPipeline = _innerUnit.BuildDamagePipeline();
            IHandler? decoratorHandler = GetDecoratorHandler();

            if (decoratorHandler == null) return innerPipeline;
            if (innerPipeline == null) return decoratorHandler;

            decoratorHandler.SetNext(innerPipeline);
            return decoratorHandler;
        }

        protected abstract IHandler? GetDecoratorHandler();

        public IEnumerable<IWarUnit> GetFlattenedUnits()
        {
            return _innerUnit.GetFlattenedUnits();
        }
    }
}