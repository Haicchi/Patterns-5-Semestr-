using Lab1.Interfaces;
using Lab1.Model;
using Lab1.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LibraryAppController
{
    private readonly ILibraryRepository _repository;
    private readonly ILibrarySearchService _searchService;
    private readonly IBookService _bookService;
    private readonly INewspaperService _newspaperService;
    private readonly IAlmanacService _almanacService;
    private readonly ILibraryRandomGenerator _randomGenerator;
    private readonly ILibraryPrinter _printer;
    private readonly ILibraryConsoleView _view;

    public LibraryAppController(
        ILibraryRepository repository,
        ILibrarySearchService searchService,
        IBookService bookService,
        INewspaperService newspaperService,
        IAlmanacService almanacService,
        ILibraryRandomGenerator randomGenerator,
        ILibraryPrinter printer,
        ILibraryConsoleView view)
    {
        _repository = repository;
        _searchService = searchService;
        _bookService = bookService;
        _newspaperService = newspaperService;
        _almanacService = almanacService;
        _randomGenerator = randomGenerator;
        _printer = printer;
        _view = view;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            var choice = _view.ShowMainMenuAndGetChoice();
            if (choice == "0") return;

            try
            {
                switch (choice)
                {
                    case "1":
                        _printer.PrintCatalogGrouped(await _repository.GetAllAsync());
                        break;
                    case "2":
                        await HandleAddAsync();
                        break;
                    case "3":
                        var random = await _randomGenerator.GenerateRandomItemAsync();
                        _view.ShowSuccess($"Створено: {random.GetType().Name} \"{random.Title}\" (ID: {random.Id})");
                        break;
                    case "4":
                        await HandleEditAsync();
                        break;
                    case "5":
                        await HandleColumnsAsync();
                        break;
                    case "6":
                        await HandleAlmanacBooksAsync();
                        break;
                    case "7":
                        await _repository.DeleteAsync(_view.PromptId());
                        _view.ShowSuccess("Об'єкт успішно видалено.");
                        break;
                    case "8":
                        await HandleSearchAsync();
                        break;
                    default:
                        _view.ShowError("Невідомий пункт меню.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }

    private async Task HandleAddAsync()
    {
        var type = _view.PromptItemType();
        if (type == "1")
        {
            var d = _view.PromptBookData();
            var b = await _bookService.CreateBookAsync(d.Title, d.Author, d.Genre, d.Year, d.Publisher, d.Pages);
            _view.ShowSuccess($"Книгу створено з ID: {b.Id}");
        }
        else if (type == "2")
        {
            var d = _view.PromptNewspaperData();
            var n = await _newspaperService.CreateNewspaperAsync(d.Title, d.IssueNumber, d.ReleaseDate, d.Publisher);
            _view.ShowSuccess($"Газету створено з ID: {n.Id}");
        }
        else if (type == "3")
        {
            var d = _view.PromptAlmanacData();
            var a = await _almanacService.CreateAlmanacAsync(d.Title, d.Genre, d.Year, d.Publisher, d.Pages);
            _view.ShowSuccess($"Альманах створено з ID: {a.Id}");
        }
    }

    private async Task HandleEditAsync()
    {
        int id = _view.PromptId("Введіть ID об'єкта для редагування: ");
        var item = await _repository.GetByIdAsync(id);

        if (item == null)
        {
            _view.ShowError($"Об'єкт з ID {id} не знайдено.");
            return;
        }

        switch (item)
        {
            case Book:
                var (bChoice, bTitle, bYear, _) = _view.PromptBookUpdateField();
                switch (bChoice)
                {
                    case "1":
                        await _bookService.UpdateBookAsync(id, newTitle: bTitle);
                        _view.ShowSuccess("Назву книги успішно оновлено.");
                        break;
                    case "2":
                        await _bookService.UpdateBookAsync(id, newPublishYear: bYear);
                        _view.ShowSuccess("Рік видання книги успішно оновлено.");
                        break;
                    case "3":
                        await _bookService.UpdateBookAsync(id, newPageCount: bYear);
                        _view.ShowSuccess("Кількість сторінок книги успішно оновлено.");
                        break;
                    case "0":
                        break;
                    default:
                        _view.ShowError("Невідомий пункт меню.");
                        break;
                }
                break;

            case Almanac:
                var (aChoice, aStr, aPages) = _view.PromptAlmanacUpdateField();
                switch (aChoice)
                {
                    case "1":
                        await _almanacService.UpdateAlmanacAsync(id, newTitle: aStr);
                        _view.ShowSuccess("Назву альманаху успішно оновлено.");
                        break;
                    case "2":
                        await _almanacService.UpdateAlmanacAsync(id, newGenre: aStr);
                        _view.ShowSuccess("Жанр альманаху успішно оновлено.");
                        break;
                    case "3":
                        await _almanacService.UpdateAlmanacAsync(id, newPageCount: aPages);
                        _view.ShowSuccess("Кількість сторінок альманаху успішно оновлено.");
                        break;
                    case "0":
                        break;
                    default:
                        _view.ShowError("Невідомий пункт меню.");
                        break;
                }
                break;

            case Newspaper:
                var (nChoice, nTitle, nDate) = _view.PromptNewspaperUpdateField();
                switch (nChoice)
                {
                    case "1":
                        await _newspaperService.UpdateNewspaperAsync(id, newTitle: nTitle);
                        _view.ShowSuccess("Назву газети успішно оновлено.");
                        break;
                    case "2":
                        await _newspaperService.UpdateNewspaperAsync(id, newReleaseDate: nDate);
                        _view.ShowSuccess("Дату виходу газети успішно оновлено.");
                        break;
                    case "0":
                        break;
                    default:
                        _view.ShowError("Невідомий пункт меню.");
                        break;
                }
                break;

            default:
                _view.ShowError("Невідомий тип елемента бібліотеки.");
                break;
        }
    }

    private async Task HandleColumnsAsync()
    {
        int newsId = _view.PromptId("Введіть ID газети: ");
        var action = _view.PromptColumnAction();

        if (action.SubAction == "1")
        {
            await _newspaperService.AddColumnToNewspaperAsync(newsId, action.Title, action.Journalist);
            _view.ShowSuccess("Колонку додано.");
        }
        else if (action.SubAction == "2")
        {
            await _newspaperService.RemoveColumnFromNewspaperAsync(newsId, action.ColumnId);
            _view.ShowSuccess("Колонку видалено.");
        }
    }

    private async Task HandleAlmanacBooksAsync()
    {
        int almId = _view.PromptId("Введіть ID альманаху: ");
        var action = _view.PromptAlmanacBookAction();

        switch (action.SubAction)
        {
            case "1":
                await _almanacService.AddExistingBookToAlmanacAsync(almId, action.BookId);
                _view.ShowSuccess("Існуючу книгу успішно прив'язано до альманаху.");
                break;
            case "2":
                await _almanacService.AddBookToAlmanacAsync(almId, action.Title, action.Author);
                _view.ShowSuccess("Новий твір успішно створено та додано до альманаху.");
                break;
            case "3":
                await _almanacService.RemoveBookFromAlmanacAsync(almId, action.BookId);
                _view.ShowSuccess("Твір вилучено зі складу альманаху.");
                break;
            default:
                _view.ShowError("Дію скасовано або обрано невірний пункт.");
                break;
        }
    }

    private async Task HandleSearchAsync()
    {
        var search = _view.PromptSearchData();
        List<LibraryItem> results = search.SearchType switch
        {
            "1" => await _searchService.SearchByTitleAsync(search.Query),
            "2" => await _searchService.SearchByPublisherAsync(search.Query),
            "3" => await _searchService.SearchByYearAsync(search.Year),
            "4" => await _searchService.SearchByAuthorAsync(search.Query),
            _ => new List<LibraryItem>()
        };

        _view.ShowSuccess($"Знайдено: {results.Count}");
        foreach (var r in results) _printer.PrintItem(r);
    }
}