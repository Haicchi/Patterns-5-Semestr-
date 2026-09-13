using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    public abstract class LibraryItem
    {
        public int Id {  get; set; }
        public string Title { get; set; } = string.Empty;
        public int PublishYear { get; set; }
        public string Publisher { get; set; } = string.Empty;
    }

}

