using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_3._1
{
    public static class Validator
    {
        public static bool TryReadDouble(string prompt, out double value)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    value = 0;
                    return false;
                }
                string normalizedInput = input?.Replace(',', '.') ?? string.Empty;

                if (double.TryParse(normalizedInput, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                {
                    return true;
                }
                Console.WriteLine("Некоректний ввід! Будь ласка, введіть дійсне число.");
            }
        }
        public static bool TryReadOperator(string prompt, out string op)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    op = string.Empty;
                    return false;
                }

                if (input == "+" || input == "-" || input == "*" || input == "/")
                {
                    op = input;
                    return true;
                }
                Console.WriteLine("Невідомий знак! Дозволені операції: +, -, *, /");
            }
        }
        }
}
