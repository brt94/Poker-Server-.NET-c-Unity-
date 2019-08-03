using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerCommonLibrary
{
    class PlayerStats
    {
        public long Money;
        public long clientUID;
        public string Name = "Cosmin";

        public override string ToString()
        {
            return clientUID+":"+Name+":"+Money+":";
        }

    }
}
