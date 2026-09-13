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

        public BookService(ILibraryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Book> CreateBookAsync(string title, string author, string genre, int year, string publisher, int pageCount)
        {
            ValidateBookData(title, author, genre, year, publisher, pageCount);

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

        public async Task UpdateBookAsync(int id, string title, string author, string genre, int year, string publisher, int pageCount)
        {
            ValidateBookData(title, author, genre, year, publisher, pageCount);

            var item = await _repository.GetByIdAsync(id);

            if (item == null)
            {
                throw new KeyNotFoundException($"Книгу з ID {id} не знайдено.");
            }

            if (item is not Book book)
            {
                throw new InvalidOperationException($"Елемент з ID {id} не є книгою.");
            }

            book.Title = title.Trim();
            book.Author = author.Trim();
            book.Genre = genre.Trim();
            book.PublishYear = year;
            book.Publisher = publisher.Trim();
            book.PageCount = pageCount;

            await _repository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
            {
                throw new KeyNotFoundException($"Книгу з ID {id} не знайдено.");
            }

            if (item is not Book)
            {
                throw new InvalidOperationException($"Елемент з ID {id} не є книгою.");
            }

            await _repository.DeleteAsync(id);
        }

        private static void ValidateBookData(string title, string author, string genre, int year, string publisher, int pageCount)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Назва книги не може бути порожньою.", nameof(title));

            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Автор книги не може бути порожнім.", nameof(author));

            if (string.IsNullOrWhiteSpace(genre))
                throw new ArgumentException("Жанр книги не може бути порожнім.", nameof(genre));

            if (string.IsNullOrWhiteSpace(publisher))
                throw new ArgumentException("Видавництво не може бути порожнім.", nameof(publisher));

            if (year < 1 || year > DateTime.UtcNow.Year)
                throw new ArgumentOutOfRangeException(nameof(year), "Рік видання вказано некоректно.");

            if (pageCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageCount), "Кількість сторінок має бути більшою за нуль.");
        }
    }
}