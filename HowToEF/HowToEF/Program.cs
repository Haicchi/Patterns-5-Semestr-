using HowToEF;
using System;
using System.Linq;

using var db = new AppDbContext();

var defaultUser = new User
{
    Name = "Менеджер Агентства",
    Age = 28
};

db.Users.Add(defaultUser);
db.SaveChanges(); // База сама сгенерирует Id

db.Bookings.Add(new Booking
{
    Destination = "Марса-Алам",
    UserId = defaultUser.Id
});

db.SaveChanges();
Console.WriteLine("Данные успешно добавлены!");