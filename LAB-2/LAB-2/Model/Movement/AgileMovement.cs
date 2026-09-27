using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Model.Movement
{
    public class AgileMovement:IMovement
    {
        public MovementType Type => MovementType.AgileSprint;
        public string Desc => "Лісовий спринт";
        public int MoveDistance => 4; 

        public void Move(int fromX, int fromY, int toX, int toY) =>
            Console.WriteLine($" безшумно перебігає з ({fromX},{fromY}) у ({toX},{toY})");
    }
}
