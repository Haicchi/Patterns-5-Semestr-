using Lab1.Interfaces;
using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class NewspaperService : INewspaperService
    {
        private readonly ILibraryRepository _repository;

        public NewspaperService(ILibraryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Newspaper> CreateNewspaperAsync(string title, int issueNumber, DateTime releaseDate, string publisher, List<NewspaperColumn>? initialColumns = null)
        {
            ValidateNewspaperData(title, issueNumber, releaseDate, publisher);

            var newspaper = new Newspaper
            {
                Title = title.Trim(),
                IssueNumber = issueNumber,
                ReleaseDate = DateTime.SpecifyKind(releaseDate, DateTimeKind.Utc),
                PublishYear = releaseDate.Year,
                Publisher = publisher.Trim(),
                Columns = initialColumns ?? new List<NewspaperColumn>()
            };

            await _repository.AddAsync(newspaper);
            return newspaper;
        }

        public async Task UpdateNewspaperAsync(int id, string title, int issueNumber, DateTime releaseDate, string publisher)
        {
            ValidateNewspaperData(title, issueNumber, releaseDate, publisher);

            var newspaper = await GetNewspaperOrThrowAsync(id);

            newspaper.Title = title.Trim();
            newspaper.IssueNumber = issueNumber;
            newspaper.ReleaseDate = DateTime.SpecifyKind(releaseDate, DateTimeKind.Utc);
            newspaper.PublishYear = releaseDate.Year;
            newspaper.Publisher = publisher.Trim();

            await _repository.UpdateAsync(newspaper);
        }

        public async Task AddColumnToNewspaperAsync(int newspaperId, string columnTitle, string journalist)
        {
            if (string.IsNullOrWhiteSpace(columnTitle))
                throw new ArgumentException("Назва колонки не може бути порожньою.", nameof(columnTitle));

            if (string.IsNullOrWhiteSpace(journalist))
                throw new ArgumentException("Ім'я журналіста не може бути порожнім.", nameof(journalist));

            var newspaper = await GetNewspaperOrThrowAsync(newspaperId);

            var column = new NewspaperColumn
            {
                Title = columnTitle.Trim(),
                JournalistName = journalist.Trim()
            };

            newspaper.Columns.Add(column);
            await _repository.UpdateAsync(newspaper);
        }

        public async Task RemoveColumnFromNewspaperAsync(int newspaperId, int columnId)
        {
            var newspaper = await GetNewspaperOrThrowAsync(newspaperId);
            var column = newspaper.Columns.FirstOrDefault(c => c.Id == columnId);
            if (column == null)
                throw new KeyNotFoundException($"Колонку з ID {columnId} не знайдено у вказаній газеті.");

            await _repository.RemoveColumnAsync(columnId);
        }

        public async Task DeleteNewspaperAsync(int id)
        {
            await GetNewspaperOrThrowAsync(id);
            await _repository.DeleteAsync(id);
        }

        private async Task<Newspaper> GetNewspaperOrThrowAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
            {
                throw new KeyNotFoundException($"Газету з ID {id} не знайдено.");
            }

            if (item is not Newspaper newspaper)
            {
                throw new InvalidOperationException($"Елемент з ID {id} не є газетою.");
            }

            return newspaper;
        }

        private static void ValidateNewspaperData(string title, int issueNumber, DateTime releaseDate, string publisher)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Назва газети не може бути порожньою.", nameof(title));

            if (string.IsNullOrWhiteSpace(publisher))
                throw new ArgumentException("Видавництво не може бути порожнім.", nameof(publisher));

            if (issueNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(issueNumber), "Номер газети має бути додатним числом.");

            if (releaseDate > DateTime.UtcNow.AddDays(1))
                throw new ArgumentException("Дата виходу газети не може бути з майбутнього.", nameof(releaseDate));
        }
    }
}