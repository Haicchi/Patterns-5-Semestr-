using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Records
{
    public record DamageContext(
        IWarUnit Target,
        int RawDamage,
        int CalculatedDamage,
        bool IsBlocked = false
    );
}
