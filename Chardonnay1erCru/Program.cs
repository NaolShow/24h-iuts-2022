using System;
using System.Collections.Generic;

namespace Chardonnay1erCru {

    internal class Program {

        private static NetworkManager Manager;

        private static void PickHighestValueCards() {

            // On récupère les scores avec et sans pioche
            Dictionary<CardType, double> scoresNotSommet = Manager.Deck.ScoreNotSommet();
            Dictionary<CardType, double> scoresSommet = Manager.Deck.ScoreSommet();

            // On calcule la différence de score
            Dictionary<CardType, double> scores = new Dictionary<CardType, double>();
            scores[CardType.Gamay] = scoresSommet[CardType.Gamay] - scoresNotSommet[CardType.Gamay];
            scores[CardType.Pinot] = scoresSommet[CardType.Pinot] - scoresNotSommet[CardType.Pinot];
            scores[CardType.Chardonnay] = scoresSommet[CardType.Chardonnay] - scoresNotSommet[CardType.Chardonnay];
            scores[CardType.Aligote] = scoresSommet[CardType.Aligote] - scoresNotSommet[CardType.Aligote];

            CardType bestCard = CardType.Gamay;
            double highestValue = 0;
            foreach (KeyValuePair<CardType, double> pair in scores) {

                if (pair.Value > highestValue) {
                    highestValue = pair.Value;
                    bestCard = pair.Key;
                }

            }

            double quantity = 0;
            int highestid = -1;
            for (int i = 0; i < Manager.Pick.Cards.Count; i++) {

                if (Manager.Pick.Cards[i].Type != bestCard) continue;

                if (highestid == -1 || Manager.Pick.Cards[i].Quantity > quantity) {

                    highestid = i;
                    quantity = Manager.Pick.Cards[i].Quantity;

                }

            }

            Manager.Pick.Get(highestid);

        }

        private static bool TryPick(Func<Card, bool> shouldPick) {

            // On boucle sur toutes les cartes
            for (int i = 0; i < Manager.Pick.Cards.Count; i++) {

                // Si on doit pick la carte
                if (shouldPick.Invoke(Manager.Pick.Cards[i])) {

                    // On la prend
                    Manager.Pick.Get(i);
                    return true;

                }

            }
            return false;

        }

        private static void PickHighestType(CardType type) {

            int highest = -1;
            for (int i = 0; i < Manager.Pick.Cards.Count; i++) {

                // Si c'est pas une carte que l'on veut
                if (Manager.Pick.Cards[i].Type != type) continue;

                if (highest == -1 || Manager.Pick.Cards[i].Quantity > highest) {
                    highest = i;
                }

            }
            Manager.Pick.Get(highest);

        }

        private static void Phase1() {

            // On tente de récupérer sabotage
            if (TryPick((card) => card.Type == CardType.Sabotage)) return;

            // Pinot 6 et 5
            if (TryPick((card) => card.Type == CardType.Pinot && card.Quantity >= 6)) return;
            if (TryPick((card) => card.Type == CardType.Pinot && card.Quantity >= 5)) return;
            if (TryPick((card) => card.Type == CardType.Pinot && card.Quantity >= 4)) return;
            if (TryPick((card) => card.Type == CardType.Pinot && card.Quantity >= 3)) return;

            // Chardonnay 6 et 5
            if (TryPick((card) => card.Type == CardType.Chardonnay && card.Quantity >= 6)) return;
            if (TryPick((card) => card.Type == CardType.Chardonnay && card.Quantity >= 5)) return;
            if (TryPick((card) => card.Type == CardType.Chardonnay && card.Quantity >= 4)) return;

            // Si nous avons moins de 2 bouteilles, on en prend une
            Dictionary<CardType, double> quantities = Manager.Deck.QA;
            if (quantities[CardType.Bouteille] < 2 && TryPick((card) => card.Type == CardType.Bouteille)) return;

            // Si c'est 3 bouteilles
            if (Manager.Pick.Cards[0].Type == CardType.Bouteille && Manager.Pick.Cards[1].Type == CardType.Bouteille && Manager.Pick.Cards[2].Type == CardType.Bouteille) {
                Manager.Pick.Get(1);
                return;
            }

            PickHighestValueCards();

        }

        private static void Phase2() {

            // On tente de récupérer sabotage
            if (TryPick((card) => card.Type == CardType.Sabotage)) return;

            // Pinot 6 et 5
            if (TryPick((card) => card.Type == CardType.Pinot && card.Quantity >= 6)) return;
            if (TryPick((card) => card.Type == CardType.Pinot && card.Quantity >= 5)) return;

            // Si on peut saboter, on sabote forcément la personne de gauche
            if (Manager.Deck.HaveSabotage) {

                Manager.Saboter(NetworkManager.Cible.GAUCHE);
                return;

            }

            Dictionary<CardType, double> totaux = Manager.Deck.ScoreNotSommet();

            Dictionary<CardType, double> quantities = Manager.Deck.QA;

            double highest = 0;
            CardType highestCardType = CardType.Gamay;
            foreach (KeyValuePair<CardType, double> pair in totaux) {

                if (pair.Key == CardType.Pinot) continue;

                if (pair.Value > highest) {

                    highest = pair.Value;
                    highestCardType = pair.Key;

                }

            }

            if (totaux[CardType.Pinot] > 150 || totaux[CardType.Pinot] > 80 && Manager.Deck.Cards.Count > 13) {

                Manager.Poser(CardType.Pinot);
                return;

            } else if ((highest > 35 && quantities[CardType.Bouteille] >= 2) || highest > 80) {

                Manager.Poser(highestCardType);
                return;

            }

            // Chardonnay 6 et 5
            if (TryPick((card) => card.Type == CardType.Chardonnay && card.Quantity >= 6)) return;
            if (TryPick((card) => card.Type == CardType.Chardonnay && card.Quantity >= 5)) return;

            // On vérifie les quantités si on a moins de 2 bouteilles on prend
            if (quantities[CardType.Bouteille] < 2 && TryPick((card) => card.Type == CardType.Bouteille)) return;

            // Si c'est 3 bouteilles
            if (Manager.Pick.Cards[0].Type == CardType.Bouteille && Manager.Pick.Cards[1].Type == CardType.Bouteille && Manager.Pick.Cards[2].Type == CardType.Bouteille) {
                Manager.Pick.Get(1);
                return;
            }

            PickHighestValueCards();

        }

        private static int GetBaddestCard() {

            // On choppe le pire type de carte que l'on a
            Dictionary<CardType, double> cards = Manager.Deck.ScoreNotSommet();
            CardType badType = CardType.Gamay;
            double badValue = int.MaxValue;
            foreach (KeyValuePair<CardType, double> pair in cards) {

                if (pair.Value < badValue) {
                    badType = pair.Key;
                    badValue = pair.Value;
                }

            }

            // On boucle sur tout le deck
            double baddestQuantity = int.MaxValue;
            int baddestIndex = 0;
            for (int i = 0; i < Manager.Deck.Cards.Count; i++) {

                // Si c'est du mauvais type
                if (badType != Manager.Deck.Cards[i].Type) continue;

                if (baddestQuantity > Manager.Deck.Cards[i].Quantity) {
                    baddestQuantity = Manager.Deck.Cards[i].Quantity;
                    baddestIndex = i;
                }

            }
            Manager.Deck.RemoveCard(baddestIndex);
            return baddestIndex;
        }

        private static void Phase3() {

            int cardCount = Manager.Deck.Cards.Count;

            Dictionary<CardType, double> quantities = Manager.Deck.QuantiteAdditionee();

            List<int> bouteilles = new List<int>();
            if (quantities[CardType.Bouteille] > 3) {

                for (int i = 0; i < cardCount; i++) {

                    if (Manager.Deck.Cards[i].Type == CardType.Bouteille) bouteilles.Add(i);

                }

            }

            if (bouteilles.Count > 1) Manager.Defausser(bouteilles[0], bouteilles[1]);
            else if (bouteilles.Count > 0) Manager.Defausser(bouteilles[0]);
            else if (cardCount == 15) Manager.Defausser(GetBaddestCard());
            else if (cardCount == 16) Manager.Defausser(GetBaddestCard(), GetBaddestCard());
            else Manager.PasserTour();

        }

        static void Main(string[] args) {

            try {

                // On initialise la connexion au serveur de jeu et on s'inscrit
                Manager = new NetworkManager();
                Manager.Join();

                while (true) {

                    // On attends le début de notre tour de jeu
                    Manager.WaitForTurn();

                    Phase1();
                    Phase2();
                    Phase3();

                }

                Console.ReadLine();

            } catch (Exception ex) {

                Console.WriteLine(ex);
                Console.ReadLine();

            }



        }

    }

}
