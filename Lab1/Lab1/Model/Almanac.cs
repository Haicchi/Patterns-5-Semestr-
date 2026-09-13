using Lab1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    public class Almanac:LibraryItem,IHasContributors
    {
        public string Genre { get; set; } = string.Empty;
        public int PageCount { get; set; }

        public List<AlmanacBook> Books { get; set; } = new();

        public IEnumerable<string> GetContributors()
        {
            return Books.Select(b => b.Author).Where(a => !string.IsNullOrEmpty(a)).Distinct();
        }
    }
}
