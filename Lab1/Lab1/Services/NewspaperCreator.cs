using Lab1.Interfaces;
using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class NewspaperCreator : IItemCreator
    {
        private readonly INewspaperService _newspaperService;
        private readonly ILibraryConsoleView _view;

        public NewspaperCreator(INewspaperService newspaperService, ILibraryConsoleView view)
        {
            _newspaperService = newspaperService;
            _view = view;
        }

        public async Task<LibraryItem> CreateAsync()
        {
            var d = _view.PromptNewspaperData();
            return await _newspaperService.CreateNewspaperAsync(d.Title, d.IssueNumber, d.ReleaseDate, d.Publisher);
        }
    }

}
