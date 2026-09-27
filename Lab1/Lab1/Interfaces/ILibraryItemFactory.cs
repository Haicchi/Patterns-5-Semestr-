using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface ILibraryItemFactory
    {
        Task<LibraryItem> CreateItemAsync(string typeChoice);
    }
}
