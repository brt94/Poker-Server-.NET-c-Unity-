using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using ClientServerCommonLibrary;

namespace PokerClient
{
    /// <summary>
    /// Static Table 
    /// Clients Manipulation
    /// GetTableProperties LIKE : 
    /// ->SB/BB
    /// ->HandTimer
    /// ->POT?!?//Calculate from player bets? faster...
    /// </summary>
    public class Table
    {
        public static int BB = 25;
        public static int HandTime = 10;

        public static Card F1 = new Card();
        public static Card F2 = new Card();
        public static Card F3 = new Card();
        public static Card T = new Card();
        public static Card R = new Card();
        internal static void RefreshCards(string datarec)
        {
            var ps = datarec.Split(':');
            F1 = ps[1].ToCard();
            F2 = ps[2].ToCard();
            F3 = ps[3].ToCard();
            T = ps[4].ToCard();
            R = ps[5].ToCard();

        }

        //public static 
        public static List<ClientBase> Clienti = new List<ClientBase>();
        //  public static bool init = false;
        //  0           1        2        3        4         5        6      7        8
        //updateSelf:tableSeat,isTurn, isActive, folded, onhandBet, card1, card2
        internal static void Refresh(string datarec)
        {
            datarec = datarec.Remove(0,7);
            var ps = datarec.Split('|');
            foreach(var playerInfo in ps)
            {
                try
                {
                    var playerInfos = playerInfo.Split(':');
                    var player = Clienti[int.Parse(playerInfos[0])];
                    player.isTurn = playerInfos[1].ToBool();
                    player.isActive = playerInfos[2].ToBool();
                    player.folded = playerInfos[3].ToBool();
                    player.onhandBet = int.Parse(playerInfos[4]);
                    player.card1 = playerInfos[5].ToCard();
                    player.card2 = playerInfos[6].ToCard();
                    player.Name = playerInfos[7];
                    player.hasWon = playerInfos[8].ToBool();
                    player.Cash = int.Parse(playerInfos[9]);
                }
                catch
                {

                }
            }
        }
    }
}
