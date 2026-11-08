using LAB_2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2.Records
{
    public record CommandContext(
        ICommand Command,
        IWarUnit Target,
        int MapWidth,
        int MapHeight
        );
}
