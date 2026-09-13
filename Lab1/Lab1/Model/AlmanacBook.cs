using Lab1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    public class AlmanacBook
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PageCount { get; set; }

        public int AlmanacId { get; set; }
        public Almanac Almanac { get; set; } = null!;
    }
}
