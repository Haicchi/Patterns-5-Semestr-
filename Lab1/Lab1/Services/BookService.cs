using Lab1.Interfaces;
using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class BookService : IBookService
    {
        private readonly ILibraryRepository _repository;
        private readonly ILibraryItemValidator _validator;

        public BookService(ILibraryRepository repository, ILibraryItemValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<Book> CreateBookAsync(string title, string author, string genre, int year, string publisher, int pageCount)
        {
            var (isValid, error) = _validator.ValidateBook(title, publisher, year, author, genre, pageCount);
            if (!isValid)
            {
                throw new ArgumentException(error);
            }

            var book = new Book
            {
                Title = title.Trim(),
                Author = author.Trim(),
                Genre = genre.Trim(),
                PublishYear = year,
                Publisher = publisher.Trim(),
                PageCount = pageCount
            };

            await _repository.AddAsync(book);
            return book;
        }

        public async Task UpdateBookAsync(int id, string? newTitle = null, int? newPublishYear = null, int? newPageCount = null)
        {
            var book = await GetBookOrThrowAsync(id);

            var updatedTitle = newTitle?.Trim() ?? book.Title;
            var updatedYear = newPublishYear ?? book.PublishYear;
            var updatedPages = newPageCount ?? book.PageCount;

            var (isValid, error) = _validator.ValidateBook(
                updatedTitle,
                book.Publisher,
                updatedYear,
                book.Author,
                book.Genre,
                updatedPages
            );

            if (!isValid)
            {
                throw new ArgumentException(error);
            }

            book.Title = updatedTitle;
            book.PublishYear = updatedYear;
            book.PageCount = updatedPages;

            await _repository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await GetBookOrThrowAsync(id);
            await _repository.DeleteAsync(id);
        }

        private async Task<Book> GetBookOrThrowAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
            {
                throw new KeyNotFoundException($"Книгу з ID {id} не знайдено.");
            }

            if (item is not Book book)
            {
                throw new InvalidOperationException($"Елемент з ID {id} не є книгою.");
            }

            return book;
        }
    }
}