using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using ClientServerCommonLibrary;

namespace PokerClient
{
    public class SynchronousSocketClient
    {
        static bool Connected;
        static Socket Server;
        public static void StartClient(string userID, string userPW)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                Server = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    Server.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        Server.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes("logmein:" + userID + "<EOF>");

                    // Send the data through the socket.  
                    int bytesSent = Server.Send(msg);
                    Thread th = new Thread(() =>
                    {
                        Connected = true;
                        while (Connected)
                        {
                            
                                
                                int bytesRec = Server.Receive(bytes);
                                var datarec = string.Format("{0}",
                                    Encoding.ASCII.GetString(bytes, 0, bytesRec));
                                if (datarec.Contains("<eof>"))
                                {
                                    var cmds = datarec.Split('$');

                                    for (int i = 0; i < cmds.Length; i++)
                                    {
                                        string[] cmd = cmds[i].Split(':');                                       
                                        if (!string.IsNullOrEmpty(cmd[0]))
                                        {
                                            switch (cmd[0])
                                            {
                                                case "setcard":
                                                    {
                                                        switch (cmd[1])
                                                        {
                                                            case "1":
                                                                Client.Instance.card1 = cmd[2].ToCard();
                                                                break;
                                                            case "2":
                                                                Client.Instance.card2 = cmd[2].ToCard();
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "update":
                                                    {
                                                        Table.Refresh(datarec);
                                                        break;
                                                    }
                                                case "updateSelf":
                                                    {   //updateSelf:0::0:0:False:True:True:0:0.2:0.2:Cosmin:0:
                                                        Client.Instance.UpdatePlayer(datarec);
                                                        break;
                                                    }
                                                case "ut":
                                                    {
                                                        Table.RefreshCards(cmds[i]);
                                                    }
                                                    break;
                                                default:
                                                    continue;
                                            }
                                        }
                                    }
                                }
                            
                            

                        }
                        Server.Shutdown(SocketShutdown.Both);
                        Server.Close();
                    });
                    th.Start();
                    // Release the socket.  


                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal static void Dispose()
        {
            Connected = false;
            //WaitOne();

        }

        public static void SendResponse(string v)
        {
            byte[] msg = Encoding.ASCII.GetBytes(v);

            // Send the data through the socket.  
            int bytesSent = Server.Send(msg);
        }
    }


}