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

        Aligote,
        Chardonnay,
        Gamay,
        Pinot

    }

    public class Card {

        public bool IsGrape { get; set; }
        public CardType Type { get; set; }
        public int Quantity { get; set; }

        /// <summary>
        /// Créer une carte "bouteille vide"
        /// </summary>
        public Card() {

            Type = CardType.Bouteille;
            IsGrape = false;

        }

        /// <summary>
        /// Créer une carte raisin ou bouteille avec nom et quantité
        /// </summary>
        public Card(bool isGrape, CardType card, int quantity) {

            // On indique si c'est un raisin ou une bouteille
            IsGrape = isGrape;

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

            // Si on a qu'un seul argument
            // => C'est une bouteille vide
            if (splitted.Length == 1) return new Card();

            // On renvoie la carte
            return new Card(splitted[0].Equals("RAISIN"), splitted[1].ToCardType(), int.Parse(splitted[2]));

        }

    }
}
