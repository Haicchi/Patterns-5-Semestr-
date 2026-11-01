using LAB_2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Interfaces
{
    public interface IBattleMediator
    {
        void Notify(object sender, BattleEvent battleEvent, object? payload = null);
    }
}
