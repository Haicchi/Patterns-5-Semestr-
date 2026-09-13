using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    public class NewspaperColumn
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string JournalistName { get; set; } = string.Empty;
        public int NewspaperId { get; set; }
    }
}
