using System;
using System.Collections.Generic;
using LAB_2.Interfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Movement
{
    public class HeavyMovement:IMovement
    {
        public MovementType Type => MovementType.HeavyMove;
        public string Desc => "Дварвська хода";
        public int MoveDistance => 1; 

        public void Move(int fromX, int fromY, int toX, int toY) =>
            Console.WriteLine($" безшумно перебігає з ({fromX},{fromY}) у ({toX},{toY})");

    }
}
