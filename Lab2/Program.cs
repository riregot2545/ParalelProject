using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2
{
    public class Program
    {
        static void Main(string[] args)
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
