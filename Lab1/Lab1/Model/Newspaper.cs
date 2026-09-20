using Lab1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    public class Newspaper:LibraryItem

    {
        
        public int IssueNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<NewspaperColumn> Columns { get; set; } = new();

        public override IEnumerable<string> GetContributors()
        {
            return Columns.Select(a => a.JournalistName).Where(a => !string.IsNullOrEmpty(a)).Distinct();
        }
    }
}
