using ClientServerCommonLibrary;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PokerServer
{
    public class Client:ClientBase
    {

        public DataBaseLink dlinq = new DataBaseLink();

        //ON GAME
        public Table masa = null;
        //
       
        
        public int Atempts { get; internal set; }

        public Socket Soket;

        internal bool IsTurn
        {
            get
            {
                return isTurn;
            }
            set
            {
                if (value == true)
                {
                    //sendMsg("Is your turn");
                }
                else
                {
                   // sendMsg("Turn over");
                }
                isTurn = value;
            }
        }
        private handStrenght hs= new handStrenght();
        public handStrenght HandStrenght
        {
            get
            {
                return hs;
            }
            set
            {
                hs = value;
                base.handStrenght = hs.Strenght;
            }
        }

        public void ImBack()
        {
            isActive = true;
        }
      

      

        //private int lastBet;

        internal void SetCard(int i, Card card)
        {
            if (i == 0)
            {
                card1 = card;
                if (Soket.Connected)
                {                   //cmd0:cmd1:cm.d2
                    string data = "$setcard:1:" + card1.Culoare + "." + card1.Numar+"$";
                    byte[] byteData = Encoding.ASCII.GetBytes(data);
                    Soket.Send(byteData);
                }
            }
            if (i == 1)
            {
                card2 = card;
                if (Soket.Connected)
                {
                    string data = "$setcard:2:" + card2.Culoare + "." + card2.Numar + "$";
                    byte[] byteData = Encoding.ASCII.GetBytes(data);
                    Soket.Send(byteData);
                }
            }
            if (i > 1)
            {
                Log.Write("Splited more than 2 CARDS");
            }
        }

        internal void Ping()
        {
            //throw new NotImplementedException();
        }

        internal void FoldCards(bool inactive = false,bool endofRound = false)
        {
            folded = true;
            //nextAction = null;
            if (endofRound == false)
            {               
                tookDecision = true;
            }
            else
            {
                card1 = Card.Null;
                card2 = Card.Null;
            }
            //sendMsg("Inactive Fold");
            if (inactive)
                isActive = false;
        }

        internal string Update(bool showcards = true)
        {
            if (masa == null)
                return "";
            string ss = "$update:";
            //int cnt = 0;
            if (masa.Clienti.Length > 1)
            {
                for (int i = 0; i < masa.Clienti.Length; i++)//(var q in masa.Clienti)
                {
                    var q = masa.Clienti[i];
                    if (q != null)
                    {
                        if (q != this)
                        {

                            if (!showcards)
                                ss += "|" + i + ":" + q.ToString(true);
                            else
                                ss += "|" + i + ":" + q.ToString(false);


                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return ss + "$";
        }
        internal string UpdateSelf()
        {

            string ss = "$";
            ss += "updateSelf:" + tableSeat + ":" + this.ToString(true);
            string data = ss + "$";

            return data;
        }

        internal void Bet(int v)
        {         
            Cash -= v;
            onhandBet += v;
            masa.bigBet = onhandBet;
            tookDecision = true;
        }

        internal void Call()
        {
            //masa.bigBet -= onhandBet;  
            int a = masa.bigBet - onhandBet;
            onhandBet = masa.bigBet;
            Cash -= a;
            //masa.bigBet = onhandBet;
            tookDecision = true;
        }

        public void sendMsg(string msg)
        {
            //string data = string.Format("update:to={0}:{1}", table.turnOrder, ss);
            byte[] byteData = Encoding.ASCII.GetBytes(msg);
            Soket.Send(byteData);
        }

        internal void Check()
        {
            tookDecision = true;
            //throw new NotImplementedException();
        }

        public Client()
        {
            
                  
                   
                   
             
        }

        internal void UpdateTable()
        {            
            string ss = "$" + UpdateSelf() + Update();
            ss += "$ut:";
            
                foreach (var c in masa.TableCards)
                {
                    ss += c.ToString() + ":";
                }
            
            string data = ss + "$<eof>";
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            try
            {
                Soket.Send(byteData);
            }
            catch
            {
                //skip till it work
            }
        }
    }
}