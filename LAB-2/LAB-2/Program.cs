using LAB_2.Service;
using System;
using System.Text;

namespace LAB_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.Title = "Війна за володорювання кланом";
            var menu = new GameMenu();
            menu.Run();
        }
    }
}