using System.Net.Http;
using System.Text.Json;
using Lab1.Interfaces;
using Lab1.Model;

namespace Lab1.Services
{
    public class OpenLibraryItemGenerator : ILibraryRandomGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly Random _rnd = new();

        public OpenLibraryItemGenerator(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://openlibrary.org/");
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("LibraryLabApp/1.0");
        }

        public async Task<LibraryItem> GenerateRandomItemAsync()
        {
     
            var subjects = new[] { "fantasy", "history", "science", "programming", "philosophy" };
            var selectedSubject = subjects[_rnd.Next(subjects.Length)];

            var response = await _httpClient.GetAsync($"subjects/{selectedSubject}.json?limit=15");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var works = doc.RootElement.GetProperty("works");

            if (works.GetArrayLength() == 0)
                throw new Exception("Не вдалося отримати дані з Open Library.");

            var work = works[_rnd.Next(works.GetArrayLength())];

            var title = work.GetProperty("title").GetString() ?? "Без назви";
            var author = work.GetProperty("authors")[0].GetProperty("name").GetString() ?? "Невідомий автор";
            var year = work.TryGetProperty("first_publish_year", out var yearProp) ? yearProp.GetInt32() : 2020;

            return new Book
            {
                Title = title,
                Author = author,
                Genre = selectedSubject,
                PublishYear = Math.Clamp(year, 1500, DateTime.UtcNow.Year),
                Publisher = "Open Library Edition",
                PageCount = _rnd.Next(120, 650)
            };
        }
    }
}