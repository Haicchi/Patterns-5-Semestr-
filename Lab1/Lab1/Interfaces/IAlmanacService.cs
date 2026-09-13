using Lab1.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface IAlmanacService
    {
        Task<Almanac> CreateAlmanacAsync(string title, string genre, int year, string publisher, int pageCount, List<AlmanacBook>? initialBooks = null);
        Task UpdateAlmanacAsync(int id, string title, string genre, int year, string publisher, int pageCount);
        Task AddBookToAlmanacAsync(int almanacId, string bookTitle, string author);
        Task RemoveBookFromAlmanacAsync(int almanacId, int almanacBookId);
        Task DeleteAlmanacAsync(int id);
    }
}