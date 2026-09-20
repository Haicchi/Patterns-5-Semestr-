using Lab1.Interfaces;
using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class AlmanacService : IAlmanacService
    {
        private readonly ILibraryRepository _repository;
        private readonly ILibraryItemValidator _validator;

        public AlmanacService(ILibraryRepository repository, ILibraryItemValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<Almanac> CreateAlmanacAsync(string title, string genre, int year, string publisher, int pageCount, List<Book>? initialBooks = null)
        {
            var (isValidBase, errorBase) = _validator.ValidateBaseItem(title, publisher, year);
            if (!isValidBase)
            {
                throw new ArgumentException(errorBase);
            }

            if (string.IsNullOrWhiteSpace(genre))
            {
                throw new ArgumentException("Жанр альманаху не може бути порожнім.", nameof(genre));
            }

            if (pageCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageCount), "Кількість сторінок має бути більшою за нуль.");
            }

            var almanac = new Almanac
            {
                Title = title.Trim(),
                Genre = genre.Trim(),
                PublishYear = year,
                Publisher = publisher.Trim(),
                PageCount = pageCount,
                Books = initialBooks ?? new List<Book>()
            };

            await _repository.AddAsync(almanac);
            return almanac;
        }

        public async Task AddExistingBookToAlmanacAsync(int almanacId, int bookId)
        {
            var almanac = await GetAlmanacOrThrowAsync(almanacId);

            var item = await _repository.GetByIdAsync(bookId);
            if (item == null)
            {
                throw new KeyNotFoundException($"Книгу з ID {bookId} не знайдено.");
            }

            if (item is not Book book)
            {
                throw new InvalidOperationException($"Елемент з ID {bookId} не є книгою.");
            }

            if (!almanac.Books.Any(b => b.Id == bookId))
            {
                almanac.Books.Add(book);
                await _repository.UpdateAsync(almanac);
            }
        }

        public async Task AddBookToAlmanacAsync(int almanacId, string bookTitle, string author)
        {
            var almanac = await GetAlmanacOrThrowAsync(almanacId);

            var (isValid, error) = _validator.ValidateBook(bookTitle, almanac.Publisher, almanac.PublishYear, author, almanac.Genre, 1);
            if (!isValid)
            {
                throw new ArgumentException(error);
            }

            var newBook = new Book
            {
                Title = bookTitle.Trim(),
                Author = author.Trim(),
                Publisher = almanac.Publisher,
                PublishYear = almanac.PublishYear,
                Genre = almanac.Genre,
                PageCount = 1
            };

            almanac.Books.Add(newBook);
            await _repository.UpdateAsync(almanac);
        }

        public async Task UpdateAlmanacAsync(int id, string? newTitle = null, string? newGenre = null, int? newPageCount = null)
        {
            var almanac = await GetAlmanacOrThrowAsync(id);

            var updatedTitle = newTitle?.Trim() ?? almanac.Title;
            var updatedGenre = newGenre?.Trim() ?? almanac.Genre;
            var updatedPages = newPageCount ?? almanac.PageCount;

            var (isValidBase, errorBase) = _validator.ValidateBaseItem(updatedTitle, almanac.Publisher, almanac.PublishYear);
            if (!isValidBase)
            {
                throw new ArgumentException(errorBase);
            }

            if (string.IsNullOrWhiteSpace(updatedGenre))
            {
                throw new ArgumentException("Жанр альманаху не може бути порожнім.", nameof(newGenre));
            }

            if (updatedPages <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newPageCount), "Кількість сторінок має бути більшою за нуль.");
            }

            almanac.Title = updatedTitle;
            almanac.Genre = updatedGenre;
            almanac.PageCount = updatedPages;

            await _repository.UpdateAsync(almanac);
        }

        public async Task RemoveBookFromAlmanacAsync(int almanacId, int bookId)
        {
            var almanac = await GetAlmanacOrThrowAsync(almanacId);

            var bookToRemove = almanac.Books.FirstOrDefault(b => b.Id == bookId);
            if (bookToRemove != null)
            {
                almanac.Books.Remove(bookToRemove);
                await _repository.UpdateAsync(almanac);
            }
        }

        public async Task DeleteAlmanacAsync(int id)
        {
            await GetAlmanacOrThrowAsync(id);
            await _repository.DeleteAsync(id);
        }

        private async Task<Almanac> GetAlmanacOrThrowAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
            {
                throw new KeyNotFoundException($"Альманах з ID {id} не знайдено.");
            }

            if (item is not Almanac almanac)
            {
                throw new InvalidOperationException($"Елемент з ID {id} не є альманахом.");
            }

            return almanac;
        }
    }
}