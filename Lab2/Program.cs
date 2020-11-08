using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // Параметры инициализации задачи 7
            int chickensCount = 5;
            int foodCount = 30;
            TimeSpan sleep = TimeSpan.FromMilliseconds(50);
            TimeSpan chickenExecutionTime = TimeSpan.FromSeconds(5);

            Console.WriteLine("Задача 7 выполняется:");
            FeedingChickens feedingChickens = new FeedingChickens(chickensCount, foodCount, sleep, chickenExecutionTime);
            feedingChickens.Start();
            Console.WriteLine("Задача 7 выполнена.");

            Console.ReadLine();
        }
    }
}
