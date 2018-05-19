using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace FinderConsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Finder finder = new Finder();

            Console.Write("Введите путь для поиска (Пример: C:\\): ");
            var dir = Console.ReadLine();

            if (dir != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Чтобы возобновить либо остановить поиск нажмите пробел");
                Console.ResetColor();
                Thread.Sleep(2000);
            }

            var sw = new Stopwatch();
            sw.Start();

            List<string> location = finder.StartScanDirectory(@dir);

            sw.Stop();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Конец поиска");
            Console.WriteLine("Кол-во файлов: " + location.Count);
            Console.WriteLine("Время поиска: " + sw.Elapsed);
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
