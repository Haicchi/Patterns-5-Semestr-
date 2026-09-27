using Lab1.Interfaces;
using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class AlmanacCreator:IItemCreator
    {
        private readonly IAlmanacService _almanacService;
        private readonly ILibraryConsoleView _view;

        public AlmanacCreator(IAlmanacService almanacService, ILibraryConsoleView view)
        {
            _almanacService = almanacService;
            _view = view;
        }

        public async Task<LibraryItem> CreateAsync()
        {
            var d = _view.PromptAlmanacData();
            return await _almanacService.CreateAlmanacAsync(d.Title, d.Genre, d.Year, d.Publisher, d.Pages);
        }
    }
}
