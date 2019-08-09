using ClientServerCommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ClientServerCommonLibrary
{
    public static class Extensii
    {
        public static long ToLong(this string str)
        {
            return Convert.ToInt64(str);
        }
        public static bool ToBool(this string str)
        {
            return bool.Parse(str);
        }
        public static Card ToCard(this string str)
        {
            var q = str.Split('.');
            return new Card() {Culoare = (Color)(int.Parse(q[0])),Numar = int.Parse(q[1])};
        }
        
        //public static Card Max(this List<Card> cards)
        //{
        //    //var o = (from c in cards where c.Numar > 1 select c.Numar).Max();
        //    return new Card();
        //}
    }
}
