using System;

namespace PokerServer
{
    public class TableConfig
    {
        public string Name = "";
        public int buyin = 10000;
        public int seats = 6;
        public int bigblind = 50;
        public int ante;
        public int handTime = 20;

        internal bool SimilarTo(TableConfig tc)
        {
           if(buyin == tc.buyin)
            {
                if(seats == tc.seats)
                {
                    //if(bigblind == tc.bigblind)
                    //{
                        return true;
                    //}
                }
            }
            return false;
        }
    }
}