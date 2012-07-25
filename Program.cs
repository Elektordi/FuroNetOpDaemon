using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading;

namespace FuroNetOpDaemon
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MainLoop loop = new MainLoop();
            /*Timer t = new Timer(1000);
            t.Elapsed += loop.Tick;
            t.Enabled = true;*/

            while (true)
            {
                Thread.Sleep(100);
                loop.Recv();
            }
        }
    }
}

