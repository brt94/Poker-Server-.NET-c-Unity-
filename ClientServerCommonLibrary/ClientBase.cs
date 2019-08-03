using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ClientServerCommonLibrary
{    
    public class ClientBase
    {
        public int tablebigBet;
        public long clientUID
        {
            get
            {
                return playerStats.clientUID;
            }
            set
            {
                playerStats.clientUID = value;
            }
        }
        //USED FOR GAMEPLAY 
        //ONHAND
        //[DataMember]
        public int onhandBet = 0;
        public Card card1 = new Card();
        public Card card2 = new Card();
        public int tableSeat = 0;
        public string Name = "Cosmin";
        //
        public bool isTurn = false;
        public bool isActive = true;  

        public bool folded = true;
        public bool tookDecision;

        private PlayerStats playerStats = new PlayerStats();

        public void UpdatePlayer(string upstring)
        {
            //  0           1        2        3         4     5           6       7
            //updateSelf:tableSeat,isTurn, isActive, folded, onhandBet, card1, card2

            var cmd = upstring.Split(':');
            tableSeat = int.Parse(cmd[1]);
            isTurn = cmd[2].ToBool();
            isActive = cmd[3].ToBool();
            folded = cmd[4].ToBool();
            onhandBet = int.Parse(cmd[5]);
            card1 = cmd[6].ToCard();
            card2 = cmd[7].ToCard();
            Name = cmd[8];
            tablebigBet = int.Parse(cmd[9]);


            //s0->8
        }

        public override string ToString()
        {
            return ToString(false);
        }
        public string ToString(bool showCards = false)
        {
            //TODO -> 
            //Bitshitfting for booleans and cardsID
           StringBuilder ss = new StringBuilder();
           switch(showCards)
            {
                case true:
                    return String.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:", isTurn, isActive, folded, onhandBet, card1, card2,Name,tablebigBet);
                    //break;
                case false:
                    return String.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:", isTurn, isActive, folded, onhandBet, Card.Null , Card.Null, Name, tablebigBet);
                    //break;
            }
            return "";
        }
    }
}
