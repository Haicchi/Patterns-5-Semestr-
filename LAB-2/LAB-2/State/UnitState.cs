using LAB_2.Model.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.State
{
    public abstract class UnitState
    {
        protected Character _unit = null!;

        public void SetContext(Character unit)
        {
            _unit = unit;
        }

        public abstract string Name { get; }
        public abstract int Duration { get; set; }
        public abstract bool CanTakeTurn();
        public abstract void OnTurnStart();
        public abstract void OnTurnEnd();
    }
}
