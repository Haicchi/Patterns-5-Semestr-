using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB_2.Interfaces;

namespace LAB_2.Model.Movement
{
    public class MarchMovement : IMovement
    {
        public MovementType Type => MovementType.March;

        public string Desc => "Піший ход";

        public int MoveDistance => 2;

        public void Move(int fromX, int fromY, int toX, int toY)
        {
            Console.WriteLine($" марширує пішки з ({fromX},{fromY}) у ({toX},{toY})");
        }
    }
}
