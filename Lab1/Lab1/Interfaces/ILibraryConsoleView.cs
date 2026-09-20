using System;

namespace Lab1.Interfaces
{
    public interface ILibraryConsoleView
    {
        string ShowMainMenuAndGetChoice();
        string PromptItemType();
        (string Title, string Author, string Genre, int Year, string Publisher, int Pages) PromptBookData();
        (string Title, int IssueNumber, DateTime ReleaseDate, string Publisher) PromptNewspaperData();
        (string Title, string Genre, int Year, string Publisher, int Pages) PromptAlmanacData();
        (string SubAction, string Title, string Journalist, int ColumnId) PromptColumnAction();
        (string SubAction, string Title, string Author, int BookId) PromptAlmanacBookAction();

        (string FieldChoice, string? StringVal, int? IntVal, DateTime? DateVal) PromptBookUpdateField();
        (string FieldChoice, string? StringVal, int? IntVal) PromptAlmanacUpdateField();
        (string FieldChoice, string? StringVal, DateTime? DateVal) PromptNewspaperUpdateField();
        (string SearchType, string Query, int Year) PromptSearchData();
        int PromptId(string prompt = "Введіть ID об'єкта: ");
        void ShowSuccess(string message);
        void ShowError(string message);
    }
}