using ClientServerCommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokerServer
{
    public class Table
    {
        public string tableID;
        //public Log tbleLog = new Log();
        public Client[] Clienti;
        public TableConfig tableConfig = new TableConfig();
        public CardPack cardPack = new CardPack();

        public bool SeatPlayer(Client client)
        {
            for (int i = 0; i < Clienti.Length; i++)
            {
                if (Clienti[i] == null)
                {
                    Clienti[i] = client;
                    Clienti[i].masa = this;
                    Clienti[i].tableSeat = i;
                    Console.WriteLine("Welcome Player " + client.dlinq.UID + " to table");
                    return true;
                }
            }
            return false;
        }

        //   bool canMod = true;
        // int lastbet = 0;
        public int Pot { get; private set; }
        public List<Card> TableCards = new List<Card>();
        int to;
        public int turnOrder
        {
            get
            {
                return to;
            }
            set
            {
                Clienti[to].isTurn = false;
                to = value;
                Clienti[to].isTurn = true;
                time = DateTime.Now;
            }

        }

        int sb = 0;
        int smallBlind
        {
            get
            {
                return sb;
            }
            set
            {

                sb = value;

            }
        }
        int bigBlind
        {
            get
            {
                return GetNext(sb);
            }
        }

        public int GetPrevious(int d, bool checkFold = false)
        {
            if (round > 0)
            {
                checkFold = true;
            }
            do
            {
                d--;
                if (d < 0)
                    d = Clienti.Length - 1;
            } while (Clienti[d] == null || ((bool)Clienti[d]?.folded && checkFold));
            return d;
        }
        public int GetNext(int d, bool checkFold = false)
        {
            if (round > 0)
            {
                checkFold = true;
            }
            do
            {
                d++;
                if (d > Clienti.Length - 1)
                    d = 0;
            } while (Clienti[d] == null || ((bool)Clienti[d]?.folded && checkFold));
            return d;
        }
        int lend;
        int lastToEnd
        {
            get
            {
                return lend;
            }
            set
            {
                lend = value;
                //ResetDecisions
            }
        }

        DateTime time = DateTime.Now;
        private int handTakers()
        {
            int cnt = 0;
            //var notfolded = (from q in Clienti where q.folded != true select q).Count();
            for (int i = 0; i < Clienti.Length; i++)
            {
                if (Clienti[i]?.folded != true)
                    cnt++;

            }
            return cnt;
        }
        private int turnTakers()
        {
            int cnt = 0;
            //var notfolded = (from q in Clienti where q.folded != true select q).Count();
            for (int i = 0; i < Clienti.Length; i++)
            {
                if (Clienti[i] != null)
                    cnt++;

            }
            return cnt;
        }
        public int round = 0;
        internal int bigBet;

        public void Create(TableConfig _tableConfig)
        {

            Clienti = new Client[_tableConfig.seats];
            OpenVisualizer(this);
            // 
            tableConfig = _tableConfig;
            bool endOfRound = true;
            ///if true move to next flop
            bool decisionTaken = false;
            // int cntToNextRound = Clienti.Count;

            Thread TableLoop = new Thread(() =>
           {
               while (true)
               {
                   Thread.Sleep(100);
                   if (turnTakers() > 1)
                   {
                        //if (Clienti[turnOrder].folded)
                        //    continue;
                        if (handTakers() <= 1)
                       {
                           endOfRound = true;
                       }
                       if (endOfRound)
                       {
                           round = 0;
                           Console.WriteLine("Game Over.");
                           TableCards.Clear2();
                           foreach (var p in Clienti)
                           {
                               p?.FoldCards();
                           }
                           Clienti[smallBlind].Bet(tableConfig.bigblind / 2);
                           Clienti[bigBlind].Bet(tableConfig.bigblind);
                           cardPack.Shuffle();
                           for (int i = 0; i <= 1; i++)//TODO SPLIT FROM small blind
                           {
                               foreach (var p in Clienti)
                               {
                                   if (p != null)
                                   {
                                       //p.isActive = true;
                                       p.folded = false;
                                       p.SetCard(i, cardPack.nextCard());
                                   }
                                   // Pachet.lastCard++;
                               }
                           }
                           lastToEnd = bigBlind;
                           turnOrder = GetNext(bigBlind);
                           endOfRound = false;
                           round = 1;
                       }
                       if (!endOfRound)
                       {
                           if ((DateTime.Now > time.AddSeconds(tableConfig.handTime)))
                           {
                               if (bigBet > Clienti[turnOrder].onhandBet)
                               {
                                   Clienti[turnOrder].FoldCards(true);
                               }
                               else
                               {
                                   Clienti[turnOrder].Check();
                               }
                               turnOrder = GetNext(turnOrder);
                               if (GetPrevious(turnOrder) == lastToEnd)
                               {
                                   if (Clienti[GetPrevious(turnOrder)].onhandBet == Clienti[turnOrder].onhandBet)
                                       decisionTaken = true;
                               }
                                //decisionTaken = true;
                            }
                           else
                           {
                               if (Clienti[turnOrder].tookDecision)
                               {
                                   if (Clienti[turnOrder].onhandBet > Clienti[GetPrevious(turnOrder)].onhandBet)
                                   {
                                       if (round == 1)
                                       {
                                           if (Clienti[turnOrder].onhandBet > this.tableConfig.bigblind)
                                           {
                                               lastToEnd = GetPrevious(turnOrder);
                                           }
                                       }
                                       else
                                       {
                                           lastToEnd = GetPrevious(turnOrder);
                                       }


                                   }
                                   Clienti[turnOrder].tookDecision = false;
                                   turnOrder = GetNext(turnOrder);
                                   if (GetPrevious(turnOrder) == lastToEnd)
                                   {
                                       if (Clienti[GetPrevious(turnOrder)].onhandBet == Clienti[turnOrder].onhandBet)
                                           decisionTaken = true;
                                   }
                                   
                               }
                                //LISTEN FOR INTERACTION
                            }
                           if (decisionTaken)
                           {
                               bigBet = 0;
                               decisionTaken = false;
                               foreach (var c in Clienti)
                               {
                                   if (c != null)
                                       c.onhandBet = 0;
                               }
                               round++;
                               if (round == 2)//Flop
                                {
                                   turnOrder = smallBlind;
                                   lastToEnd = GetPrevious(smallBlind);
                                   TableCards.Add(cardPack.nextCard());
                                   TableCards.Add(cardPack.nextCard());
                                   TableCards.Add(cardPack.nextCard());
                                    //continue;
                                }
                               if (round == 3)//River
                                {

                                   turnOrder = smallBlind;
                                   lastToEnd = GetPrevious(smallBlind);
                                   TableCards.Add(cardPack.nextCard());
                                    //continue;
                                }
                               if (round == 4)//Turn
                                {
                                   turnOrder = smallBlind;
                                   lastToEnd = GetPrevious(smallBlind);
                                   TableCards.Add(cardPack.nextCard());
                               }
                                //if (round == 5)//Turn
                                //{
                                //    turnOrder = smallBlind;
                                //    lastToEnd = GetPrevious(smallBlind);
                                //    //
                                //    TableCards.Add(cardPack.nextCard());
                                //}
                                if (round == 6)//END
                                {
                                   smallBlind = GetNext(smallBlind);
                                    //lastToEnd = GetPrevious(smallBlind);
                                    SetWinner();
                                   endOfRound = true;
                                   Thread.Sleep(2000);
                                   TableCards.Clear();
                               }
                           }
                           else
                           {



                           }

                       }
                       
                        // canMod = true;                       
                    }
               }
           });
            TableLoop.Start();
        }

        private void OpenVisualizer(Table table)
        {
            Thread thx = new Thread(() =>
            {
                TableTurnVisualizer visualizer = new TableTurnVisualizer(table);
                visualizer.ShowDialog();
            });
            thx.Start();
        }

        //internal void DoAction(Client client, string v1, string v2)
        //{
        //    if (client.isTurn)
        //    {
        //        switch (v1)
        //        {
        //            case "raise":
        //            case "bet":
        //                {
        //                    Clienti[turnOrder].Bet(int.Parse(v2));
        //                    Pot += int.Parse(v2);
        //                    lastToEnd = turnOrder;
        //                    turnOrder++;
        //                    break;

        //                }
        //            case "call":
        //                {
        //                    Clienti[turnOrder].Call(int.Parse(v2));
        //                    Pot += int.Parse(v2);
        //                    turnOrder++;
        //                    break;
        //                }
        //            case "fold":
        //                {
        //                    Clienti[turnOrder].FoldCards();
        //                    turnOrder++;
        //                    break;
        //                }
        //            case "check":
        //                {
        //                    turnOrder++;
        //                    break;

        //                }
        //        }
        //    }
        //}

        private void GameRestart()
        {
            // throw new NotImplementedException();
        }

        private void SetWinner()
        {
            //  throw new NotImplementedException();
        }





        public void Destroy()
        {

        }

    }
}
