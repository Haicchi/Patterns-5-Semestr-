using LAB_2.Interfaces;
using LAB_2.Model.Characters;

namespace LAB_2.Commands
{
    public class MoveToCommand : ICommand
    {
        private readonly int _deltaX;
        private readonly int _deltaY;
        public string Name => "Move To";

        public MoveToCommand(int deltaX, int deltaY)
        {
            _deltaX = deltaX;
            _deltaY = deltaY;
        }

        public void Execute(IWarUnit unit, int mapWidth, int mapHeight)
        {
            if (unit is Character character)
            {
                character.MoveBy(_deltaX, _deltaY, mapWidth, mapHeight);
            }
        }
    }
}