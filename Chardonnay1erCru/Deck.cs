using System.Collections.Generic;
using System.Linq;

namespace Chardonnay1erCru {
    public class Deck {

        private NetworkManager Manager;
        public Deck(NetworkManager manager) => Manager = manager;

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
        /// Renvoie vrai si nous disposons dans notre deck d'une carte sabotage
        /// </summary>
        public bool HaveSabotage {
            get {

                // On boucle sur toutes les cartes
                foreach (Card card in _Cards) {

                    // Si c'est une bouteille vide, on renvoie vrai
                    if (card.Type == CardType.Sabotage) return true;

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

        public Dictionary<CardType, double> QA => QuantiteAdditionee();

        public Dictionary<CardType, double> QuantiteAdditionee() {
            Dictionary<CardType, double> sommedic = new Dictionary<CardType, double>();
            sommedic.Add(CardType.Aligote, 0);
            sommedic.Add(CardType.Chardonnay, 0);
            sommedic.Add(CardType.Gamay, 0);
            sommedic.Add(CardType.Pinot, 0);
            sommedic.Add(CardType.Bouteille, 0);

            for (int i = 0; i < Cards.Count; i++) {
                switch (this.Cards[i].Type) {
                    case CardType.Aligote:
                        sommedic[CardType.Aligote] += Cards[i].Quantity;
                        break;
                    case CardType.Chardonnay:
                        sommedic[CardType.Chardonnay] += Cards[i].Quantity;
                        break;
                    case CardType.Gamay:
                        sommedic[CardType.Gamay] += Cards[i].Quantity;
                        break;
                    case CardType.Pinot:
                        sommedic[CardType.Pinot] += Cards[i].Quantity;
                        break;
                    case CardType.Bouteille:
                        sommedic[CardType.Bouteille]++;
                        break;
                }
            }

            return sommedic;

        }

        public Dictionary<CardType, double> ScoreNotSommet() {

            Dictionary<CardType, double> sommedic = QA;

            sommedic[CardType.Aligote] = sommedic[CardType.Aligote] * sommedic[CardType.Aligote] * 0.8;
            sommedic[CardType.Chardonnay] = sommedic[CardType.Chardonnay] * sommedic[CardType.Chardonnay];
            sommedic[CardType.Gamay] = sommedic[CardType.Gamay] * sommedic[CardType.Gamay] * 0.9;
            sommedic[CardType.Pinot] = sommedic[CardType.Pinot] * sommedic[CardType.Pinot] * 1.2;

            return sommedic;
        }

        public Dictionary<CardType, double> ScoreSommet() {

            Dictionary<CardType, double> sommedic = QA;

            foreach (Card card in Manager.Pick.Cards) sommedic[card.Type] += card.Quantity;

            sommedic[CardType.Aligote] = sommedic[CardType.Aligote] * sommedic[CardType.Aligote] * 0.8;
            sommedic[CardType.Chardonnay] = sommedic[CardType.Chardonnay] * sommedic[CardType.Chardonnay];
            sommedic[CardType.Gamay] = sommedic[CardType.Gamay] * sommedic[CardType.Gamay] * 0.9;
            sommedic[CardType.Pinot] = sommedic[CardType.Pinot] * sommedic[CardType.Pinot] * 1.2;

            return sommedic;

        }

    }

}