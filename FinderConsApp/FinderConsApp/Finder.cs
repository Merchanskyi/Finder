using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinderConsApp
{
    public class Finder
    {
        private List<string> _result = new List<string>();
        private object _lock = new object();

        public List<string> StartScanDirectory(string baseDirectory)
        {
            var task = ScanDirectoryAsync(baseDirectory);

            var suspend = false;

            while (!task.IsCompleted)
            {
                if (Console.KeyAvailable)
                {
                    var pressButton = Console.ReadKey();
                    if (pressButton.Key == ConsoleKey.Spacebar)
                    {
                        if (suspend)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Поиск продолжен, нажмите пробел чтобы поставить на паузу");
                            Console.ResetColor();
                            Thread.Sleep(400);
                            Monitor.Exit(_lock);
                        }
                        else
                        {
                            Monitor.Enter(_lock);
                            Thread.Sleep(300);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Поиск в ожидании, нажмите пробел чтобы продолжить");
                            Console.ResetColor();
                        }
                    }
                    suspend = !suspend;
                }
                Thread.Sleep(100);
            }
            return _result;
        }

        private Task ScanDirectoryAsync(string baseDirectory)
        {
            return Task.Run(async () =>
            {
                Monitor.Enter(_lock);
                Monitor.Exit(_lock);

                if (!Directory.Exists(baseDirectory))
                {
                    return;
                }

                try
                {
                    Console.WriteLine(baseDirectory);
                    _result.AddRange(Directory.GetFiles(baseDirectory));

                    await Task.WhenAll(Directory.GetDirectories(baseDirectory).Select(x => ScanDirectoryAsync(x)));
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            });
        }
    }
}