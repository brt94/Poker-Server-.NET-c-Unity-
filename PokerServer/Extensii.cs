using ClientServerCommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerServer
{
    public static class Extensii
    {
        public static void Clear2(this List<Card> cards)
        {
            Card nl = new Card() { Culoare = Color.InimaNeagra, Numar = 0 };
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i] = nl;
            }
            cards.Clear();
        }
    }
}
