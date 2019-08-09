using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokerServer
{
    public class GameServer
    {

        public GameServer()
        {

        }
        public static void one2one(Client client)
        {
            Thread reciever = new Thread(() =>
            {
                while (true)
                {
                    if (client.Soket.Connected)
                    {
                        byte[] buffer = new byte[14];
                        int r = 0;
                        try
                        {
                            r = client.Soket.Receive(buffer);
                        }
                        catch
                        {
                            break;
                        }
                        if (r > 1)
                        {
                            string str = Encoding.ASCII.GetString(buffer);
                            //if (str.Contains("<eof>"))
                            //{
                                string[] cmd = str.Split(':');
                                switch (cmd[0])
                                {
                                    case "cn":
                                        {
                                            client.Name = cmd[1];
                                            break;
                                        }
                                    case "play":
                                        {
                                            Console.WriteLine("PLAY");
                                            var args = cmd[1].Split('.');
                                            TableConfig tc = new TableConfig();
                                            tc.buyin = int.Parse(args[0]);
                                            tc.seats = int.Parse(args[1]);
                                            MatchMaking.Queue.Add(client, tc);
                                            Console.WriteLine("Finding Table for" + client.dlinq.UID);

                                            break;
                                        }
                                    case "bet":
                                        if (client.isTurn)
                                        {
                                            client.Bet(int.Parse(cmd[1]));
                                        }
                                        break;
                                    case "raise":
                                        if (client.isTurn)
                                        {
                                            client.Bet(int.Parse(cmd[1]));
                                        }
                                        break;
                                    case "call":
                                        if (client.isTurn)
                                        {
                                            client.Call();
                                        }
                                        break;
                                    case "fold":
                                        if (client.isTurn)
                                        {
                                            client.FoldCards(bool.Parse(cmd[1]));
                                        }
                                        break;
                                    case "check":
                                        if (client.isTurn)
                                        {
                                            client.Check();
                                        }
                                        break;
                                    case "active":
                                        client.isActive = true;
                                        break;
                                        //menuShits;
                                }
                            }

                        //}
                    }
                    else
                    {
                        try
                        {
                            client.Soket.Dispose();
                        }
                        catch
                        {

                        }
                    }
                    Thread.Sleep(200);
                }

            });
            reciever.Start();
        }
    }
}
