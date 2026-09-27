using Lab1.Interfaces;
using Lab1.Model;
using System.Threading.Tasks;

namespace Lab1.Factories
{
    public class BookCreator : IItemCreator
    {
        private readonly IBookService _bookService;
        private readonly ILibraryConsoleView _view;

        public BookCreator(IBookService bookService, ILibraryConsoleView view)
        {
            _bookService = bookService;
            _view = view;
        }

        public async Task<LibraryItem> CreateAsync()
        {
            var d = _view.PromptBookData();
            return await _bookService.CreateBookAsync(d.Title, d.Author, d.Genre, d.Year, d.Publisher, d.Pages);
        }
    }
}