using ClientServerCommonLibrary;
using System;

namespace PokerServer
{
    public class CardPack
    {
        int lastCard = 0;
        public Card[] Carti = new Card[52];
        public Card nextCard()
        {
            lastCard++;
            return Carti[lastCard];
        }
        internal void Shuffle()
        {
            int rnd = new Random().Next(100, 300);
            for (int i = 0; i <= rnd; i++)
            {
                int r1 = new Random().Next(0, 52);                
                int r2 = new Random().Next(0, 52);
                while(r2==r1)
                {
                    r2 = new Random().Next(0, 52);
                }
                if (r1 != r2)
                {
                    var aux = Carti[r1];
                    Carti[r1] = Carti[r2];
                    Carti[r2] = aux;
                }
            }
            lastCard = 0;
        }
        public CardPack()
        {
            int cnt = 0;
            for (int i = 1; i <= 4; i++)
            {
                //2  =2 .... A = 14
                for (int y = 2; y <= 14; y++)
                {
                    Carti[cnt] = new Card();
                    Carti[cnt].Culoare = (Color)i;
                    Carti[cnt].Numar = y;
                   

                    cnt++;
                }
            }
        }
    }
}