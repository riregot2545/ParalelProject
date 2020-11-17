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
            Console.WriteLine("Задача 2 выполняется:");
            MessageExchange messageExchange = new MessageExchange();
            MyThread mt1 = new MyThread("Manufacturer", messageExchange, 10);
            MyThread mt2 = new MyThread("Consumer", messageExchange, 10);
            mt1.thread.Join();
            mt2.thread.Join();

            Console.WriteLine("Задача 2 выполнена");
            Console.ReadLine();

            /*
             * 
             * Задача Саши
             * 
             */

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
