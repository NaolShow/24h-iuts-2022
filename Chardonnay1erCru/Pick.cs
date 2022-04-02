using System;
using System.Collections.Generic;
using System.Linq;

namespace Chardonnay1erCru {

    public class Pick {

        private NetworkManager Manager;
        public Pick(NetworkManager manager) => Manager = manager;

        private List<Card> _Cards = new List<Card>();
        /// <summary>
        /// Liste des cartes qui sont disponibles au centre
        /// </summary>
        public IReadOnlyList<Card> Cards => _Cards;

        public Card Get(int cardId) {

            // On pioche et on récupère la carte prise
            Manager.SendMessage($"PIOCHER|{cardId}");
            return Card.GetCard(Manager.GetMessage().Split('|')[1]);

        }

        public void Refresh() {

            // On clear la liste de cartes
            _Cards.Clear();

            // On demande le sommet et on récupère le string splitté
            Manager.SendMessage("SOMMET");
            string[] splitted = Manager.GetMessage().Split('|');

            // Si la réponse de la commande n'est pas bonne
            if (splitted[0] != "OK") throw new Exception("Une erreur est survenue lors du sommet?");

            // On boucle sur tous les arguments
            // => On ajoute la carte à la liste des sommets
            foreach (string cardText in splitted.Skip(1)) _Cards.Add(Card.GetCard(cardText));

        }

    }

}
