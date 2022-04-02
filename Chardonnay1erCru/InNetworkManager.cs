using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Chardonnay1erCru {

    public class InNetworkManager {

        /// <summary>
        /// Détermine si cette IA a été lancée la première ou non (est le serveur)
        /// </summary>
        public bool IsServer => Listener != null;

        private TcpListener Listener;
        private TcpClient Client;

        private StreamReader InStream;
        private StreamWriter OutStream;

        public InNetworkManager() {

            // On initialise un serveur
            Listener = new TcpListener(IPAddress.Loopback, 17539);

            try {

                // On lance un serveur et on attend que la deuxième IA se lance
                Listener.Start();
                Console.WriteLine($"[Intramuros] Création du serveur");
                Client = Listener.AcceptTcpClient();
                Console.WriteLine($"[Intramuros] Client connecté au serveur");

            } catch (SocketException) {

                // On supprime la référence du serveur
                Listener = null;

                // On initialise un client et on se connecte
                Client = new TcpClient("127.0.0.1", 17539);

                Console.WriteLine($"[Intramuros] Connexion du client au serveur");

            }

            // On récupère les deux streams
            NetworkStream stream = Client.GetStream();
            InStream = new StreamReader(stream);
            OutStream = new StreamWriter(stream);

        }

    }

}
