using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Threading;

namespace FuroNetOpDaemon
{
    class MainLoop
    {

        IPEndPoint ipep;
        Socket newsock;
        EndPoint Remote;

        String header = "*FuroNetworkOperator*";
        //bool fools = false;
        Random rand;

        System.Timers.Timer fools;

        WMPLib.WindowsMediaPlayer wplayer;

        public MainLoop()
        {
            rand = new Random();

            wplayer = new WMPLib.WindowsMediaPlayer();

            fools = new System.Timers.Timer(100);
            fools.Elapsed += this.Tick;
            //fools.Enabled = true;

            ipep = new IPEndPoint(IPAddress.Any, 2012);

            newsock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Dgram, ProtocolType.Udp);
            //newsock.Blocking = false;
            //newsock.ReceiveTimeout = 100;
            newsock.Bind(ipep);

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            Remote = (EndPoint)(sender);
        }

        public void Stop()
        {
            newsock.Close();
        }

        private static void KillProcess(String processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }

        public void Tick(object sender, ElapsedEventArgs e)
        {
                Point p = Cursor.Position;
                p.X += rand.Next(100) - 50;
                p.Y += rand.Next(100) - 50;
                Cursor.Position = p;
        }

        public void Recv()
        {

            try
            {

                byte[] data = new byte[1024];
                int recv;
                while ((recv = newsock.ReceiveFrom(data, ref Remote)) > 0)
                {
                    String msg = Encoding.ASCII.GetString(data, 0, recv);
                    if (msg.Length < header.Length) return;
                    if (msg.Substring(0, header.Length) != header) return;
                    String cmd = msg.Substring(header.Length);
                    Console.WriteLine(cmd);
                    switch (cmd)
                    {
                        case "music_on":
                            
                            //if (System.Environment.MachineName.ToLowerInvariant() == "bombyx8")
                            //{
                                KillProcess("plugin-container");
                            //}
                            

                            ThreadPool.QueueUserWorkItem(
                                 delegate(object param)
                                 {
                                     wplayer = new WMPLib.WindowsMediaPlayer();
                                     wplayer.URL = "c:\\windows\\demo.mp3";
                                 }
                            );


                            //wplayer.settings.setMode("loop", true);
                            //wplayer.URL = "demo.mp3";
                            //wplayer.controls.play();
                            break;

                        case "music_off":
                            if(wplayer!=null)
                                wplayer.controls.stop();
                            break;

                        case "fool_on":
                            fools.Enabled = true;
                            break;

                        case "fool_off":
                            fools.Enabled = false;
                            break;

                        case "shutdown":
                            Process proc = new Process();
                            proc.StartInfo = new ProcessStartInfo("shutdown","-s -t 60 -c \"Allez, on y va !\"");
                            proc.Start();
                            break;

                        default:
                            // ERR ?!
                            break;
                    }
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
            }
            catch (System.ObjectDisposedException)
            {
                newsock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Dgram, ProtocolType.Udp);
                newsock.Blocking = false;
                newsock.Bind(ipep);
            }
        }

    }
}
