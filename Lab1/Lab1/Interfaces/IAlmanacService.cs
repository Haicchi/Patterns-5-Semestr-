using Lab1.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface IAlmanacService
    {
        Task<Almanac> CreateAlmanacAsync(string title, string genre, int year, string publisher, int pageCount, List<Book>? initialBooks = null);

        Task AddExistingBookToAlmanacAsync(int almanacId, int bookId);

        Task UpdateAlmanacAsync(int id, string? newTitle = null, string? newGenre = null, int? newPageCount = null);
        Task AddBookToAlmanacAsync(int almanacId, string bookTitle, string author);
        Task RemoveBookFromAlmanacAsync(int almanacId, int almanacBookId);
        Task DeleteAlmanacAsync(int id);
    }
}