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
            Use();
            int taskN = int.Parse(Console.ReadLine());
            switch (taskN)
            {
                case 2:
                    Task2();
                    break;
                case 7:
                    Task7();
                    break;
                case 8:
                    Task8();
                    break;
            }

            Console.ReadLine();
        }

        private static void Use()
        {
            Console.WriteLine("Пожалуйста, введите номер задачи. Доступные задачи: 2, 7, 8.");
        }

        private static void Task2()
        {
            Console.WriteLine("Задача 2 выполняется:");
            MessageExchange messageExchange = new MessageExchange();
            MyThread mt1 = new MyThread("Manufacturer", messageExchange, 10);
            MyThread mt2 = new MyThread("Consumer", messageExchange, 10);
            mt1.thread.Join();
            mt2.thread.Join();

            Console.WriteLine("Задача 2 выполнена");
            
        }

        private static void Task7()
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
        }

        private static void Task8()
        {
            int H = 20;
            int N = 5;
            HoneyPot pot = new HoneyPot(H);
            Bear bear = new Bear(pot);
            Thread bearT = new Thread(new ThreadStart(bear.run));
            bearT.Start();
            for (int i = 0; i < N; i++)
            {
                Bee bee = new Bee(pot);
                Thread beeT = new Thread(new ThreadStart(bee.run));
                beeT.Start();
            }

            bearT.Join();
        }


    }
}
