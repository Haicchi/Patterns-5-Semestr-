using Lab1.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class LibraryConsoleView : ILibraryConsoleView
    {
        private readonly ILibraryItemValidator _validator;

        public LibraryConsoleView(ILibraryItemValidator validator)
        {
            _validator = validator;
        }

        public string ShowMainMenuAndGetChoice()
        {
            Console.WriteLine("\n=== МЕНЮ БІБЛІОТЕКИ ===");
            Console.WriteLine("1. Показати каталог (з групуванням за типом)");
            Console.WriteLine("2. Додати елемент");
            Console.WriteLine("3. Додати випадковий об'єкт");
            Console.WriteLine("4. Редагувати базову інформацію");
            Console.WriteLine("5. Керування складом газети (колонки)");
            Console.WriteLine("6. Керування складом альманаху (твори)");
            Console.WriteLine("7. Видалити об'єкт за ID");
            Console.WriteLine("8. Пошук");
            Console.WriteLine("0. Вихід");
            Console.Write("Оберіть пункт: ");

            return Console.ReadLine() ?? "";
        }

        public string PromptItemType()
        {
            Console.WriteLine("1. Книга | 2. Газета | 3. Альманах");
            Console.Write("Вибір: ");
            return Console.ReadLine() ?? "";
        }

        public (string Title, string Author, string Genre, int Year, string Publisher, int Pages) PromptBookData()
        {
            while (true)
            {
                var title = ReadString("Назва: ");
                var author = ReadString("Автор: ");
                var genre = ReadString("Жанр: ");
                var year = ReadInt("Рік видання: ");
                var pub = ReadString("Видавництво: ");
                var pages = ReadInt("Кількість сторінок: ");

                var (isValid, error) = _validator.ValidateBook(title, pub, year, author, genre, pages);
                if (isValid)
                {
                    return (title, author, genre, year, pub, pages);
                }

                ShowError(error ?? "Помилка валідації книги.");
                Console.WriteLine("Будь ласка, введіть дані книги знову.\n");
            }
        }

        public (string Title, int IssueNumber, DateTime ReleaseDate, string Publisher) PromptNewspaperData()
        {
            while (true)
            {
                var title = ReadString("Назва газети: ");
                var issue = ReadInt("Номер випуску: ");
                var date = ReadDate("Дата виходу (рррр-мм-дд): ");
                var pub = ReadString("Видавництво: ");

                var (isValid, error) = _validator.ValidateNewspaper(title, pub, date.Year, issue, date);
                if (isValid)
                {
                    return (title, issue, date, pub);
                }

                ShowError(error ?? "Помилка валідації газети.");
                Console.WriteLine("Будь ласка, введіть дані газети знову.\n");
            }
        }

        public (string Title, string Genre, int Year, string Publisher, int Pages) PromptAlmanacData()
        {
            while (true)
            {
                var title = ReadString("Назва альманаху: ");
                var genre = ReadString("Жанр: ");
                var year = ReadInt("Рік видання: ");
                var pub = ReadString("Видавництво: ");
                var pages = ReadInt("Кількість сторінок: ");

                var (isValidBase, errorBase) = _validator.ValidateBaseItem(title, pub, year);
                if (!isValidBase)
                {
                    ShowError(errorBase!);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(genre))
                {
                    ShowError("Жанр альманаху не може бути порожнім.");
                    continue;
                }

                if (pages <= 0)
                {
                    ShowError("Кількість сторінок має бути більшою за нуль.");
                    continue;
                }

                return (title, genre, year, pub, pages);
            }
        }

        public (string SubAction, string Title, string Journalist, int ColumnId) PromptColumnAction()
        {
            Console.WriteLine("1. Додати колонку | 2. Видалити колонку");
            Console.Write("Дія: ");
            var action = Console.ReadLine() ?? "";

            if (action == "1")
            {
                while (true)
                {
                    var title = ReadString("Назва колонки: ");
                    var journalist = ReadString("Журналіст: ");

                    var (isValid, error) = _validator.ValidateColumn(title, journalist);
                    if (isValid)
                        return (action, title, journalist, 0);

                    ShowError(error ?? "Некоректні дані колонки.");
                }
            }
            if (action == "2")
            {
                var id = ReadInt("Введіть ID колонки для видалення: ");
                return (action, "", "", id);
            }
            return (action, "", "", 0);
        }

        public (string SubAction, string Title, string Author, int BookId) PromptAlmanacBookAction()
        {
            Console.WriteLine("1. Прив'язати існуючу книгу за ID");
            Console.WriteLine("2. Створити та додати новий твір");
            Console.WriteLine("3. Видалити твір зі складу альманаху");
            Console.Write("Оберіть дію: ");
            var action = Console.ReadLine() ?? "";

            if (action == "1")
            {
                var bookId = ReadInt("Введіть ID існуючої книги: ");
                return (action, "", "", bookId);
            }

            if (action == "2")
            {
                while (true)
                {
                    var title = ReadString("Назва твору: ");
                    var author = ReadString("Автор: ");

                    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
                    {
                        ShowError("Назва та автор твору не можуть бути порожніми.");
                        continue;
                    }

                    return (action, title, author, 0);
                }
            }

            if (action == "3")
            {
                var bookId = ReadInt("Введіть ID твору для видалення з альманаху: ");
                return (action, "", "", bookId);
            }

            return (action, "", "", 0);
        }

        public (string SearchType, string Query, int Year) PromptSearchData()
        {
            Console.WriteLine("1. Назва | 2. Видавництво | 3. Рік | 4. Автор/Журналіст");
            Console.Write("Критерій: ");
            var type = Console.ReadLine() ?? "";

            if (type == "3")
            {
                var year = ReadInt("Рік: ");
                return (type, "", year);
            }

            var query = ReadString("Текст для пошуку: ");
            return (type, query, 0);
        }

        public (string FieldChoice, string? StringVal, int? IntVal, DateTime? DateVal) PromptBookUpdateField()
        {
            Console.WriteLine("\nОберіть поле для редагування книги:");
            Console.WriteLine("1. Назва");
            Console.WriteLine("2. Рік видання");
            Console.WriteLine("3. Кількість сторінок");
            Console.WriteLine("0. Скасувати");
            Console.Write("Вибір: ");
            var choice = Console.ReadLine() ?? "";

            return choice switch
            {
                "1" => (choice, ReadString("Нова назва: "), null, null),
                "2" => (choice, null, ReadInt("Новий рік видання: "), null),
                "3" => (choice, null, ReadInt("Нова кількість сторінок: "), null),
                _ => (choice, null, null, null)
            };
        }

        public (string FieldChoice, string? StringVal, int? IntVal) PromptAlmanacUpdateField()
        {
            Console.WriteLine("\nОберіть поле для редагування альманаху:");
            Console.WriteLine("1. Назва");
            Console.WriteLine("2. Жанр");
            Console.WriteLine("3. Кількість сторінок");
            Console.WriteLine("0. Скасувати");
            Console.Write("Вибір: ");
            var choice = Console.ReadLine() ?? "";

            return choice switch
            {
                "1" => (choice, ReadString("Нова назва: "), null),
                "2" => (choice, ReadString("Новий жанр: "), null),
                "3" => (choice, null, ReadInt("Нова кількість сторінок: ")),
                _ => (choice, null, null)
            };
        }

        public (string FieldChoice, string? StringVal, DateTime? DateVal) PromptNewspaperUpdateField()
        {
            Console.WriteLine("\nОберіть поле для редагування газети:");
            Console.WriteLine("1. Назва");
            Console.WriteLine("2. Дата виходу");
            Console.WriteLine("0. Скасувати");
            Console.Write("Вибір: ");
            var choice = Console.ReadLine() ?? "";

            return choice switch
            {
                "1" => (choice, ReadString("Нова назва: "), null),
                "2" => (choice, null, ReadDate("Нова дата виходу (рррр-мм-дд): ")),
                _ => (choice, null, null)
            };
        }

        public int PromptId(string prompt = "Введіть ID об'єкта: ") => ReadInt(prompt);

        public void ShowSuccess(string message) => Console.WriteLine(message);

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Помилка: {message}");
            Console.ResetColor();
        }

        private string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();
                ShowError("Значення не може бути порожнім.");
            }
        }

        private int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int val))
                    return val;
                ShowError("Будь ласка, введіть коректне ціле число.");
            }
        }

        private DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    return date;
                ShowError("Формат: РРРР-ММ-ДД (наприклад, 2024-05-18).");
            }
        }
    }
}