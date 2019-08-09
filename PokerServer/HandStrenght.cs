using ClientServerCommonLibrary;

namespace PokerServer
{
    
    public class handStrenght
    {
        public _HandStrenght _Strenght = _HandStrenght.NotSet;
        public _HandStrenght Strenght
        {
            get
            {
                return _Strenght;
            }
            set
            {
                if (value > _Strenght || value == _HandStrenght.NotSet)
                {
                    _Strenght = value;
                }
            }
        }
        //public event void DoShit();
        public Card _HightCard1;
        public Card HightCard1
        {
            get
            {
                return _HightCard1;
            }
            set
            {
                hcTotal[4] = hcTotal[3];
                hcTotal[3] = hcTotal[2];
                hcTotal[2] = hcTotal[1];
                hcTotal[1] = hcTotal[0];
                hcTotal[0] = value;
               
            }
        }
        public Card HightCard2; //= //0;
        public Card HightCard3;
        public Card HightCard4;
        public Card HightCard5;
        //public int Kicker = 0;
        internal Card[] hcTotal
        {
            get
            {
                return new Card[] { HightCard1, HightCard2, HightCard3, HightCard4, HightCard5 };
            }
        }
        int cnt = 0;
        //public Card Next
        //{
        //    set
        //    {
        //        hcTotal[cnt] = value;
        //        cnt++;
        //    }
        //}
    }
}