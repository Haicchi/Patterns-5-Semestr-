
using Lab1.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            using var context = new AppDbContext();
            await context.Database.EnsureCreatedAsync();

            
            var repository = new LibraryRepository(context);
            var searchService = new LibrarySearchService(repository);
            var bookService = new BookService(repository);
            var newspaperService = new NewspaperService(repository);
            var almanacService = new AlmanacService(repository);
            var printer = new LibraryPrinter();
            var view = new LibraryConsoleView();
            var randomGenerator = new LibraryRandomGenerator(bookService, newspaperService, almanacService);


            var initializator = new LibraryInitializator(repository,randomGenerator);
            await initializator.SeedAsync();
          
            
            var app = new LibraryAppController(repository, searchService, bookService, newspaperService, almanacService, randomGenerator, printer, view);

            await app.RunAsync();
        }
    }
}