using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lab2
{
    class MessageExchange
    {
        private object locker = new object();
        private string message = "";

        public void WriteMessage(bool running)
        {
            lock (locker)
            {
                if (!running)
                {
                    Monitor.Pulse(locker);
                    return;
                }

                Console.Write("Производитель пишет: ");
                message = Console.ReadLine();
                Monitor.Pulse(locker);

                Monitor.Wait(locker);
            }
        }

        public void ReadMessage(bool running)
        {
            lock (locker)
            {
                if (!running)
                {
                    Monitor.Pulse(locker);
                    return;
                }

                Console.WriteLine("Потребитель читает: {0}", message);
                Monitor.Pulse(locker);

                Monitor.Wait(locker);
            }
        }
    }

    class MyThread
    {
        public Thread thread;
        MessageExchange messageExchange;

        public MyThread(string name, MessageExchange messageExchange)
        {
            thread = new Thread(Run);
            this.messageExchange = messageExchange;
            thread.Name = name;
            thread.Start();
        }

        void Run()
        {
            if (thread.Name == "Manufacturer")
            {
                for (int i = 0; i < 5; i++)
                    messageExchange.WriteMessage(true);
                messageExchange.WriteMessage(false);
            }
            else
            {
                Thread.Sleep(100);
                for (int i = 0; i < 5; i++)
                    messageExchange.ReadMessage(true);
                messageExchange.ReadMessage(false);
            }
        }
    }
}
