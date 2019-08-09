using System;
using System.Collections.Generic;

namespace ClientServerCommonLibrary
{
    public class CardComparer : IComparer<Card>
    {
        public int Compare(Card x, Card y)
        {
            if (x.Numar == 0 || y.Numar == 0)
            {
                return 0;
            }
            return x.CompareTo(y);
        }
    }

    public enum Color
    {
        Fliped =0,
        InimaRosie =1,
        InimaNeagra,
        Trefla,
        Romb,
      
    }   
    public class Card : IComparer<Card>
    {
        public Color Culoare = Color.InimaNeagra;
        public int Numar = 0;

        public static Card Null =         
                new Card()
                {
                    Culoare = Color.Fliped,
                    Numar = 0
                };




        bool hightLight = false;

        public override string ToString()
        {
            return (int)Culoare + "." + Numar;//.ToString();
        }

        internal int CompareTo(Card y)
        {
            throw new NotImplementedException();
        }

        public int Compare(Card x, Card y)
        {
            throw new NotImplementedException();
        }

        public Card()
        {
            Culoare = Color.InimaNeagra;
            Numar = 0;
        }
    }
}