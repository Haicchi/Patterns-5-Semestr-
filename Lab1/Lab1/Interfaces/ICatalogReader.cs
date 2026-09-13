using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab1.Model;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface ICatalogReader
    {
        Task<List<LibraryItem>> GetAllAsync();
        Task<LibraryItem?> GetByIdAsync(int id);
        Task<List<T>> GetByTypeAsync<T>() where T : LibraryItem;
    }
}
