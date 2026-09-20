
using Lab1.Interfaces;
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
            var validator = new LibraryItemValidator();
            var searchService = new LibrarySearchService(repository);
            var bookService = new BookService(repository,validator);
            var newspaperService = new NewspaperService(repository,validator);
            var almanacService = new AlmanacService(repository,validator);
            var printer = new LibraryPrinter();
            
            var view = new LibraryConsoleView(validator);

            var randomGenerator = new LibraryRandomGenerator(bookService, newspaperService, almanacService);


            var initializator = new LibraryInitializator(repository,randomGenerator);
            await initializator.SeedAsync();
          
            
            var app = new LibraryAppController(repository, searchService, bookService, newspaperService, almanacService, randomGenerator, printer, view);

            await app.RunAsync();
        }
    }
}