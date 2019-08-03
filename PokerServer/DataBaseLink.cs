using System;

namespace PokerServer
{  
    public class DataBaseLink
    {
        public static int uid = 1001;
        private long money = 100000;
        public long Money
        {
            get
            {
                return money;
            }
            set
            {
                money = value;
            }
        }

        public object Name { get; internal set; }

        public int UID = 0;

        internal void register(string v)
        {
            //GET links based on V=userUID
            uid++;
            UID = uid;
                
        }
    }
}