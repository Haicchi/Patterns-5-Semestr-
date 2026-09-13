using Lab1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    public class Book:LibraryItem,IHasContributors
    {
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int PageCount { get; set; }

        public IEnumerable<string> GetContributors()
        {
            return new[] { Author };
        }
    }
}
