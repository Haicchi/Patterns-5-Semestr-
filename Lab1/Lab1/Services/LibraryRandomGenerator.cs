using Lab1.Interfaces;
using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class LibraryRandomGenerator:ILibraryRandomGenerator
    {
        private static readonly Random _random = new();
        private readonly IBookService _bookService;
        private readonly INewspaperService _newspaperService;
        private readonly IAlmanacService _almanacService;

        private static readonly string[] Titles = { "Світанок", "Вітер змін", "Хроніки часу", "Останній маяк", "Золота доба" };
        private static readonly string[] Authors = { "Олександр Олесь", "Василь Стус", "Ліна Костенко", "Павло Тичина", "Максим Рильський" };
        private static readonly string[] Publishers = { "Фоліо", "Ранок", "А-ба-ба-га-ла-ма-га", "Віват", "Основи" };
        private static readonly string[] Genres = { "Фантастика", "Драма", "Детектив", "Історичний", "Пригоди" };

        public LibraryRandomGenerator(IBookService bookService, INewspaperService newspaperService, IAlmanacService almanacService)
        {
            _bookService = bookService;
            _newspaperService = newspaperService;
            _almanacService = almanacService;
        }

        public async Task<LibraryItem> GenerateRandomItemAsync()
        {
            int typeChoice = _random.Next(3);

            return typeChoice switch
            {
                0 => await GenerateBookAsync(),
                1 => await GenerateNewspaperAsync(),
                _ => await GenerateAlmanacAsync()
            };
        }

        private async Task<Book> GenerateBookAsync()
        {
            string title = Titles[_random.Next(Titles.Length)] + " " + _random.Next(1, 100);
            string author = Authors[_random.Next(Authors.Length)];
            string genre = Genres[_random.Next(Genres.Length)];
            string publisher = Publishers[_random.Next(Publishers.Length)];
            int year = _random.Next(1950, 2025);
            int pages = _random.Next(50, 800);

            return await _bookService.CreateBookAsync(title, author, genre, year, publisher, pages);
        }

        private async Task<Newspaper> GenerateNewspaperAsync()
        {
            string title = "Вісник #" + _random.Next(1, 50);
            int issue = _random.Next(1, 500);
            string publisher = Publishers[_random.Next(Publishers.Length)];
            var releaseDate = DateTime.UtcNow.AddDays(-_random.Next(1, 365));

            var columns = new List<NewspaperColumn>
            {
                new NewspaperColumn { Title = "Події дня", JournalistName = Authors[_random.Next(Authors.Length)] },
                new NewspaperColumn { Title = "Спорт", JournalistName = Authors[_random.Next(Authors.Length)] }
            };

            return await _newspaperService.CreateNewspaperAsync(title, issue, releaseDate, publisher, columns);
        }

        private async Task<Almanac> GenerateAlmanacAsync()
        {
            string title = "Збірка творів #" + _random.Next(1, 100);
            string genre = Genres[_random.Next(Genres.Length)];
            string publisher = Publishers[_random.Next(Publishers.Length)];
            int year = _random.Next(1990, 2025);
            int pages = _random.Next(150, 600);

            var books = new List<AlmanacBook>
            {
                new AlmanacBook { Title = Titles[_random.Next(Titles.Length)], Author = Authors[_random.Next(Authors.Length)] },
                new AlmanacBook { Title = Titles[_random.Next(Titles.Length)], Author = Authors[_random.Next(Authors.Length)] }
            };

            return await _almanacService.CreateAlmanacAsync(title, genre, year, publisher, pages, books);
        }
    }
}