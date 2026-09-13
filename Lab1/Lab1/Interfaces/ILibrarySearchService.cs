using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab1.Model;

namespace Lab1.Interfaces
{
    public interface ILibrarySearchService
    {
        Task<List<LibraryItem>> SearchByTitleAsync(String title);
        Task<List<LibraryItem>> SearchByPublisherAsync(string publisher);
        Task<List<LibraryItem>> SearchByYearAsync(int year);
        Task<List<LibraryItem>> SearchByAuthorAsync(string author);
    }
}
