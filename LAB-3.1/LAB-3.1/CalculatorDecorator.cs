using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_3._1
{
    public abstract class CalculatorDecorator : ICalculator
    {
        protected readonly ICalculator _innerCalculator;

        protected CalculatorDecorator(ICalculator innerCalculator)
        {
            _innerCalculator = innerCalculator ?? throw new ArgumentNullException(nameof(innerCalculator));
        }

        public virtual double Add(double a, double b) => _innerCalculator.Add(a, b);
        public virtual double Subtract(double a, double b) => _innerCalculator.Subtract(a, b);
        public virtual double Multiply(double a, double b) => _innerCalculator.Multiply(a, b);
        public virtual double Divide(double a, double b) => _innerCalculator.Divide(a, b);
    }
}
