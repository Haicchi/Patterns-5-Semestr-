using Lab1.Factories;
using Lab1.Interfaces;
using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class LibraryItemFactory : ILibraryItemFactory
    {
        private readonly BookCreator _bookCreator;
        private readonly NewspaperCreator _newspaperCreator;
        private readonly AlmanacCreator _almanacCreator;

        public LibraryItemFactory(
            BookCreator bookCreator,
            NewspaperCreator newspaperCreator,
            AlmanacCreator almanacCreator)
        {
            _bookCreator = bookCreator;
            _newspaperCreator = newspaperCreator;
            _almanacCreator = almanacCreator;
        }

        public Task<LibraryItem> CreateItemAsync(string typeChoice)
        {
            IItemCreator creator = typeChoice switch
            {
                "1" => _bookCreator,
                "2" => _newspaperCreator,
                "3" => _almanacCreator,
                _ => throw new ArgumentException("Невірний тип об'єкта.")
            };

            return creator.CreateAsync();
        }
    }
}
