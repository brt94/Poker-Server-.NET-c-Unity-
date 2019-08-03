using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokerServer
{

    public class MatchMaking
    {

        public static Dictionary<Client, TableConfig> Queue = new Dictionary<Client, TableConfig>();
        public static List<Client> aux = new List<Client>();
        public MatchMaking()
        {
            Thread th = new Thread(() =>
            {
                while (true)
                    if (Queue.Count > 0)
                    {
                        Thread.Sleep(200);
                        foreach (var player in Queue.Keys)
                        {
                            player.Atempts++;
                            var tc = Queue[player];
                            try
                            {
                                Table t = TablesMonitor.Find(tc);
                                if (t != null)
                                {
                                    if (t.SeatPlayer(player))
                                    {
                                        aux.Add(player);
                                    }
                                }
                                if (player.Atempts > 20)
                                {
                                    TablesMonitor.CreateTable(tc).SeatPlayer(player);
                                    aux.Add(player);
                                }


                            }

                            catch
                            {
                                if (player.Atempts > 10)
                                {
                                    TablesMonitor.CreateTable(tc).SeatPlayer(player);
                                    aux.Add(player);
                                }
                            }

                        }
                        foreach (var a in aux)
                        {
                            Queue.Remove(a);
                        }

                    }
            });
            th.Start();
        }
    }
}
