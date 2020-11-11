using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lab2
{
    public class HoneyPot
    {
        int potSize;
        int portions;
        public HoneyPot(int potSize)
        {
            this.potSize = potSize;
        }
        public void emptyPot()
        {
            portions = 0;
            Console.WriteLine("Бочонок пуст");
        }
        public void addPortion()
        {
            portions++;
            Console.WriteLine(portions.ToString() + " порция меда");
        }

        public bool isFull() { return potSize == portions; }
    }

    public class Bear
    {
        HoneyPot pot;
        public Bear(HoneyPot pot)
        {
            this.pot = pot;
        }
        public void run()
        {
            while (true)
            {
                lock (pot)
                {
                    while (!pot.isFull())
                    {
                        Monitor.Wait(pot);
                    }
                    Console.WriteLine("Пчела будит медведя");
                    Console.WriteLine("Медведь проснулся");
                    pot.emptyPot();
                    Monitor.PulseAll(pot);
                }
            }

        }
    }

    public class Bee
    {

        HoneyPot pot;

        public Bee(HoneyPot pot)
        {
            this.pot = pot;
        }


        public void run()
        {
            while (true)
            {
                Thread.Sleep(50);

                lock (pot)
                {
                    while (pot.isFull())
                    {
                        Monitor.Wait(pot);
                    }
                    pot.addPortion();
                    if (pot.isFull())
                        Monitor.PulseAll(pot);
                }
            }

        }
    }


}
