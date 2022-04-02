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

        public void Defausser(int x1, int x2)
        {
            OutStream.WriteLine("DEFAUSSER|")
        }

    }

}
