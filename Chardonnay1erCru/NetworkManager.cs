using System.IO;
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

        /// <summary>
        /// Enumeration du joueur à attaquer
        /// </summary>
        public enum Cible
        {
            GAUCHE,
            DROITE
        }

        /// <summary>
        /// Methode pour poser une carte. Envoie la requête "POSER" au serveur
        /// </summary>
        /// <param name="bouteille">La bouteille qui sera posée</param>
        public void Poser(Bouteille bouteille)
        {
            OutStream.WriteLine($"POSER|{bouteille}");
        }

        /// <summary>
        /// Sabote un joueur. Envoie la requête "SABOTER" au serveur
        /// </summary>
        /// <param name="cible">La cible a saboter</param>
        public void Saboter(Cible cible)
        {
            switch (cible)
            {
                case Cible.GAUCHE:
                    OutStream.WriteLine($"SABOTER|-1");
                    break;
                case Cible.DROITE:
                    OutStream.WriteLine($"SABOTER|1");
                    break;
            }
        }

    }

}