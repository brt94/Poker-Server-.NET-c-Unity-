namespace ClientServerCommonLibrary
{
    public enum Color
    {
        Fliped =0,
        InimaRosie =1,
        InimaNeagra,
        Romb,
        Trefla,
    }   
    public class Card
    {
        public Color Culoare = Color.InimaNeagra;
        public int Numar = 0;

        public static Card Null =         
                new Card()
                {
                    Culoare = Color.Fliped,
                    Numar = 0
                };
            
        
            
        

        public override string ToString()
        {
            return Numar + "." + (int)Culoare;//.ToString();
        }
        public Card()
        {
            Culoare = Color.InimaNeagra;
            Numar = 0;
        }
    }
}