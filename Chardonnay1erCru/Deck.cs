using System.Collections.Generic;
using System.Linq;

namespace Chardonnay1erCru {
    public class Deck {

        private List<Card> _Cards = new List<Card>();
        /// <summary>
        /// Liste des cartes que nous avons
        /// </summary>
        public IReadOnlyList<Card> Cards => _Cards;

        public void Refresh(string msg) {

            // On récupère tous les arguments
            string[] splitted = msg.Split('|');

            // On boucle dessus (or le "OK") et on les ajoutes en carte
            foreach (string split in splitted.Skip(1)) _Cards.Add(Card.GetCard(split));

        }

    }

}