using Lab1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lab1.Model;

namespace Lab1.Services
{
    public class LibrarySearchService : ILibrarySearchService
    {
        private readonly ILibraryRepository _repository;

        public LibrarySearchService(ILibraryRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<LibraryItem>> SearchByAuthorAsync(string author)
        {
            if (string.IsNullOrWhiteSpace(author)) return new List<LibraryItem>();
            var items = await _repository.GetAllAsync();
            return items.Where(item => item.GetContributors().Any(c => c.Contains(author, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        public async Task<List<LibraryItem>> SearchByPublisherAsync(string publisher)
        {
            if (string.IsNullOrWhiteSpace(publisher)) return new List<LibraryItem>();
            var items = await _repository.GetAllAsync();
            return items.Where(item=>item.Publisher.Contains(publisher , StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<List<LibraryItem>> SearchByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return new List<LibraryItem>();
            var items = await _repository.GetAllAsync();
            return items.Where(item=>item.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<List<LibraryItem>> SearchByYearAsync(int year)
        {
            if(year == 0||year == null) return new List<LibraryItem>();
           
            var items = await _repository.GetAllAsync();
            return items.Where(item=>item.PublishYear==year).ToList();  
        }
    }
}
