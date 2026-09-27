using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Interfaces;

namespace Lab1.Model
{
    public class NewspaperColumn:IPrototype<NewspaperColumn>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string JournalistName { get; set; } = string.Empty;

        public NewspaperColumn Clone()
        {
            return new NewspaperColumn
            {
                Id = 0,
                Title = this.Title,
                JournalistName = this.JournalistName
            };
        }

    }
}
