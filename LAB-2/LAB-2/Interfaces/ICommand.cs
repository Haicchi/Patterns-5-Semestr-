using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        void Execute(IWarUnit target, int mapWidth, int mapHeight);
    }
}
