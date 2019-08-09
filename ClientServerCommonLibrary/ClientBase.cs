using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ClientServerCommonLibrary
{
    public delegate void tookChanged();
    public class ClientBase
    {
        public _HandStrenght handStrenght = _HandStrenght.NotSet;
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
        public int _onhandBet = 0;
        public int onhandBet
        {
            get
            {
                return _onhandBet;
            }
            set
            {
               if(value != onhandBet)
                {
                    _onhandBet = value;
                    onHandBetChanged?.Invoke();
                }
            }
        }
        public int Cash
        {
            get
            {
               return playerStats.Money;
            }
            set
            {
                if(value != playerStats.Money)
                {
                    playerStats.Money = value;
                    onCashChanged?.Invoke();
                }
            }
        }
        public Card _card1 = new Card();
        public Card card1
        {
            get
            {
                return _card1;
            }
            set
            {
                if(card1 != value)
                {
                    _card1 = value;
                    onCard1Changed?.Invoke();
                }
            }
        }
        public Card _card2 = new Card();
        public Card card2
        {
            get
            {
                return _card2;
            }
            set
            {
                if (card2 != value)
                {
                    _card2 = value;
                    onCard2Changed?.Invoke();
                }
            }
        }

        public int tableSeat = 0;
        public string Name
        {
            get
            {
                return playerStats.Name;
            }
            set
            {
                if(Name != value)
                {
                    playerStats.Name = value;
                    onNameChanged?.Invoke();
                }
            }
        }
        //
        public bool _isTurn = false;
        public bool isTurn
        {
            get
            {
                return _isTurn;
            }
            set
            {
                if (value != isTurn)
                {
                    _isTurn = value;
                    onTurnChanged?.Invoke();
                }
            }
        }
        private bool _isActive = false;  
        public bool isActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if(_isActive != value)
                {
                    _isActive = value;
                    onActiveChanged?.Invoke();
                }
            }
        }
        public bool _hasWon = false;
        public bool hasWon
        {
            get
            {
                return _hasWon;
            }
            set
            {
                if(value != _hasWon)
                {
                    _hasWon = value;
                    onPlayerWin?.Invoke();
                }
            }
        }

        private bool _folded = true;
        public bool folded
        {
            get
            {
                return _folded;
            }
            set
            {
                if(value != _folded)
                {
                    _folded = value;
                    onFoldChanged?.Invoke();
                }
            }
        }

       

        public bool _tookDecision = false;
        public bool tookDecision
        {
            get
            {
                return _tookDecision;
            }
            set
            {
                _tookDecision = value;
                tookDecisionChanged?.Invoke();
            }
        }
        public event tookChanged tookDecisionChanged;
        public event tookChanged onHandBetChanged;
        public event tookChanged onCashChanged;
        public event tookChanged onCard1Changed;
        public event tookChanged onCard2Changed;
        public event tookChanged onNameChanged;
        public event tookChanged onFoldChanged;
        public event tookChanged onActiveChanged;
        public event tookChanged onPlayerWin;
        public event tookChanged onTurnChanged;
        

        public PlayerStats playerStats = new PlayerStats();

        

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
                    return String.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:" +
                        "{10}:", isTurn, isActive, folded, onhandBet, card1, card2,Name,tablebigBet,hasWon,Cash,(int)handStrenght);
                    //break;
                case false:
                    return String.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:", isTurn, isActive, folded, onhandBet, Card.Null , Card.Null, Name, tablebigBet,hasWon,Cash,(int)handStrenght);
                    //break;
            }
            return "";
        }
    }
}
