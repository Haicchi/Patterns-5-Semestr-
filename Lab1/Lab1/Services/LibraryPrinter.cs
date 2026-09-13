using Lab1.Interfaces;
using Lab1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class LibraryPrinter:ILibraryPrinter
    {
        public void PrintCatalogGrouped(IEnumerable<LibraryItem> items)
        {
            var list = items.ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("Каталог порожній.");
                return;
            }

            var grouped = list.GroupBy(x => x.GetType().Name);

            foreach (var group in grouped)
            {
                Console.WriteLine($"\n--- РІЗНОВИД: {group.Key.ToUpper()} ({group.Count()} шт.) ---");
                foreach (var item in group)
                {
                    PrintItem(item);
                }
            }
        }

        public void PrintItem(LibraryItem item)
        {
            switch (item)
            {
                case Book b:
                    Console.WriteLine($"[ID: {b.Id}] Книга: \"{b.Title}\" | Автор: {b.Author} | Жанр: {b.Genre} | {b.PublishYear} р. | Стор: {b.PageCount} | Вид: {b.Publisher}");
                    break;

                case Newspaper n:
                    Console.WriteLine($"[ID: {n.Id}] Газета: \"{n.Title}\" (№{n.IssueNumber}) | Вихід: {n.ReleaseDate:yyyy-MM-dd} | Вид: {n.Publisher} | Колонок: {n.Columns.Count}");
                    foreach (var col in n.Columns)
                        Console.WriteLine($"      -> [ID колонки: {col.Id}] \"{col.Title}\" (Журналіст: {col.JournalistName})");
                    break;

                case Almanac a:
                    Console.WriteLine($"[ID: {a.Id}] Альманах: \"{a.Title}\" | Жанр: {a.Genre} | {a.PublishYear} р. | Стор: {a.PageCount} | Вид: {a.Publisher}");
                    foreach (var b in a.Books)
                        Console.WriteLine($"      -> [ID твору: {b.Id}] \"{b.Title}\" (Автор: {b.Author})");
                    break;
            }
        }
    }
}
