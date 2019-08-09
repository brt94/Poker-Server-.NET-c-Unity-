using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using ClientServerCommonLibrary;

namespace PokerClient
{
    public class Client
    {
        private static ClientBase _this;
        public static ClientBase Instance
        {
            get
            {
                if(_this != null)
                return _this;
                else
                {
                    _this = new ClientBase();
                    return _this;
                }
            }
            set
            {
                _this = value;
            }
        }

        public static void Fold()
        {
            SynchronousSocketClient.SendResponse("fold:false:");
        }

        public static void Connect(string userID)
        {
          //  SynchronousSocketClient.StartClient(userID);
        }

        public static void Call(int p)
        {
            SynchronousSocketClient.SendResponse("call:"+p+":");
        }

        public static void Bet(int v)
        {
            SynchronousSocketClient.SendResponse("bet:" + v + ":");
        }

        public static void Connect(string userID,string userPW)
        {
            SynchronousSocketClient.StartClient(userID,userPW);
        }
        public static void Dispose()
        {
            SynchronousSocketClient.Dispose();
        }
        public static void SendCMD(string cmd)
        {
            SynchronousSocketClient.SendResponse(cmd);
        }

        public static void SetActive()
        {
            SynchronousSocketClient.SendResponse("active:0");
        }
    }
}
