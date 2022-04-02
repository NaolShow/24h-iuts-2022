using System.Collections.Generic;
using System.Linq;

namespace Chardonnay1erCru {
    public class Deck {

        public List<Card> Cards = new List<Card>();

        public void Refresh(string msg) {
            string[] splitted = msg.Split('|');

            foreach (string split in splitted.Skip(1)) {
                Cards.Add(Card.GetCard(split));
            }
        }
    }
}
