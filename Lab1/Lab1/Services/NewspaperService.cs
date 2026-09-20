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
        private readonly ILibraryItemValidator _validator;

        public NewspaperService(ILibraryRepository repository, ILibraryItemValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<Newspaper> CreateNewspaperAsync(string title, int issueNumber, DateTime releaseDate, string publisher, List<NewspaperColumn>? initialColumns = null)
        {
            var (isValid, error) = _validator.ValidateNewspaper(title, publisher, releaseDate.Year, issueNumber, releaseDate);
            if (!isValid)
            {
                throw new ArgumentException(error);
            }

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

        public async Task UpdateNewspaperAsync(int id, string? newTitle = null, DateTime? newReleaseDate = null)
        {
            var newspaper = await GetNewspaperOrThrowAsync(id);

            var updatedTitle = newTitle?.Trim() ?? newspaper.Title;
            var updatedDate = newReleaseDate.HasValue
                ? DateTime.SpecifyKind(newReleaseDate.Value, DateTimeKind.Utc)
                : newspaper.ReleaseDate;

            var (isValid, error) = _validator.ValidateNewspaper(
                updatedTitle,
                newspaper.Publisher,
                updatedDate.Year,
                newspaper.IssueNumber,
                updatedDate
            );

            if (!isValid)
            {
                throw new ArgumentException(error);
            }

            newspaper.Title = updatedTitle;
            newspaper.ReleaseDate = updatedDate;
            newspaper.PublishYear = updatedDate.Year;

            await _repository.UpdateAsync(newspaper);
        }

        public async Task AddColumnToNewspaperAsync(int newspaperId, string columnTitle, string journalist)
        {
            var (isValid, error) = _validator.ValidateColumn(columnTitle, journalist);
            if (!isValid)
            {
                throw new ArgumentException(error);
            }

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
            {
                throw new KeyNotFoundException($"Колонку з ID {columnId} не знайдено у вказаній газеті.");
            }

            newspaper.Columns.Remove(column);
            await _repository.UpdateAsync(newspaper);
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
    }
}