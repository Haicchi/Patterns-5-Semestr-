using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_3._1
{
    public class LightCalculator:CalculatorDecorator
    {
        public LightCalculator(ICalculator innerCalculator) : base(innerCalculator) { }

        public double Add(double a, double b)
        {
            if (b == 0) return a;       
            if (a == 0) return b;       
            return base.Add(a, b);
        }

        public double Subtract(double a, double b)
        {
            if (b == 0) return a;       
            if (a == 0) return -b;    
            return base.Subtract(a, b);
        }

        public double Multiply(double a, double b)
        {
            if (a == 0 || b == 0)   
                return 0;
            return base.Multiply(a, b);
        }

        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Ділення на нуль неможливе.");
            if (a == 0)
                return 0;

            return base.Divide(a, b);
        }
    }
}
