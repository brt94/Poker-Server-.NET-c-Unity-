using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = new GameServer();
            var m = new MatchMaking();
            bool stop = false;

            Thread ping = new Thread(() =>
            {
                while (!stop)
                {
                    Thread.Sleep(100);
                    foreach (var c in AsynchronousSocketListener.Clienti)
                    {
                        try
                        {
                            string data = "ping:" + DateTime.Now;
                            byte[] byteData = Encoding.ASCII.GetBytes(data);
                            c.Send(byteData);
                        }
                        catch
                        {
                          //Can be late or disconected
                        }
                    }

                }
            });
            ping.Start();
            LoginServer.Start();
          
           

        }
    }
}
