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

                message = TextGeneration.Generate(7);
                Console.WriteLine("Manufacturer has written.");
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

                Console.WriteLine("Consumer is reading: {0}", message);
                Monitor.Pulse(locker);

                Monitor.Wait(locker);
            }
        }
    }

    class MyThread
    {
        public Thread thread;
        MessageExchange messageExchange;

        public MyThread(string name, MessageExchange messageExchange, int repeats)
        {
            thread = new Thread(() => Run(repeats));
            this.messageExchange = messageExchange;
            thread.Name = name;
            thread.Start();
        }

        void Run(int repeats)
        {
            if (thread.Name == "Manufacturer")
            {
                for (int i = 0; i < repeats; i++)
                {
                    messageExchange.WriteMessage(true);
                    Thread.Sleep(500);
                }
                messageExchange.WriteMessage(false);
            }
            else
            {
                Thread.Sleep(100);
                for (int i = 0; i < repeats; i++)
                {
                    messageExchange.ReadMessage(true);
                    Thread.Sleep(500);
                }
                messageExchange.ReadMessage(false);
            }
        }
    }

    static class TextGeneration
    {
        public static string Generate(int num_letters)
        {
            char[] letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

            Random rand = new Random();

            string word = "";

            for (int j = 1; j <= num_letters; j++)
            {
                int letter_num = rand.Next(0, letters.Length - 1);
                word += letters[letter_num];
            }

            return word;
        }
    }
}