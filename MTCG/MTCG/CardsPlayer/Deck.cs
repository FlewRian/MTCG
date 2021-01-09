using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class Deck
    {
        public List<BaseCard> PlayerDeck { get; set; }
        public int MaxCards { get; set; }

        public Deck()
        {
            PlayerDeck = new List<BaseCard>();
            MaxCards = 4;
        }
    }
}