using Lab1.Model;
using System.Threading.Tasks;

namespace Lab1.Interfaces
{
    public interface ILibraryRandomGenerator
    {
        Task<LibraryItem> GenerateRandomItemAsync();
    }
}