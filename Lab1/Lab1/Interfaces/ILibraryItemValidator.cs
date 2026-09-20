using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface ILibraryItemValidator
    {
        (bool IsValid, string? Error) ValidateBaseItem(string title, string publisher, int publishYear);
        (bool IsValid, string? Error) ValidateBook(string title, string publisher, int publishYear, string author, string genre, int pages);
        (bool IsValid, string? Error) ValidateNewspaper(string title, string publisher, int publishYear, int issueNumber, DateTime releaseDate);
        (bool IsValid, string? Error) ValidateColumn(string title, string journalist);
    }
}
