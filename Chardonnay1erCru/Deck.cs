using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chardonnay1erCru
{
    public class Deck
    {
        public List<Card> deck;

        public Deck()
        {
            deck = new List<Card>();
        }

        public void Refresh(string msg)
        {
            string[] splitted = msg.Split('|');

            foreach (string split in splitted.Skip(1))
            {
                deck.Add(Card.GetCard(split));
            }
        }
    }
}
