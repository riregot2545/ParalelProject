using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageExchange messageExchange = new MessageExchange();
            MyThread mt1 = new MyThread("Manufacturer", messageExchange);
            MyThread mt2 = new MyThread("Consumer", messageExchange);
            mt1.thread.Join();
            mt2.thread.Join();

            Console.WriteLine("Задача 2 выполнена");
            Console.ReadLine();
        }
    }
}
