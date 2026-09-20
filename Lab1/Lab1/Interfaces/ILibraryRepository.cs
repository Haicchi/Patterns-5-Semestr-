using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface ILibraryRepository:ICatalogReader,ICatalogWriter
    {
        Task RemoveColumnAsync(int columnId);
        
    }
}
