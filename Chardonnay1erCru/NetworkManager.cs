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

        public Deck Deck { get; }
        public Pick Pick { get; }

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

            // On initialise le deck
            Deck = new Deck(this);
            Pick = new Pick(this);

        }

        /// <summary>
        /// Renvoie un message du serveur (bloque le thread en attendant)
        /// </summary>
        public string GetMessage() => InStream.ReadLine();
        /// <summary>
        /// Envoie un message au serveur
        /// </summary>
        public void SendMessage(string message) => OutStream.WriteLine(message);

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

        public void PasserTour() {

            // On défausse une carte
            OutStream.WriteLine("DEFAUSSER");

            // On récupère notre main
            Deck.Refresh(InStream.ReadLine());

        }

        public void Defausser(int x) {

            // On défausse une carte
            OutStream.WriteLine($"DEFAUSSER|{x}");

            // On récupère notre main
            Deck.Refresh(InStream.ReadLine());

        }

        public void Defausser(int x1, int x2) {

            // On défausse une carte
            OutStream.WriteLine($"DEFAUSSER|{x1}|{x2}");

            // On récupère notre main
            Deck.Refresh(InStream.ReadLine());

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
        /// <param name="cardType">La carte qui sera posée</param>
        public void Poser(CardType cardType) {

            if (cardType == CardType.Aligote) OutStream.WriteLine("POSER|Aligoté");
            else OutStream.WriteLine($"POSER|{cardType}");

            // On lit notre jeu et on refresh le deck
            Deck.Refresh(InStream.ReadLine());

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

            // On lit notre jeu et on refresh le deck
            Deck.Refresh(InStream.ReadLine());

        }

        /// <summary>
        /// Bloque le thread appellant jusqu'au début du tour de jeu<br/>
        /// Au moment ou le tour de jeu commence, raffraichie les listes de <see cref="Pick"/> et <see cref="Deck"/>
        /// </summary>
        public void WaitForTurn() {

            // On attend que l'on reçois le message début tour
            string message = string.Empty;
            while (!message.StartsWith("DEBUT_TOUR")) message = InStream.ReadLine();

            // On refresh le deck et le pick
            Deck.Refresh(message);
            Pick.Refresh();

        }

    }

}
