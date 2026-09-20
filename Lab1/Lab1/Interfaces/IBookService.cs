using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface IBookService
    {
        Task<Book> CreateBookAsync(string title, string author, string genre, int year, string publisher, int pageCount);
        Task UpdateBookAsync(int id, string? newTitle = null, int? newPublishYear = null, int? newPageCount = null);
        Task DeleteBookAsync(int id);
    }
}
