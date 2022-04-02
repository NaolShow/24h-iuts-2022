using System.Collections.Generic;
using System.Linq;

namespace Chardonnay1erCru {
    public class Deck {

        private List<Card> _Cards = new List<Card>();
        /// <summary>
        /// Liste des cartes que nous avons
        /// </summary>
        public IReadOnlyList<Card> Cards => _Cards;

        /// <summary>
        /// Renvoie vrai si notre deck est plein
        /// </summary>
        public bool IsFull => _Cards.Count >= 16;

        /// <summary>
        /// Renvoie vrai si nous disposons dans notre deck d'une bouteille vide
        /// </summary>
        public bool HaveEmptyBottle {
            get {

                // On boucle sur toutes les cartes
                foreach (Card card in _Cards) {

                    // Si c'est une bouteille vide, on renvoie vrai
                    if (card.Type == CardType.Bouteille) return true;

                }
                return false;

            }
        }

        /// <summary>
        /// Ajoute une carte au deck car nous venons de la piochée
        /// </summary>
        public void AddPickedCard(Card card) => _Cards.Add(card);
        /// <summary>
        /// Rafraichie la liste des cartes du deck
        /// </summary>
        public void Refresh(string msg) {

            // Clear le deck
            _Cards.Clear();

            // On récupère tous les arguments
            string[] splitted = msg.Split('|');

            // On boucle dessus (or le "OK") et on les ajoutes en carte
            foreach (string split in splitted.Skip(1)) _Cards.Add(Card.GetCard(split));

        }

    }

}