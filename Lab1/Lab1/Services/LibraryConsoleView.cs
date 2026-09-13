using Lab1.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class LibraryConsoleView:ILibraryConsoleView
    {
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
            var title = ReadString("Назва: ");
            var author = ReadString("Автор: ");
            var genre = ReadString("Жанр: ");
            var year = ReadInt("Рік видання: ", 1, DateTime.UtcNow.Year);
            var pub = ReadString("Видавництво: ");
            var pages = ReadInt("Кількість сторінок: ", 1);
            return (title, author, genre, year, pub, pages);
        }

        public (string Title, int IssueNumber, DateTime ReleaseDate, string Publisher) PromptNewspaperData()
        {
            var title = ReadString("Назва газети: ");
            var issue = ReadInt("Номер випуску: ", 1);
            var date = ReadDate("Дата виходу (рррр-мм-дд): ");
            var pub = ReadString("Видавництво: ");
            return (title, issue, date, pub);
        }

        public (string Title, string Genre, int Year, string Publisher, int Pages) PromptAlmanacData()
        {
            var title = ReadString("Назва альманаху: ");
            var genre = ReadString("Жанр: ");
            var year = ReadInt("Рік видання: ", 1, DateTime.UtcNow.Year);
            var pub = ReadString("Видавництво: ");
            var pages = ReadInt("Кількість сторінок: ", 1);
            return (title, genre, year, pub, pages);
        }

        public (string SubAction, string Title, string Journalist, int ColumnId) PromptColumnAction()
        {
            Console.WriteLine("1. Додати колонку | 2. Видалити колонку");
            Console.Write("Дія: ");
            var action = Console.ReadLine() ?? "";

            if (action == "1")
            {
                var title = ReadString("Назва колонки: ");
                var journalist = ReadString("Журналіст: ");
                return (action, title, journalist, 0);
            }
            if (action == "2")
            {
                var id = ReadInt("Введіть ID колонки для видалення: ", 1);
                return (action, "", "", id);
            }
            return (action, "", "", 0);
        }

        public (string SubAction, string Title, string Author, int BookId) PromptAlmanacBookAction()
        {
            Console.WriteLine("1. Додати твір | 2. Видалити твір");
            Console.Write("Дія: ");
            var action = Console.ReadLine() ?? "";

            if (action == "1")
            {
                var title = ReadString("Назва твору: ");
                var author = ReadString("Автор: ");
                return (action, title, author, 0);
            }
            if (action == "2")
            {
                var id = ReadInt("Введіть ID твору для видалення: ", 1);
                return (action, "", "", id);
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
                var year = ReadInt("Рік: ", 1, DateTime.UtcNow.Year);
                return (type, "", year);
            }

            var query = ReadString("Текст для пошуку: ");
            return (type, query, 0);
        }

        public int PromptId(string prompt = "Введіть ID об'єкта: ") => ReadInt(prompt, 1);

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
                Console.WriteLine("Значення не може бути порожнім.");
            }
        }

        private int ReadInt(string prompt, int min = 1, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int val) && val >= min && val <= max)
                    return val;
                Console.WriteLine($"Введіть число від {min} до {max}.");
            }
        }

        private DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    return date;
                Console.WriteLine("Формат: РРРР-ММ-ДД.");
            }
        }
    }
}
