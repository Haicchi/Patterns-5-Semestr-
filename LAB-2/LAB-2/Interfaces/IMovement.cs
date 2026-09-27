using LAB_2.Model.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Interfaces
{
    public interface IMovement
    {
        MovementType Type { get; }
        string Desc { get; }

        int MoveDistance { get; }

        void Move(int fromX, int fromY, int toX, int toY);

    }
}
