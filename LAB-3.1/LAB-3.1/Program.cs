using LAB_3._1;
using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        ICalculator calculator = new LightCalculator(new FullCalculator());

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Калькулятор запущено ===");
        Console.WriteLine("Підтримувані операції: +, -, *, /");
        Console.WriteLine("Для виходу введіть 'exit' на будь-якому етапі.\n");

        while (true)
        {
            if (!Validator.TryReadDouble("Введіть число A: ", out double a))
                break;
            if (!Validator.TryReadOperator("Виберіть операцію (+, -, *, /): ", out string op))
                break;
            if (!Validator.TryReadDouble("Введіть число B: ", out double b))
                break;
            try
            {
                double result = op switch {
                    "+" => calculator.Add(a, b),
                    "-" => calculator.Subtract(a, b),
                    "*" => calculator.Multiply(a, b),
                    "/" => calculator.Divide(a, b),
                    _ => throw new InvalidOperationException("Невідома операція.")
                };
                result = Math.Round(result, 5);
                Console.WriteLine($"Результат: {a} {op} {b} = {result}\n");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}\n");
            }
        }

        Console.WriteLine("Програму завершено.");
    }
    
    }
