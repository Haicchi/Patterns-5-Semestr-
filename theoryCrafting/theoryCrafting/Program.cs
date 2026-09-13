using System.Text;
using theoryCrafting;

Console.OutputEncoding = Encoding.UTF8;

Random rand = new Random();
List<SystemOption> options = new List<SystemOption>();

double p1Min = 80, p1Max = 160;
double p2Min = 80, p2Max = 400;
double p3Min = 0.8, p3Max = 80;

double a1 = 0.2, a2 = 0.1, a3 = 0.7;

// --- Крок 1. Згенерувати множину можливих об’єктів за трьома критеріями. ---
Console.WriteLine("Крок 1. Згенерована множина можливих об’єктів:");
for (int i = 1; i <= 15; i++)
{
    var option = new SystemOption
    {
        Id = i,
        P1 = Math.Round(p1Min + rand.NextDouble() * (p1Max - p1Min), 2),
        P2 = Math.Round(p2Min + rand.NextDouble() * (p2Max - p2Min), 2),
        P3 = Math.Round(p3Min + rand.NextDouble() * (p3Max - p3Min), 2)
    };
    options.Add(option);
    Console.WriteLine($"F{option.Id,-2} | P1: {option.P1,6} | P2: {option.P2,6} | P3: {option.P3,5}");
}

// --- Крок 2. Сформувати множину Парето. ---
for (int i = 0; i < options.Count; i++)
{
    for (int j = 0; j < options.Count; j++)
    {
        if (i == j) continue;
        bool jIsBetterOrEqual = options[j].P1 >= options[i].P1 &&
                                options[j].P2 <= options[i].P2 &&
                                options[j].P3 >= options[i].P3;

        bool jIsStrictlyBetter = options[j].P1 > options[i].P1 ||
                                 options[j].P2 < options[i].P2 ||
                                 options[j].P3 > options[i].P3;

        if (jIsBetterOrEqual && jIsStrictlyBetter)
        {
            options[i].IsParetoOptimal = false;
            break;
        }
    }
}

Console.WriteLine("\nКрок 2. Відкинуті (доміновані) варіанти:");
var discardedSet = options.Where(o => !o.IsParetoOptimal).ToList();
foreach (var opt in discardedSet)
{
    Console.WriteLine($"F{opt.Id,-2} | P1: {opt.P1,6} | P2: {opt.P2,6} | P3: {opt.P3,5}");
}

var paretoSet = options.Where(o => o.IsParetoOptimal).ToList();
Console.WriteLine("\nМножина Парето (оптимальні за Парето об'єкти):");
foreach (var opt in paretoSet)
{
    Console.WriteLine($"F{opt.Id,-2} | P1: {opt.P1,6} | P2: {opt.P2,6} | P3: {opt.P3,5}");
}

// --- Кроки 3, 4 та 5. Нормування, застосування ваг та розрахунок функції корисності ---
Console.WriteLine("\nКроки 3, 4 та 5. Детальний розрахунок функції корисності для множини Парето:");
Console.WriteLine($"{"ID",-3} | {"Норм P1",-7} | {"Норм P2",-7} | {"Норм P3",-7} | {"Зваж P1",-7} | {"Зваж P2",-7} | {"Зваж P3",-7} | Корисність (Сума)");
Console.WriteLine(new string('-', 90));

SystemOption bestOption = null;

foreach (var opt in paretoSet)
{
    
    opt.NormP1 = opt.P1 / p1Max;
    opt.NormP2 = opt.P2 / p2Max; 
    opt.NormP3 = opt.P3 / p3Max;

   
    opt.WeightedP1 = opt.NormP1 * a1;
    opt.WeightedP2 = opt.NormP2 * a2;
    opt.WeightedP3 = opt.NormP3 * a3;

    opt.Utility = opt.WeightedP1 - opt.WeightedP2 + opt.WeightedP3;

    Console.WriteLine($"F{opt.Id,-2} | {opt.NormP1,7:F4} | {opt.NormP2,7:F4} | {opt.NormP3,7:F4} | {opt.WeightedP1,7:F4} | {opt.WeightedP2,7:F4} | {opt.WeightedP3,7:F4} | {opt.Utility:F4}");

    if (bestOption == null || opt.Utility > bestOption.Utility)
    {
        bestOption = opt;
    }
}

// --- Крок 6. Визначити оптимальне рішення. ---
Console.WriteLine($"\nКрок 6. Визначено оптимальне рішення:");
Console.WriteLine($"Найкращий єдиний варіант: F{bestOption.Id} (Максимальна функція корисності: {bestOption.Utility:F4})");