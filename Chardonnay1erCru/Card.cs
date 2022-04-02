namespace Chardonnay1erCru {

    public static class CardTypeExtensions {

        public static CardType ToCardType(this string typeString) {

            // On convertit en enumeration
            if (typeString.Equals("Pinot")) return CardType.Pinot;
            else if (typeString.Equals("Chardonnay")) return CardType.Chardonnay;
            else if (typeString.Equals("Gamay")) return CardType.Gamay;
            else return CardType.Aligote;

        }

    }

    public enum CardType {

        Bouteille,
        Sabotage,

        Aligote,
        Chardonnay,
        Gamay,
        Pinot

    }

    public class Card {

        /// <summary>
        /// Détermine si la carte est du raisin ou non
        /// </summary>
        public bool IsGrape => Type != CardType.Bouteille && Type != CardType.Sabotage;
        public CardType Type { get; set; }
        public int Quantity { get; set; }

        /// <summary>
        /// Créer une carte du type donné<br/>
        /// (A utiliser seulement pour sabotage et bouteille vide)
        /// </summary>
        public Card(CardType type) => Type = type;

        /// <summary>
        /// Créer une carte raisin ou bouteille avec nom et quantité
        /// </summary>
        public Card(CardType card, int quantity) {

            Type = card;
            Quantity = quantity;

        }

        public override string ToString() => $"Type={Type};Quantity={Quantity};IsGrape={IsGrape}";

        /// <summary>
        /// Renvoie une carte à partir de son string
        /// </summary>
        public static Card GetCard(string cardText) {

            // On split le texte avec les ;
            string[] splitted = cardText.Split(';');

            // Si on a qu'un seul argument (soit sabotage ou bouteille)
            if (splitted.Length == 1) {

                // Si c'est une bouteille
                if (splitted[0].Equals("BOUTEILLE")) return new Card(CardType.Bouteille);
                // Si c'est un sabotage
                else return new Card(CardType.Sabotage);

            }

            // On renvoie la carte
            return new Card(splitted[1].ToCardType(), int.Parse(splitted[2]));

        }

    }
}
