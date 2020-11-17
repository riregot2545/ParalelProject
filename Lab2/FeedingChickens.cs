using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2
{

    class Chicken
    {
        private ChickenMom chickenMom;
        private TimeSpan sleepTime;
        private Feeder feeder;

        public Chicken(ChickenMom chickenMom, TimeSpan sleepTime, Feeder feeder)
        {
            this.chickenMom = chickenMom;
            this.sleepTime = sleepTime;
            this.feeder = feeder;
        }

        public void Eat()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                Feeder.Semaphore.WaitOne();
                Console.WriteLine("[{1}] Chick {0} started eating", Thread.CurrentThread.Name, DateTime.Now.Ticks);
                int remainFood = feeder.Feed();
                Console.WriteLine("[{2}] Chick {0} ended eating. Remaining food={1}", Thread.CurrentThread.Name, remainFood, DateTime.Now.Ticks);
                if (remainFood == 0)
                {
                    Console.WriteLine("[{1}] Chick {0} called for food", Thread.CurrentThread.Name, DateTime.Now.Ticks);
                    chickenMom.CallForFood();
                }
                else
                {
                    Feeder.Semaphore.Release();
                }

                Thread.Sleep(sleepTime);
            }
        }
    }

    class ChickenMom
    { 
        private Feeder feeder;
        private int foodCount;
        private object lockObj;

        public ChickenMom(Feeder feeder, int foodCount)
        {
            this.feeder = feeder;
            this.foodCount = foodCount;
            this.lockObj = new object();
        }

        public void CallForFood()
        {
            Console.WriteLine("[{1}] Chick {0} called me (mom) for food", Thread.CurrentThread.Name, DateTime.Now.Ticks);
            lock (lockObj)
            {
                Monitor.Pulse(lockObj);
            }
        }

        public void ReplaceFeeder()
        {
            lock (lockObj)
            {
                while (Thread.CurrentThread.IsAlive)
                { 
                    feeder.ReplaceFood(foodCount);
                    Console.WriteLine("[{0}] Mom replaced food", DateTime.Now.Ticks);
                    Monitor.Wait(lockObj);
                }
            }
        }
    }

    class Feeder
    {
        public static Semaphore Semaphore;

        private volatile int food;

        public int Feed()
        {
            food--;
            return food;
        }

        public void ReplaceFood(int count)
        {
            food = count;
            Semaphore.Release();            
        }
    }

    class FeedingChickens
    {
        private int chickensCount;
        private int foodCount;
        private TimeSpan sleepTime;
        private TimeSpan executionTime;

        public FeedingChickens(int chickensCount, int foodCount, TimeSpan sleepTime, TimeSpan executionTime)
        {
            this.chickensCount = chickensCount;
            this.foodCount = foodCount;
            this.sleepTime = sleepTime;
            this.executionTime = executionTime;
        }

        public void Start()
        {
            Feeder.Semaphore = new Semaphore(chickensCount-1, chickensCount);
            Feeder feeder = new Feeder();

            List<Thread> chickensThreads = new List<Thread>(chickensCount + 1);
            ChickenMom chickenMom = new ChickenMom(feeder, foodCount);
            Thread chickenMomThread = new Thread(chickenMom.ReplaceFeeder);
            chickenMomThread.Name = "Mom";
            chickenMomThread.Start();
            chickensThreads.Add(chickenMomThread);

            for (int i = 0; i < chickensCount; i++)
            {
                Chicken chicken = new Chicken(chickenMom, sleepTime, feeder);
                Thread chickenThread = new Thread(chicken.Eat);
                chickenThread.Name = (i + 1).ToString();
                chickenThread.Start();
                chickensThreads.Add(chickenThread);
            }

            Thread.Sleep(executionTime);
            chickensThreads.ForEach(t => t.Abort());
        }

    }
}
