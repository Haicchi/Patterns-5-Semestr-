using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theoryCrafting
{
    public class SystemOption
    {
        public int Id { get; set; }
        public double P1 { get; set; }
        public double P2 { get; set; }
        public double P3 { get; set; }
        public double NormP1 { get; set; }
        public double NormP2 { get; set; }
        public double NormP3 { get; set; }
        public double WeightedP1 { get; set; }
        public double WeightedP2 { get; set; }
        public double WeightedP3 { get; set; }
        public double Utility { get; set; }
        public bool IsParetoOptimal { get; set; } = true;
    }
}
