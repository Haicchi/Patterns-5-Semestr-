using Lab1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public sealed class LibraryItemValidator:ILibraryItemValidator
    {

        private const int MinYear = 1450;

        private LibraryItemValidator() { }

        private static LibraryItemValidator _instance;

        public static LibraryItemValidator GetInstance()
        {
            if(_instance == null)
            {
                _instance = new LibraryItemValidator();
            }
            return _instance;
        }

        public (bool IsValid, string? Error) ValidateBaseItem(string title, string publisher, int publishYear)
        {
            if (string.IsNullOrWhiteSpace(title))
                return (false, "Назва видання не може бути порожньою.");

            if (string.IsNullOrWhiteSpace(publisher))
                return (false, "Видавництво має бути обов'язково вказане.");

            int currentYear = DateTime.Now.Year;
            if (publishYear < MinYear || publishYear > currentYear)
                return (false, $"Рік видання повинен бути в межах від {MinYear} до {currentYear}.");

            return (true, null);
        }

        public (bool IsValid, string? Error) ValidateBook(string title, string publisher, int publishYear, string author, string genre, int pages)
        {
            var baseValidation = ValidateBaseItem(title, publisher, publishYear);
            if (!baseValidation.IsValid)
                return baseValidation;

            if (string.IsNullOrWhiteSpace(author))
                return (false, "Автор книги обов'язковий.");

            if (string.IsNullOrWhiteSpace(genre))
                return (false, "Жанр книги обов'язковий.");

            if (pages <= 0)
                return (false, "Кількість сторінок має бути більшою за нуль.");

            return (true, null);
        }

        public (bool IsValid, string? Error) ValidateNewspaper(string title, string publisher, int publishYear, int issueNumber, DateTime releaseDate)
        {
            var baseValidation = ValidateBaseItem(title, publisher, publishYear);
            if (!baseValidation.IsValid)
                return baseValidation;

            if (issueNumber <= 0)
                return (false, "Номер випуску газети повинен бути додатним числом.");

            if (releaseDate.Year != publishYear)
                return (false, "Рік точної дати випуску має збігатися з роком видання.");

            return (true, null);
        }

        public (bool IsValid, string? Error) ValidateColumn(string title, string journalist)
        {
            if (string.IsNullOrWhiteSpace(title))
                return (false, "Заголовок колонки не може бути порожнім.");

            if (string.IsNullOrWhiteSpace(journalist))
                return (false, "Ім'я журналіста не може бути порожнім.");

            return (true, null);
        }
    }
}
