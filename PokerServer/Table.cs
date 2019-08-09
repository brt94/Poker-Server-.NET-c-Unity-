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
                    Clienti[i].tookDecisionChanged += Table_tookDecisionChanged;
                    Console.WriteLine("Welcome Player " + client.dlinq.UID + " to table");
                    Clienti[i].isActive = true;
                    return true;
                }
            }
            return false;
        }

        private void Table_tookDecisionChanged()
        {
            if (round == 0)
                return;
            Clienti[turnOrder].isTurn = false;
            Clienti[turnOrder]._tookDecision = false;
            bool _return = false;
            if (Clienti[turnOrder].onhandBet < Clienti[GetPrevious(turnOrder)].onhandBet && !_return)
            {
                Clienti[turnOrder].folded = true;
                //inactive or took decision to fold 
                if (to != lastToEnd)
                {
                    _return = true;
                }
                else
                {
                    round++;
                    UpdateAll();
                    return;
                }
            }
            //Raise or BET
            if (Clienti[turnOrder].onhandBet > Clienti[GetPrevious(turnOrder)].onhandBet && !_return)
            {
                lastToEnd = GetPrevious(to);

                _return = true;
            }
            if (Clienti[to].onhandBet == Clienti[GetPrevious(to)].onhandBet && !_return)
            {
                if (to != lastToEnd)
                {
                    _return = true;
                }
                else
                {
                    round++;
                    UpdateAll();
                    return;
                }
            }
            if (_return)
            {
                turnOrder = GetNext(turnOrder);
                Clienti[turnOrder].isTurn = true;
                time = DateTime.Now.AddSeconds(-2);
                UpdateAll();
            }
            else
            {
                Console.WriteLine("Untreated Case!!!!! Rw");
            }

        }

        //   bool canMod = true;
        // int lastbet = 0;
        public int Pot { get; private set; }
        public Card[] tableCards = new Card[5];
        public Card[] TableCards
        {
            get
            {               
                return tableCards;                
            }
            set
            {
                tableCards = value;
                UpdateAll();
            }
        }
        int to;
        public int turnOrder
        {
            get
            {
                return to;
            }
            set
            {
                to = value;
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
                // checkFold = true;
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
                if (Clienti[i]?.folded == false)
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
        public int rnd = 0;
        public int round
        {
            get
            {
                return rnd;

            }
            set
            {
                if (rnd != value)
                {
                    rnd = value;
                    OnRoundChanged();
                }
            }
        }

        private void OnRoundChanged()
        {
           
            foreach (var c in Clienti)
            {
                if (c != null)
                {
                    Pot += c.onhandBet;
                    c.onhandBet = 0;
                }
            }
            if (round == 2)//Flop
            {
                TableCards[0] = cardPack.nextCard();
                TableCards[1]=cardPack.nextCard();
                TableCards[2]=cardPack.nextCard();
            }
            if (round == 3)//River
            {
                TableCards[3] = cardPack.nextCard();
            }
            if (round == 4)//Turn
            {
                TableCards[4] = cardPack.nextCard();
            }
            if (round == 5)//END
            {
                endOfRound = true;
                Clienti[turnOrder].isTurn = false;
                return;
            }
            turnOrder = smallBlind;
            lastToEnd = GetPrevious(smallBlind);
            Clienti[turnOrder].isTurn = true;
            time = DateTime.Now.AddSeconds(-2);
            //for (int i = 0; i < Clienti.Length; i++)
            //{
            //    if (Clienti[i] != null)
            //    {
            //        GetHandStrenght(i);
            //    }
            //}
        }

        internal int bigBet;
        bool _endOfRound = false;
        bool endOfRound
        {
            get
            {
                return _endOfRound;
            }
            set
            {
                if (_endOfRound != value)
                {
                    _endOfRound = value;
                    if (_endOfRound == true)
                    {
                        restartRound();
                    }
                }
            }
        }

        private void restartRound()
        {
            Console.WriteLine("Restarting Game...");
            round = 0;
            SetWinner();            
            TableCards.Clear2();
            foreach (var p in Clienti)
            {
                if (p != null)
                {
                    p.HandStrenght = new handStrenght();
                    p.FoldCards(false, true);
                    p.isTurn = false;
                    p.tookDecision = false;
                    //p.SendAnim(ClientAnimations.NewCards);
                }
            }
            Thread.Sleep(1000);
            UpdateAll();
            Thread.Sleep(1000);
            //W8 2 sec Till next round           
            smallBlind = GetNext(smallBlind);
            lastToEnd = bigBlind;
            turnOrder = GetNext(bigBlind);
            Clienti[smallBlind].Bet(tableConfig.bigblind / 2);
            Clienti[bigBlind].Bet(tableConfig.bigblind);
            cardPack.Shuffle();
            for (int i = 0; i <= 1; i++)//TODO SPLIT FROM small blind
            {
                foreach (var p in Clienti)
                {
                    if (p != null)
                    {
                        p.folded = false;
                        p.SetCard(i, cardPack.nextCard());
                    }

                }
            }          
            Clienti[turnOrder].IsTurn = true;
            endOfRound = false;
            round = 1;
            UpdateAll();
        }

        public void Create(TableConfig _tableConfig)
        {
            for(int i = 0;i<TableCards.Length;i++)
            {
                TableCards[i] = Card.Null;
               
            }
            Clienti = new Client[_tableConfig.seats];
            tableConfig = _tableConfig;
            Thread TableLoop = new Thread(() =>
           {
               while (true)
               {
                   //Thread.Sleep(100);
                   if (turnTakers() > 1)
                   {
                       if (round == 0)
                       {
                           if (!endOfRound)
                           {
                               endOfRound = true;
                           }
                       }
                       if (Clienti[turnOrder].folded)
                       {
                           continue;
                       }
                       if (!Clienti[turnOrder].isActive)
                       {
                           Clienti[turnOrder].FoldCards(true);
                       }
                       if (!endOfRound)
                       {
                           //Inactive Check
                           if ((DateTime.Now > time.AddSeconds(tableConfig.handTime)))//TIME OVER
                           {
                               ///Fold cause inavctive
                               if (Clienti[GetPrevious(turnOrder)].onhandBet > Clienti[turnOrder].onhandBet)
                               {
                                   Clienti[turnOrder].FoldCards(true);
                               }
                               //Check Cause Inactive
                               if (Clienti[GetPrevious(turnOrder)].onhandBet == Clienti[turnOrder].onhandBet)
                               {
                                   Clienti[turnOrder].Check();
                               }
                               //turnOrder = GetNext(turnOrder); 
                           }

                       }
                   }
               }
           });
            TableLoop.Start();
        }
        private void UpdateAll()
        {
            foreach (var c in Clienti)
            {
                if (c != null)
                {
                    c.UpdateTable();
                }
            }
        }

        private void GameRestart()
        {
            // throw new NotImplementedException();
        }

        private void SetWinner()
        {
            for (int i = 0; i < Clienti.Length; i++)
            {
                if (Clienti[i] != null)
                {
                    GetHandStrenght(i);
                }
            }

            var best = (from p in Clienti where p != null select p).Max(x => x.HandStrenght.Strenght);
            var winners = (from p in Clienti where p!=null && p.HandStrenght.Strenght == best select p).ToList();
            if (winners.Count > 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (winners.Count > 1)
                    {
                        //var best2 = (from p in winners where p != null select p).Max(x => x.handStrenght.hcTotal[i]);
                       // var winners2 = (from p in winners where p.handStrenght.hcTotal[i] == best2 select p).ToList();
                       // winners = winners2;
                    }
                    else
                    {
                        break;
                    }
                }
                foreach (var winner in winners)
                {
                    winner.hasWon = true;
                    Console.WriteLine(winner.Name + " has won with " + winner.HandStrenght.Strenght.ToString());

                }
            }
            else
            {
                winners.Last().hasWon = true;
                Console.WriteLine(winners.Last().Name + " has won with " + winners.Last().HandStrenght.Strenght.ToString());
            }






        }

        private void GetHandStrenght(int i)
        {

            var client = Clienti[i];
            List<Card> carti = new List<Card>();
            carti.Add(client.card1);
            carti.Add(client.card2);
            foreach(var crd in tableCards)
            {
                carti.Add(crd);
            }
            #region royaleCheck
            //Check Red Color
            var crosii = (from c in carti where c.Culoare == Color.InimaRosie && c.Numar > 1 select c).ToList();
            if (crosii.Count() >= 5)
            {
                if (canFormChinta(crosii, out client.HandStrenght._HightCard1))
                {
                    if (client.HandStrenght.HightCard1.Numar == 14)
                    {
                        client.HandStrenght.Strenght = _HandStrenght.RoyaleFlush;
                    }
                    else
                    {
                        client.HandStrenght.Strenght = _HandStrenght.StraightFlush;
                    }
                    return;
                }
                else
                {
                    client.HandStrenght.Strenght = _HandStrenght.Flush;
                    client.HandStrenght.HightCard1 = crosii.OrderBy(x => x.Numar).Last();
                }
                
            }
            //Check black Color
            var cnegree = (from c in carti where c.Culoare == Color.InimaNeagra && c.Numar > 1 select c).ToList();
            if (cnegree.Count() >= 5)
            {
                if (canFormChinta(cnegree, out client.HandStrenght._HightCard1))
                {
                    if (client.HandStrenght.HightCard1.Numar == 14)
                    {
                        client.HandStrenght.Strenght = _HandStrenght.RoyaleFlush;
                    }
                    else
                    {
                        client.HandStrenght.Strenght = _HandStrenght.StraightFlush;
                    }
                    return;
                }
                else
                {
                    client.HandStrenght.Strenght = _HandStrenght.Flush;
                    client.HandStrenght.HightCard1 = cnegree.OrderBy(x => x.Numar).Last();
                }
            }
            //Check clubs Color
            var ctreflee = (from c in carti where c.Culoare == Color.Trefla && c.Numar > 1 select c).ToList();
            if (ctreflee.Count() >= 5)
            {
                if (canFormChinta(ctreflee, out client.HandStrenght._HightCard1))
                {
                    if (client.HandStrenght.HightCard1.Numar == 14)
                    {
                        client.HandStrenght.Strenght = _HandStrenght.RoyaleFlush;
                    }
                    else
                    {
                        client.HandStrenght.Strenght = _HandStrenght.StraightFlush;
                    }
                    return;
                }
                else
                {
                    client.HandStrenght.Strenght = _HandStrenght.Flush;
                    client.HandStrenght.HightCard1 = ctreflee.OrderBy(x => x.Numar).Last();
                }
            }
            //Check diamonds Color
            var croambee = (from c in carti where c.Culoare == Color.Romb && c.Numar > 1 select c).ToList();
            if (croambee.Count() >= 5)
            {
                if (canFormChinta(croambee, out client.HandStrenght._HightCard1))
                {
                    if (client.HandStrenght.HightCard1.Numar == 14)
                    {
                        client.HandStrenght.Strenght = _HandStrenght.RoyaleFlush;
                    }
                    else
                    {
                        client.HandStrenght.Strenght = _HandStrenght.StraightFlush;
                    }
                    return;
                }
                else
                {
                    client.HandStrenght.Strenght = _HandStrenght.Flush;
                    client.HandStrenght.HightCard1 = croambee.OrderBy(x => x.Numar).Last();
                }
            }
            #endregion
            #region straightCheck
            if (canFormChinta(carti, out client.HandStrenght._HightCard1))
            {
                client.HandStrenght.Strenght = _HandStrenght.Straight;
            }
            #endregion
            #region 4AKIND/Cuie/Full/2Perechi/OPereche
            GetFullStatus(carti, i);
            #endregion
        }

        private void GetFullStatus(List<Card> carti, int i)
        {
            var client = Clienti[i];
            
            int perechiCount = 0;
            int cuieCount = 0;
            var toCmp = carti.OrderByDescending(x => x.Numar).ToArray();
            foreach (var c in toCmp)
            {
                //cnt++;
                var p = (from r in toCmp where r.Numar == c.Numar select r).Count();
                if (p == 2)
                {
                    perechiCount++;
                }
                if (p == 3)
                {
                    cuieCount++;
                    client.HandStrenght.Strenght = _HandStrenght.Cuie;
                }
                    
                if (p == 4)
                {
                    client.HandStrenght.Strenght = _HandStrenght.FourAKind;                    
                }
            }
            perechiCount = perechiCount / 2;
            cuieCount = cuieCount / 3;
            if (perechiCount > 0 && cuieCount >0 || cuieCount > 1)
            {
                client.HandStrenght.Strenght = _HandStrenght.FullHouse;
            }
            if (perechiCount >= 2 && cuieCount < 1)
            {
                client.HandStrenght.Strenght = _HandStrenght.TwoPair;
            }
            if (perechiCount == 1 && cuieCount < 1)
            {
                client.HandStrenght.Strenght = _HandStrenght.Pair;
            }

        }

        private bool canFormChinta(List<Card> crosii, out Card hightCard1)
        {
            List<Card> comparer = crosii;
            List<Card> toad = new List<Card>();
            foreach (var card in crosii)
            {
                if (card.Numar == 14)
                {
                    toad.Add(new Card() { Numar = 1, Culoare = card.Culoare });
                }
            }
            foreach(var crd in toad)
            {
                comparer.Add(crd);
            }
            var toCmp = comparer.ToArray<Card>();
            toCmp = toCmp.OrderBy(x => x.Numar).ToArray();
            int cnt = 0;
            Card hc = Card.Null;
            for (int i = 1; i < toCmp.Count(); i++)
            {
                if (toCmp[i].Numar - 1 == toCmp[i - 1].Numar)
                {
                    cnt++;
                    hc = toCmp[i];
                }
                else
                {
                    if (cnt <= 4)
                        cnt = 0;
                    else
                    {
                        hightCard1 = hc;
                        return true;
                    }
                }
            }
            hightCard1 = hc;
            return false;
        }

        public void Destroy()
        {

        }

    }
}
