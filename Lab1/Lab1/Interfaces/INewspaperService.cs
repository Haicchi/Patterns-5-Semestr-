using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface INewspaperService
    {
        Task<Newspaper> CreateNewspaperAsync(string title, int issueNumber, DateTime releaseDate, string publisher, List<NewspaperColumn>? initialColumns = null);
        Task UpdateNewspaperAsync(int id, string title, int issueNumber, DateTime releaseDate, string publisher);
        Task AddColumnToNewspaperAsync(int newspaperId, string columnTitle, string journalist);
        Task RemoveColumnFromNewspaperAsync(int newspaperId, int columnId);
        Task DeleteNewspaperAsync(int id);
    }
}