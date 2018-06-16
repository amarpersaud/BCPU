using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCPU;

namespace BCPUInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            //200Hz
            BCPU.BCPU cpu = new BCPU.BCPU(200);
            ThreadStart ts = new ThreadStart(cpu.Run);
            Thread t = new Thread(ts);
            t.Start();
            Console.ReadLine();
        }
    }
}
