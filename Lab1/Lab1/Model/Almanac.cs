using Lab1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    public class Almanac:LibraryItem,IPrototype<Almanac>
    {
        public string Genre { get; set; } = string.Empty;
        public int PageCount { get; set; }

        public List<Book> Books { get; set; } = new();

        public override IEnumerable<string> GetContributors()
        {
            return Books.Select(b => b.Author).Where(a => !string.IsNullOrEmpty(a)).Distinct();
        }
        public Almanac Clone()
        {
            return new Almanac
            {
                Id = 0,
                Title = this.Title,
                Genre = this.Genre,
                PublishYear = DateTime.UtcNow.Year,
                Publisher = this.Publisher,
                PageCount = this.PageCount,
                Books = this.Books.Select(b => b.Clone()).ToList()
            };
        }

    }
}
