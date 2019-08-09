using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ClientServerCommonLibrary
{
    public class PlayerStats
    {
        public int Money = 1500;
        public long clientUID;

        public string Name { get; internal set; }
        // public string Name = "";

        //public override string ToString()
        //{
        //    return clientUID+":"+Money+":";
        //}

    }
}
