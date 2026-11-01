using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB_2.Records;

namespace LAB_2.Handlers
{
    public abstract class DamageHandler : IHandler
    {
        protected IHandler? _next;

        public IHandler SetNext(IHandler handler)
        {
            _next = handler;
            return handler;
        }

        public virtual object Handle(object request)
        {
            if (request is not DamageContext context)
            {
                throw new ArgumentException("Очікується об'єкт типу DamageContext", nameof(request));
            }

            if (context.IsBlocked) return context;
            var updatedContext = ProcessDamage(context);
            return _next != null ? _next.Handle(updatedContext) : updatedContext;
        }

        protected abstract DamageContext ProcessDamage(DamageContext context);
    }
}
