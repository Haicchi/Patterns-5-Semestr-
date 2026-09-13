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

        public AlmanacService(ILibraryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Almanac> CreateAlmanacAsync(string title, string genre, int year, string publisher, int pageCount, List<AlmanacBook>? initialBooks = null)
        {
            ValidateAlmanacData(title, genre, year, publisher, pageCount);

            var almanac = new Almanac
            {
                Title = title.Trim(),
                Genre = genre.Trim(),
                PublishYear = year,
                Publisher = publisher.Trim(),
                PageCount = pageCount,
                Books = initialBooks ?? new List<AlmanacBook>()
            };

            await _repository.AddAsync(almanac);
            return almanac;
        }

        public async Task UpdateAlmanacAsync(int id, string title, string genre, int year, string publisher, int pageCount)
        {
            ValidateAlmanacData(title, genre, year, publisher, pageCount);

            var almanac = await GetAlmanacOrThrowAsync(id);

            almanac.Title = title.Trim();
            almanac.Genre = genre.Trim();
            almanac.PublishYear = year;
            almanac.Publisher = publisher.Trim();
            almanac.PageCount = pageCount;

            await _repository.UpdateAsync(almanac);
        }

        public async Task AddBookToAlmanacAsync(int almanacId, string bookTitle, string author)
        {
            if (string.IsNullOrWhiteSpace(bookTitle))
                throw new ArgumentException("Назва твору не може бути порожньою.", nameof(bookTitle));

            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Автор твору не може бути порожнім.", nameof(author));

            var almanac = await GetAlmanacOrThrowAsync(almanacId);

            var newBook = new AlmanacBook
            {
                Title = bookTitle.Trim(),
                Author = author.Trim()
            };

            almanac.Books.Add(newBook);
            await _repository.UpdateAsync(almanac);
        }

        public async Task RemoveBookFromAlmanacAsync(int almanacId, int almanacBookId)
        {
            var almanac = await GetAlmanacOrThrowAsync(almanacId);

            var bookToRemove = almanac.Books.FirstOrDefault(b => b.Id == almanacBookId);
            if (bookToRemove == null)
            {
                throw new KeyNotFoundException($"Твір з ID {almanacBookId} не знайдено в цьому альманасі.");
            }

            await _repository.RemoveAlmanacBookAsync(almanacBookId);
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

        private static void ValidateAlmanacData(string title, string genre, int year, string publisher, int pageCount)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Назва альманаху не може бути порожньою.", nameof(title));

            if (string.IsNullOrWhiteSpace(genre))
                throw new ArgumentException("Жанр альманаху не може бути порожнім.", nameof(genre));

            if (string.IsNullOrWhiteSpace(publisher))
                throw new ArgumentException("Видавництво не може бути порожнім.", nameof(publisher));

            if (year < 1 || year > DateTime.UtcNow.Year)
                throw new ArgumentOutOfRangeException(nameof(year), "Рік видання вказано некоректно.");

            if (pageCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageCount), "Кількість сторінок має бути більшою за нуль.");
        }
    }
}