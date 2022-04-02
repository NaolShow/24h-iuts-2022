using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace Chardonnay1erCru {

    public class NetworkManager {

        /// <summary>
        /// Identifiant du joueur (uniquement accessible après avoir appelé <see cref="Join"/>)
        /// </summary>
        public int PlayerID { get; private set; }

        private TcpClient Client;
        private StreamReader InStream;
        private StreamWriter OutStream;

        /// <summary>
        /// Creer une instance de Network Manager qui se connecte au jeu
        /// </summary>
        public NetworkManager() {

            // On initialise le client
            Client = new TcpClient("127.0.0.1", 1234);

            // On récupère le flux entrant et sortant
            NetworkStream stream = Client.GetStream();
            InStream = new StreamReader(stream);
            OutStream = new StreamWriter(stream) {
                AutoFlush = true
            };

        }

        /// <summary>
        /// Renvoie un message du serveur (bloque le thread en attendant)
        /// </summary>
        public string GetMessage() => InStream.ReadLine();

        /// <summary>
        /// Rejoint le serveur de jeu et renvoie vrai si cela s'est passé correctement
        /// </summary>
        public bool Join() {

            // On demande à rejoindre la partie
            OutStream.WriteLine("INSCRIRE");

            // On récupère le status et on le split
            string[] splittedStatus = InStream.ReadLine().Split('|');

            // On récupère le player id
            PlayerID = int.Parse(splittedStatus[1]);
            return splittedStatus[0] == "OK";

        }


        public void Defausser(int x1, int x2)
        {
            OutStream.WriteLine("DEFAUSSER|")
        }
        /// <summary>
        /// Enumeration du joueur à attaquer
        /// </summary>
        public enum Cible {
            GAUCHE,
            DROITE
        }

        /// <summary>
        /// Methode pour poser une carte. Envoie la requête "POSER" au serveur
        /// </summary>
        /// <param name="card">La carte qui sera posée</param>
        public void Poser(Card card) {
            OutStream.WriteLine($"POSER|{card.Type}");
        }

        /// <summary>
        /// Sabote un joueur. Envoie la requête "SABOTER" au serveur
        /// </summary>
        /// <param name="cible">La cible a saboter</param>
        public void Saboter(Cible cible) {

            switch (cible) {
                case Cible.GAUCHE:
                    OutStream.WriteLine($"SABOTER|-1");
                    break;
                case Cible.DROITE:
                    OutStream.WriteLine($"SABOTER|1");
                    break;
            }

        }

        /// Attend le début du tour de jeu
        /// </summary>
        public void WaitForTurn() {
            while (InStream.ReadLine() != "DEBUT_TOUR") System.Threading.Thread.Sleep(20);
        }

        public List<Card> WinesSommet = new List<Card>();
        /// <summary>
        /// Met à jour la liste des vins (uniquement lorsque c'est notre tour)
        /// </summary>
        public void Sommet() {

            // On clear la liste des sommets
            WinesSommet.Clear();

            // On demande le sommet et on récupère le string splitté
            OutStream.WriteLine("SOMMET");
            string[] splitted = InStream.ReadLine().Split('|');

            // Si la réponse de la commande n'est pas bonne
            if (splitted[0] != "OK") throw new Exception("Une erreur est survenue lors du sommet?");

            // On boucle sur tous les arguments
            // => On ajoute la carte à la liste des sommets
            foreach (string cardText in splitted.Skip(1)) WinesSommet.Add(Card.GetCard(cardText));

        }

    }

}
