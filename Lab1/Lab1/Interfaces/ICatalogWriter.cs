using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface ICatalogWriter
    {
        Task AddAsync(LibraryItem item);
        Task UpdateAsync(LibraryItem item);

        Task DeleteAsync(int id);
    }
}
